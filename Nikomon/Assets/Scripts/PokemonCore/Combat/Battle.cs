using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PokemonCore.Combat.Interface;
using PokemonCore.Utility;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace PokemonCore.Combat
{
    public enum BattleResults
    {
        Continue,
        Failed,
        Succeed,
        Aborted,
        Ran,
        Captured,
    }


    public enum BattleActions
    {
        Choosing,
        Moving,
        Damaging,
        Damaged,
        Moved
    }

    [Serializable]
    public enum Command
    {
        Move,
        Items,
        SwitchPokemon,
        GoPokemon,
        Run,
        Skip
    }

    [Serializable]
    public class Instruction
    {
        public int CombatPokemonID;
        public Command command;
        /// <summary>
        /// 这里的ID指的是不同Command下的ID，比如对于Move这里的id就是0，1，2，3代表宝可梦不同招式；对于Items就是Items本身的ID
        /// </summary>
        public int ID;
        public List<int> target;

        [JsonConstructor]
        public Instruction(int combatPokemonID, Command command, int id, List<int> target)
        {
            this.CombatPokemonID = combatPokemonID;
            this.command = command;
            this.ID = id;
            this.target = target;
        }

        public Instruction(int combatPokemonID, Command command, int id, int target)
        {
            this.CombatPokemonID = combatPokemonID;
            this.command = command;
            this.ID = id;

            this.target = new List<int>();
            this.target.Add(target);
        }

        public override string ToString()
        {
            return $"{CombatPokemonID}----{command}-----{ID}";
        }
    }

    /// <summary>
    /// Battle类，集中处理所有战斗逻辑
    /// </summary>
    public class Battle
    {
        public static Battle Instance;

        public List<CombatPokemon> alliesPokemons { get; private set; }
        public List<CombatPokemon> opponentsPokemons { get; private set; }

        #region Effect相关

        /// <summary>
        /// 选择招式之前的Effect
        /// </summary>
        public List<IEffect> ChoosingEffect { get; private set; }

        /// <summary>
        /// 招式生效之前的Effect->可以修改招式的Type,威力,Target等
        /// </summary>
        public List<IEffect> MovingEffect { get; private set; }

        /// <summary>
        /// 伤害造成前的Effect->可以阻止或削减伤害的程度等
        /// </summary>
        public List<IEffect> DamagingEffect { get; private set; }

        /// <summary>
        /// 伤害造成后的Effect->可以造成自身增减血量等效果
        /// </summary>
        public List<IEffect> DamagedEffect { get; private set; }

        /// <summary>
        /// 招式效果结束后的效果；可以用于一些附加效果，比如寄生种子
        /// </summary>
        public List<IEffect> MovedEffect { get; private set; }



        #endregion
        
        public Func<Damage, string> OnHit;

        public Action OnThisTurnEnd;

        public Action OnTurnBegin;

        public Action<CombatPokemon, CombatPokemon> OnReplacePokemon;

        public Action<BattleResults> OnBattleEnd;

        public Action<CombatPokemon> OnPokemonFainting;
        
        /// <summary>
        /// 比如说宝可梦这回合跳过，就会调用这个方法，参数是宝可梦的Combat ID
        /// </summary>
        public Action<int> OnPokemonChooseHandled;
        public Action<Instruction> OnUserChooseInstruction;

        public Dictionary<int, Trainer> Trainers;

        public Trainer UserTrainer { get; private set; }
        public List<Trainer> alliesTrainers { get; private set; }
        public List<Trainer> opponentTrainers { get; private set; }

        public List<CombatPokemon> Pokemons
        {
            get => alliesPokemons.Union(opponentsPokemons).ToList();
        }

        public List<CombatPokemon> MyPokemons
        {
            get => (from myPokes in Pokemons where myPokes.TrainerID == UserTrainer.id select myPokes).ToList();
        }

        private BattleResults mBattleResults;

        private BattleActions mBattleActions;

        private int turnCount;


        /// <summary>
        /// For internet connection. When in local, isHost is true.
        /// In the Internet, if you are the host, you have to transmit your data to the others.
        /// If you are not the host, you have to wait for the data transmit back to you;
        /// </summary>
        public bool isHost;


        public Battle(
            bool isHost = true
        )
        {
            this.isHost = isHost;
        }

        public void StartBattle(List<Pokemon> alliesPokemons,
            List<Pokemon> opponentsPokemons,
            List<Trainer> alliesTrainers,
            List<Trainer> opponentTrainers,
            IEffect[] choosingEffect = null,
            IEffect[] movingEffect = null,
            IEffect[] damagingEffect = null,
            IEffect[] damagedEffect = null,
            IEffect[] movedEffect = null)
        {
            Instance = this;

            #region init pokemons and trainers

            this.alliesPokemons = (from poke in alliesPokemons where poke!=null select new CombatPokemon(poke, this)).ToList();
            this.opponentsPokemons = (from poke in opponentsPokemons where poke!=null select new CombatPokemon(poke, this)).ToList();
            this.UserTrainer = Game.trainer;
            this.alliesTrainers = alliesTrainers;
            this.opponentTrainers = opponentTrainers;

            Trainers = new Dictionary<int, Trainer>();
            foreach (var trainer in alliesTrainers)
            {
                Trainers.Add(trainer.id,trainer);
                var pokes = (from a in Pokemons where a.TrainerID == trainer.id select a).ToArray();
                for (int i = 0; i < pokes.Length; i++)
                {
                    trainer.pokemonOnTheBattle[trainer.PokemonIndex(pokes[i].pokemon)] = true;
                }
            }
            foreach (var trainer in opponentTrainers)
            {
                Trainers.Add(trainer.id,trainer);
                var pokes = (from a in Pokemons where a.TrainerID == trainer.id select a).ToArray();
                for (int i = 0; i < pokes.Length; i++)
                {
                    trainer.pokemonOnTheBattle[trainer.PokemonIndex(pokes[i].pokemon)] = true;
                }
            }
            
            #endregion

            #region init effects

            this.ChoosingEffect = new List<IEffect>();
            if (choosingEffect != null)
                this.ChoosingEffect.AddRange(choosingEffect);

            this.MovingEffect = new List<IEffect>();
            if (movingEffect != null)
                this.MovingEffect.AddRange(movingEffect);

            this.DamagingEffect = new List<IEffect>();
            if (damagingEffect != null)
                this.DamagingEffect.AddRange(damagingEffect);

            this.DamagedEffect = new List<IEffect>();
            if (damagedEffect != null)
                this.DamagedEffect.AddRange(damagedEffect);

            this.MovedEffect = new List<IEffect>();
            if (movedEffect != null)
                this.MovedEffect.AddRange(movedEffect);

            #endregion

            Instructions = new List<Instruction>();
            CombatMoves = new List<CombatMove>();
            SwitchPokemons = new List<Tuple<CombatPokemon, Pokemon>>();
            Damages = new List<Damage>();
            turnCount = 0;
            mBattleResults = BattleResults.Continue;
            mBattleActions = BattleActions.Choosing;

            OnTurnBegin += () => { turnCount++; };

            OnBattleEnd += (o) => { Instance = null; };
            OnTurnBegin?.Invoke();
        }

        void NextAction()
        {
            if (mBattleResults != BattleResults.Continue) return;
            switch (mBattleActions)
            {
                case BattleActions.Choosing:
                    mBattleActions = BattleActions.Moving;
                    SortCombatMoves(CombatMoves);
                    ReplacePokemons(SwitchPokemons);
                    SwitchPokemons.Clear();
                    Moving(CombatMoves);
                    CombatMoves.Clear();
                    NextAction();
                    break;
                case BattleActions.Moving:
                    mBattleActions = BattleActions.Damaging;
                    Damaging(Damages);
                    Damages.Clear();
                    NextAction();
                    break;
                case BattleActions.Damaging:
                    mBattleActions = BattleActions.Damaged;
                    Damaged();
                    NextAction();
                    break;
                case BattleActions.Damaged:
                    mBattleActions = BattleActions.Moved;
                    Moved();
                    NextAction();
                    break;
                case BattleActions.Moved:
                    mBattleActions = BattleActions.Choosing;
                    OnTurnBegin?.Invoke();
                    Choosing();
                    break;
            }
        }


        #region MoveAction

        public List<Instruction> Instructions;
        public List<CombatMove> CombatMoves;
        public List<Tuple<CombatPokemon, Pokemon>> SwitchPokemons;
        public List<Damage> Damages;

        /// <summary>
        /// 选择招式前生效的Effect
        /// </summary>
        void Choosing()
        {
            List<Instruction> inss = (from instruction in (from pokemon in Pokemons select pokemon.OnChoosing())
                where instruction != null
                select instruction).ToList();
            foreach (var ins in inss.OrEmptyIfNull())
            {
                OnPokemonChooseHandled?.Invoke(ins.CombatPokemonID);
                ReceiveInstruction(ins,true);
            }
        }

        /// <summary>
        /// 招式生效之前的Effect->可以修改招式的Type,威力,Target等
        /// </summary>
        /// <param name="cm"></param>
        /// <returns>因为可能命中多个目标，所以返回时一个列表</returns>
        List<Damage> Moving(List<CombatMove> cm)
        {
            foreach (var c in cm.OrEmptyIfNull())
            {
                Damages.AddRange(GenerateDamages(c));
            }

            return Damages;
        }

        void Damaging(List<Damage> dmgs)
        {
            foreach (var d in dmgs.OrEmptyIfNull())
            {
                Damaging(d);
            }
        }

        void Damaging(Damage dmg)
        {
            if (dmg.sponsor.HP <= 0) return;
            dmg = dmg.sponsor.OnHit(dmg);
            OnHit?.Invoke(dmg);
            // if(dmg.target.HP>0)
            dmg.target.BeHurt(dmg);
            // else
            // {
            //     //TODO：一般来说只有2v2以上的对战才需要重新找个target
            //     dmg.target = RefindTarget(dmg);
            //     dmg.target.BeHurt(dmg);
            // }
        }

        void Damaged()
        {
            // return cm;
        }

        void Moved()
        {
            if (opponentTrainers.TrainerAllFaint())
            {
                mBattleResults = BattleResults.Succeed;
                OnBattleEnd?.Invoke(BattleResults.Succeed);
            }
            else if(alliesTrainers.TrainerAllFaint())
            {
                mBattleResults = BattleResults.Failed;
                OnBattleEnd?.Invoke(BattleResults.Failed);
            }
            else
            {
                OnThisTurnEnd?.Invoke();
            }
        }

        #endregion

        public void ReplacePokemons(List<Tuple<CombatPokemon,Pokemon>> list)
        {
            foreach (var tuple in list.OrEmptyIfNull())
            {
                ReplacePokemon(tuple.Item1,tuple.Item2);
            }
        }

        public void PokemonFainting(CombatPokemon combatPokemon)
        {
            OnPokemonFainting?.Invoke(combatPokemon);
        }
        
        public void ReplacePokemon(CombatPokemon currentPokemon,Pokemon nextPokemon)
        {
            CombatPokemon nPoke = new CombatPokemon(nextPokemon, this);
            
            Trainer t = Trainers[currentPokemon.TrainerID];
            t.pokemonOnTheBattle[t.PokemonIndex(currentPokemon.pokemon)] = false;
            t.pokemonOnTheBattle[t.PokemonIndex(nextPokemon)] = true;
            
            if (alliesPokemons.Contains(currentPokemon))
            {
                alliesPokemons.Remove(currentPokemon);
                alliesPokemons.Add(nPoke);
            }
            else if (opponentsPokemons.Contains(currentPokemon))
            {
                opponentsPokemons.Remove(currentPokemon);
                opponentsPokemons.Add(nPoke);
            }
            OnReplacePokemon?.Invoke(currentPokemon,nPoke);

        }

        public bool ReceiveInstruction(Instruction ins,bool fromUser=false)
        {
            //判断有没有重复输入的
            if ((from instru in Instructions where ins.CombatPokemonID == instru.CombatPokemonID select instru)
                .Count() != 0) return false;

            if (fromUser)
            {
                OnUserChooseInstruction?.Invoke(ins);
            }
            else
            {
                //去除非用户的myPokemon的输入
                if ((from p in MyPokemons where p.CombatID == ins.CombatPokemonID select p).Count() != 0)
                {
                    return false;
                }
            }
            
            UnityEngine.Debug.Log($"Received instruction:{ins}");
            Instructions.Add(ins);

            switch (ins.command)
            {
                case Command.Move:
                    CombatMoves.Add(GenerateCombatMove(ins));
                    break;
                case Command.Items:
                    break;
                case Command.GoPokemon:
                    break;
                case Command.SwitchPokemon:
                    var combatPoke = (from p in Pokemons where p.CombatID == ins.CombatPokemonID select p).ToArray()[0];
                    SwitchPokemons.Add(new Tuple<CombatPokemon, Pokemon>(combatPoke,Trainers[combatPoke.TrainerID].party[ins.ID]));
                    break;
                case Command.Run:
                    break;
            }

            if (Instructions.Count == Pokemons.Count)
            {
                Instructions.Clear();
                NextAction();
            }

            return true;
        }

        #region 工具方法

        public CombatPokemon RefindTarget(Damage dmg)
        {
            // bool isAlly = alliesPokemons.Contains(dmg.sponsor);
            bool toAlly = alliesPokemons.Contains(dmg.target);
            if (toAlly)
            {
                foreach (var pokemon in alliesPokemons)
                {
                    if (pokemon.HP > 0) return pokemon;
                }
            }
            else
            {
                foreach (var pokemon in opponentsPokemons)
                {
                    if (pokemon.HP > 0) return pokemon;
                }
            }

            return null;
        }

        public CombatMove GenerateCombatMove(Instruction ins)
        {
            //Only can one sponsor exists
            CombatPokemon sponsor =
                (from poke in Pokemons where poke.CombatID == ins.CombatPokemonID select poke).ToArray()[0];
            List<CombatPokemon> target =
                (from poke in Pokemons
                    join combatPokemonID in ins.target on poke.CombatID equals combatPokemonID
                    select poke).ToList();
            CombatMove move = new CombatMove(sponsor.pokemon.moves[ins.ID], this, sponsor, target);
            return move;
        }

        public void SortCombatMoves(List<CombatMove> cms)
        {
            if (cms != null && cms.Count != 0)
            {
                cms.Sort((o1, o2) =>
                    {
                        if (o1.Priority != o2.Priority) return o1.Priority - o2.Priority;
                        else return o1.Sponsor.SPE - o2.Sponsor.SPE;
                    }
                );
            }
        }

        //TODO:目前的判定可能造成一个技能打宝可梦两遍甚至更多的情况，目前暂不修复
        public List<Damage> GenerateDamages(CombatMove cmove)
        {
            List<Damage> dmgs = new List<Damage>();
            foreach (var target in cmove.Targets)
            {
                var t = target;
                if(!Pokemons.Contains(target))
                {
                    
                    bool isAlly = alliesTrainers.Contains(Trainers[target.TrainerID]);
                    if (isAlly)
                    {
                        t = alliesPokemons.RandomPickOne();
                    }
                    else
                    {
                        t = opponentsPokemons.RandomPickOne();
                    }
                }
                dmgs.Add(new Damage(cmove, cmove.Sponsor, t));
            }

            return dmgs;
        }

        #endregion


        #region Debug

        public string GetBattleInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("The Pokemon In the Field:");
            sb.AppendLine("-----------Allies-----------");
            foreach (var p in alliesPokemons)
            {
                sb.AppendLine(p.ToString());
            }

            sb.AppendLine("-----------Opponents-----------");
            foreach (var p in opponentsPokemons)
            {
                sb.AppendLine(p.ToString());
            }

            return sb.ToString();
        }

        #endregion
    }
}