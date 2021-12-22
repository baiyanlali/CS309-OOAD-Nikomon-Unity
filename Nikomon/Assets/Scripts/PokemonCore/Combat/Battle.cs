using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PokemonCore.Combat.Interface;
using PokemonCore.Utility;
using Newtonsoft.Json;
using PokemonCore.Inventory;
using UnityEngine;
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
        Run,
        Skip
    }

    [Serializable]
    public class Instruction
    {
        public int CombatPokemonID;
        public Command command;

        /// <summary>
        /// 这里的ID指的是不同Command下的ID，比如对于Move这里的id就是0，1，2，3代表宝可梦不同招式；对于Items就是Items本身的**Tag**，对于run就是本run宝可梦的trainer的ID
        /// </summary>
        public int ID;

        /// <summary>
        /// 这里的target对于Item属性稍微扩展一下，对于Item，Target的第一个元素代表Item的ID
        /// </summary>
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
        public List<Effect> ChoosingEffect { get; private set; }

        /// <summary>
        /// 招式生效之前的Effect->可以修改招式的Type,威力,Target等
        /// </summary>
        public List<Effect> MovingEffect { get; private set; }

        /// <summary>
        /// 伤害造成前的Effect->可以阻止或削减伤害的程度等
        /// </summary>
        public List<Effect> DamagingEffect { get; private set; }

        /// <summary>
        /// 伤害造成后的Effect->可以造成自身增减血量等效果
        /// </summary>
        public List<Effect> DamagedEffect { get; private set; }

        /// <summary>
        /// 招式效果结束后的效果；可以用于一些附加效果，比如寄生种子
        /// </summary>
        public List<(Effect,CombatPokemon)> MovedEffect { get; private set; }
        
        /// <summary>
        /// 当前场地内的EffectList
        /// </summary>
        public List<Effect> FieldEffect { get; private set; }

        #endregion

        public Action<Damage> OnHit;

        public Action<CombatPokemon> OnHitted;

        public Action<CombatMove> OnMove;

        public Action OnThisTurnEnd;

        public Action OnTurnBegin;

        public Action<CombatPokemon, CombatPokemon> OnReplacePokemon;

        public Action<BattleResults> OnBattleEnd;

        public Action<CombatPokemon> OnPokemonFainting;


        public Action<Instruction> OnUserChooseInstruction;

        public Action<bool> OnCatchPokemon;

        /// <summary>
        /// 目前应当显示那只宝可梦的move是由battle决定，并交给Show Poke Move来响应
        /// </summary>
        public Action<CombatPokemon> ShowPokeMove;

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

        /// <summary>
        /// 假设只有1v1的情况下才能run
        /// </summary>
        public bool CanRun => alliesTrainers.Count == 1 && opponentTrainers.Count == 1;

        private BattleResults mBattleResults;

        private BattleActions mBattleActions;

        private int turnCount;


        /// <summary>
        /// For internet connection. When in local, isHost is true.
        /// In the Internet, if you are the host, you have to transmit your data to the others.
        /// If you are not the host, you have to wait for the data transmit back to you;
        /// </summary>
        public bool isHost;

        /// <summary>
        /// 这个是在回合开始时判断是否有濒死的宝可梦，如果有的话就先调用方法让Trainer去换宝可梦
        /// </summary>
        public List<CombatPokemon> faintPokemon => (from poke in Pokemons where poke.HP <= 0 select poke).ToList();


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
            Effect[] choosingEffect = null,
            Effect[] movingEffect = null,
            Effect[] damagingEffect = null,
            Effect[] damagedEffect = null,
            (Effect,CombatPokemon)[] movedEffect = null)
        {
            Instance = this;

            #region init pokemons and trainers

            this.alliesPokemons = (from poke in alliesPokemons where poke != null select new CombatPokemon(poke))
                .ToList();
            this.opponentsPokemons =
                (from poke in opponentsPokemons where poke != null select new CombatPokemon(poke)).ToList();
            this.UserTrainer = Game.trainer;
            this.alliesTrainers = alliesTrainers;
            this.opponentTrainers = opponentTrainers;

            Trainers = new Dictionary<int, Trainer>();
            foreach (var trainer in alliesTrainers)
            {
                Trainers.Add(trainer.id, trainer);
                var pokes = (from a in Pokemons where a.TrainerID == trainer.id select a).ToArray();
                for (int i = 0; i < pokes.Length; i++)
                {
                    trainer.pokemonOnTheBattle[trainer.PokemonIndex(pokes[i].pokemon)] = true;
                }
            }

            foreach (var trainer in opponentTrainers)
            {
                Trainers.Add(trainer.id, trainer);
                var pokes = (from a in Pokemons where a.TrainerID == trainer.id select a).ToArray();
                for (int i = 0; i < pokes.Length; i++)
                {
                    trainer.pokemonOnTheBattle[trainer.PokemonIndex(pokes[i].pokemon)] = true;
                }
            }

            #endregion

            #region init effects

            this.ChoosingEffect = new List<Effect>();
            if (choosingEffect != null)
                this.ChoosingEffect.AddRange(choosingEffect);

            this.MovingEffect = new List<Effect>();
            if (movingEffect != null)
                this.MovingEffect.AddRange(movingEffect);

            this.DamagingEffect = new List<Effect>();
            if (damagingEffect != null)
                this.DamagingEffect.AddRange(damagingEffect);

            this.DamagedEffect = new List<Effect>();
            if (damagedEffect != null)
                this.DamagedEffect.AddRange(damagedEffect);

            this.MovedEffect = new List<(Effect,CombatPokemon)>();
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

            OnBattleEnd += (o) =>
            {
                for (int i = 0; i < Game.trainer.pokemonOnTheBattle.Length; i++)
                {
                    Game.trainer.pokemonOnTheBattle[i] = false;
                }

                Instance = null;
            };

        }

        /// <summary>
        /// 在start battle时很多外界状态都没初始化完成,这里用来将外界初始化完成的内容进行初始化
        /// </summary>
        public void CompleteInit()
        {
            OnTurnBegin?.Invoke();
            // UnityEngine.Debug.Log("Complete Battle Log");
            if(MyPokeWithNoInstructions.Count!=0)
                ShowPokeMove?.Invoke(MyPokeWithNoInstructions[0]);
        }
        
        #region new move methods
        public void NextMove()
        {
            if (mBattleResults != BattleResults.Continue) return;
            switch (mBattleActions)
            {
                case BattleActions.Choosing:
                    mBattleActions = BattleActions.Moving;
                    SortCombatMoves(CombatMoves);
                    ReplacePokemons(SwitchPokemons);
                    SwitchPokemons.Clear();
                    // CombatMoves.Clear();
                    NextMove();
                    break;
                case BattleActions.Moving:
                    if (CombatMoves.Count == 0)
                    {
                        //本回合结束
                        mBattleActions = BattleActions.Moved;
                        OnThisTurnEnd?.Invoke();
                        foreach ((Effect,CombatPokemon) e in MovedEffect.OrEmptyIfNull())
                        {
                            e.Item1.OnMoved?.Invoke(e.Item2);
                        }
                        Moved();
                        NextMove();
                        return;
                    }
                    else
                    {
                        //这里由外部调用，进行下一个招式的播放
                        mBattleActions = BattleActions.Moving;
                    }
                    SingleMoving(CombatMoves);
                    Damaging(Damages);
                    Damages.Clear();
                    
                    Moved();
                    if (mBattleResults != BattleResults.Continue) return;
                    break;
                case BattleActions.Moved:

                    mBattleActions = BattleActions.Choosing;
                    OnTurnBegin?.Invoke();
                    
                    Choosing();
                    if(MyPokeWithNoInstructions.Count!=0)
                        ShowPokeMove?.Invoke(MyPokeWithNoInstructions[0]);
                    break;
            }
        }
        
        
        void SingleMoving(List<CombatMove> cm)
        {
            if (cm.Count == 0) return;
            var c = cm[0];
            cm.RemoveAt(0);
            
            c = c.Sponsor.OnMoving(c);
            OnMove?.Invoke(c);
            Damages.AddRange(GenerateDamages(c));
        }


        #endregion

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
            foreach (var e in ChoosingEffect.OrEmptyIfNull())
            {
                foreach (var p in Pokemons)
                {
                    if (e.OnChoosing == null) continue;
                    Instruction i = e.OnChoosing(p);
                    if (i != null) ReceiveInstruction(i,true);
                }
            }
            foreach (var ins in inss.OrEmptyIfNull())
            {
                // OnPokemonChooseHandled?.Invoke(ins.CombatPokemonID);
                ReceiveInstruction(ins, true);
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
                c.Sponsor.lastMove = c.move;
                var c_final = c.Sponsor.OnMoving(c);
                OnMove?.Invoke(c_final);
                Damages.AddRange(GenerateDamages(c_final));
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

            OnHitted?.Invoke(dmg.target);
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

        /// <summary>
        /// 用来判断此turn结束后是否分出胜负
        /// </summary>
        void Moved()
        {
            if (opponentTrainers.TrainerAllFaint())
            {
                mBattleResults = BattleResults.Succeed;
                OnBattleEnd?.Invoke(BattleResults.Succeed);
            }
            else if (alliesTrainers.TrainerAllFaint())
            {
                mBattleResults = BattleResults.Failed;
                OnBattleEnd?.Invoke(BattleResults.Failed);
            }
            else
            {
                // OnThisTurnEnd?.Invoke();
            }
        }

        #endregion

        public void ReplacePokemons(List<Tuple<CombatPokemon, Pokemon>> list)
        {
            foreach (var tuple in list.OrEmptyIfNull())
            {
                ReplacePokemon(tuple.Item1, tuple.Item2);
            }
        }

        public void PokemonFainting(CombatPokemon combatPokemon)
        {
            OnPokemonFainting?.Invoke(combatPokemon);
        }

        /// <summary>
        /// 捕捉宝可梦！
        /// </summary>
        /// <param name="pokemons"></param>
        public void CatchPokemon(CombatPokemon currentPoke, int pokeBall, List<int> pokemons)
        {
            //众所周知，被捕捉的宝可梦一定是opponent
            // var pokes = (from combatPokes in opponentsPokemons
            //     join pokemon in pokemons on combatPokes.CombatID equals pokemon
            //     select combatPokes).ToList();
            List<CombatPokemon> pokes = new List<CombatPokemon>();
            foreach (var pokemon in pokemons)
            {
                pokes.Add(GetCombatPokemon(pokemon));
            }

            foreach (var p in pokes.OrEmptyIfNull())
            {
                bool result = Calculator.Catch(pokeBall, currentPoke.pokemon, p.pokemon);
                OnCatchPokemon?.Invoke(result);
                if (result)
                {
                    Game.Instance.AddPokemon(p.pokemon);
                }

                Trainers[p.TrainerID].CatchPokemon(p.pokemon);
                opponentsPokemons.Remove(p);
            }
        }

        public void ReplacePokemon(CombatPokemon currentPokemon, Pokemon nextPokemon)
        {
            CombatPokemon nPoke = new CombatPokemon(nextPokemon);

            Trainer t = Trainers[currentPokemon.TrainerID];
            t.pokemonOnTheBattle[t.PokemonIndex(currentPokemon.pokemon)] = false;
            t.pokemonOnTheBattle[t.PokemonIndex(nextPokemon)] = true;

            //TODO;
            int index = alliesPokemons.IndexOf(currentPokemon);
            // alliesPokemons.BinarySearch(currentPokemon);
            if (alliesPokemons.Contains(currentPokemon))
            {
                alliesPokemons.Remove(currentPokemon);
                alliesPokemons.Insert(index, nPoke);
            }
            else if (opponentsPokemons.Contains(currentPokemon))
            {
                opponentsPokemons.Remove(currentPokemon);
                opponentsPokemons.Insert(index, nPoke);
            }

            OnReplacePokemon?.Invoke(currentPokemon, nPoke);
        }

        public List<CombatPokemon> MyPokeWithInstructions =>
            (from poke in MyPokemons
                join instruction in Instructions on poke.CombatID equals instruction.CombatPokemonID
                select poke).ToList();

        public List<CombatPokemon> MyPokeWithNoInstructions => MyPokemons.Except(MyPokeWithInstructions).ToList();

        public bool ReceiveInstruction(Instruction ins, bool fromUser = false)
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


            Instructions.Add(ins);
            UnityEngine.Debug.Log($"Received instruction:{ins}, now has {Instructions.Count} instruction(s)");


            switch (ins.command)
            {
                case Command.Move:
                    CombatMoves.Add(GenerateCombatMove(ins));
                    break;
                case Command.Items:
                    Item.Tag tag = (Item.Tag) ins.ID;
                    int ID = ins.target[0];
                    ins.target.RemoveAt(0);

                    switch (tag)
                    {
                        case Item.Tag.PokeBalls:
                            CatchPokemon(GetCombatPokemon(ins.CombatPokemonID), ID, ins.target);
                            if (opponentTrainers.TrainerAllFaint())
                            {
                                mBattleResults = BattleResults.Captured;
                                OnBattleEnd?.Invoke(BattleResults.Captured);
                            }

                            break;
                        case Item.Tag.Berries:
                        case Item.Tag.Medicine:
                            break;
                    }

                    break;
                case Command.SwitchPokemon:
                    var combatPoke = GetCombatPokemon(ins.CombatPokemonID);
                    if (combatPoke.HP <= 0)
                    {
                        Instructions.Remove(ins);
                        ReplacePokemon(combatPoke,Trainers[combatPoke.TrainerID].party[ins.ID]);
                    }
                    else
                    {
                        SwitchPokemons.Add(new Tuple<CombatPokemon, Pokemon>(combatPoke,
                            Trainers[combatPoke.TrainerID].party[ins.ID]));
                    }
                    break;
                case Command.Run:
                    if (CanRun)
                    {
                        //只有在1（trainer）v1（trainer）的时候才能run！
                        if (alliesTrainers.Contains(Trainers[ins.ID]))
                        {
                            mBattleResults = BattleResults.Ran;
                            OnBattleEnd?.Invoke(BattleResults.Ran);
                        }
                        else
                        {
                            mBattleResults = BattleResults.Succeed;
                            OnBattleEnd?.Invoke(BattleResults.Succeed);
                        }
                    }
                    else
                    {
                    }

                    break;
                case Command.Skip:
                    break;
            }

            
            
            if (Instructions.Count == Pokemons.Count)
            {
                //如果所有指令都接收到则进入move
                Instructions.Clear();
                // UnityEngine.Debug.Log($"{Instructions.Count} instruction and {Pokemons.Count} pokemon, i can go next move");
                // NextAction();
                NextMove();
            }else if (MyPokeWithNoInstructions.Count != 0 && fromUser)
            {
                ShowPokeMove?.Invoke(MyPokeWithNoInstructions[0]);
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

        public CombatMove GenerateCombatMoveFromItem()
        {
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
                        //从大到小排列
                        if (o1.Priority != o2.Priority) return o2.Priority - o1.Priority;
                        else return o2.Sponsor.SPE - o1.Sponsor.SPE;
                    }
                );
            }
        }

        //TODO:目前的判定可能造成一个技能打宝可梦两遍甚至更多的情况，目前暂不修复
        public List<Damage> GenerateDamages(CombatMove cmove)
        {
            List<Damage> dmgs = new List<Damage>();
            foreach (var target in cmove.Targets.OrEmptyIfNull())
            {
                var t = target;
                if (!Pokemons.Contains(target))
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

        public CombatPokemon GetCombatPokemon(int combatID)
        {
            var pokes = (from poke in Pokemons where poke.CombatID == combatID select poke).ToArray();
            if (pokes.Length != 1) throw new Exception("你Combat ID 没写好，出问题了吧");
            return pokes[0];
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

        public void AddMoveEffect(string e, CombatPokemon poke)
        {
            MovedEffect.Add((Game.LuaEnv.Global.Get<Effect>(e),poke));
        }

        #endregion
    }
}