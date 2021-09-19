using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonCore.Combat.Interface;
using PokemonCore.Utility;
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
        public int ID;
        public List<int> target;

        
        public Instruction(int combatPokemonID,Command command, int id,List<int> target) 
        {
            this.CombatPokemonID = combatPokemonID;
            this.command = command;
            this.ID = id;
            this.target = target;
            
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
        
        public Func<Damage,string> OnHit;

        public Action OnThisTurnEnd;

        public Action OnTurnBegin;

        #endregion

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
            bool isHost=true
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

            this.alliesPokemons = (from poke in alliesPokemons select new CombatPokemon(poke,this)).ToList();
            this.opponentsPokemons=(from poke in opponentsPokemons select new CombatPokemon(poke,this)).ToList();
            this.UserTrainer = Game.trainer;
            this.alliesTrainers = alliesTrainers;
            this.opponentTrainers = opponentTrainers;
            #endregion

            #region init effects

            this.ChoosingEffect = new List<IEffect>();
            if(choosingEffect!=null)
                this.ChoosingEffect.AddRange(choosingEffect);
           
            this.MovingEffect = new List<IEffect>();
            if(movingEffect!=null)
                this.MovingEffect.AddRange(movingEffect);
            
            this.DamagingEffect = new List<IEffect>();
            if(damagingEffect!=null)
                this.DamagingEffect.AddRange(damagingEffect);
            
            this.DamagedEffect = new List<IEffect>();
            if(damagedEffect!=null)
                this.DamagedEffect.AddRange(damagedEffect);
            
            this.MovedEffect = new List<IEffect>();
            if(movedEffect!=null)
                this.MovedEffect.AddRange(movedEffect);
            #endregion

            Instructions = new List<Instruction>();
            CombatMoves = new List<CombatMove>();
            Damages = new List<Damage>();
            turnCount = 0;
            mBattleResults = BattleResults.Continue;
            mBattleActions = BattleActions.Choosing;
        }

        void NextAction()
        {
            switch (mBattleActions)
            {
                case BattleActions.Choosing:
                    mBattleActions = BattleActions.Moving;
                    SortCombatMoves(CombatMoves);
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
        public List<Damage> Damages;

        /// <summary>
        /// 选择招式前生效的Effect
        /// </summary>
        void Choosing()
        {
            List<Instruction> inss =(from instruction in (from pokemon in Pokemons select pokemon.OnChoosing())
                where instruction != null
                select instruction).ToList();
            foreach (var ins in inss.OrEmptyIfNull())
            {
                ReceiveInstruction(ins);
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
            dmg = dmg.sponsor.OnHit(dmg);
            OnHit?.Invoke(dmg);
            dmg.target.BeHurt(dmg);
            
        }

        void Damaged()
        {
            // return cm;
        }

        void Moved()
        {
            turnCount++;
            OnThisTurnEnd?.Invoke();
        }
        
        #endregion


        //TODO: 目前按照接受收到的Instruction数量来判断是否进入下一个阶段，不是很合理
        public bool ReceiveInstruction(Instruction ins)
        {
            
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
                    break;
                case Command.Run:
                    break;
            }

            if (Instructions.Count == Pokemons.Count)
            {
                NextAction();
                Instructions.Clear();
            }
            
            return true;
        }

        #region 工具方法

        public CombatMove GenerateCombatMove(Instruction ins)
        {
            //Only can one sponsor exists
            CombatPokemon sponsor =
                (from poke in Pokemons where poke.CombatID == ins.CombatPokemonID select poke).ToArray()[0];
            List<CombatPokemon> target =
            (from poke in Pokemons
                join combatPokemonID in ins.target on poke.CombatID equals combatPokemonID
                select poke).ToList();
            CombatMove move =new CombatMove(sponsor.pokemon.moves[ins.ID],this,sponsor,target);
            return move;
        }

        public void SortCombatMoves(List<CombatMove> cms)
        {
            if (cms != null && cms.Count != 0)
            {
                cms.Sort((o1,o2)=>
                    {
                        if (o1.Priority != o2.Priority) return o1.Priority - o2.Priority;
                        else return o1.Sponsor.SPE - o2.Sponsor.SPE;
                    }
                );
            }
        }

        
        public List<Damage> GenerateDamages(CombatMove cmove)
        {
            List<Damage> dmgs = new List<Damage>();
            foreach (var target in cmove.Targets)
            {
                dmgs.Add(new Damage(cmove,cmove.Sponsor,target));
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