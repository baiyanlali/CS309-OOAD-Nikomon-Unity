using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonCore.Combat.Interface;

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
        Run
    }

    [Serializable]
    public class Instruction
    {
        public int TrainerID;
        public Command command;
        public int ID;

        Instruction(int trainerID,Command command, int id)
        {
            this.TrainerID = trainerID;
            this.command = command;
            this.ID = id;
        }
    }

    /// <summary>
    /// Battle类，集中处理所有战斗逻辑
    /// </summary>
    public class Battle
    {
        public List<Pokemon> alliesPokemons { get; private set; }
        public List<Pokemon> opponentsPokemons { get; private set; }
        public List<Pokemon> Pokemons { get; private set; }

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

        public Trainer UserTrainer { get; private set; }
        public Trainer[] alliesTrainers { get; private set; }
        public Trainer[] opponentTrainers { get; private set; }

        private BattleResults mBattleResults;

        private BattleActions mBattleActions;

        private int turnCount;

        private int currentPokemonIndex;
        
        private Pokemon CurrentPokemon
        {
            get => Pokemons[currentPokemonIndex];
        }

        public Battle(
            
        )
        {
            
        }

        public void StartBattle(List<Pokemon> alliesPokemons,
            List<Pokemon> opponentsPokemons,
            Trainer[] alliesTrainers,
            Trainer[] opponentTrainers,
            IEffect[] choosingEffect = null,
            IEffect[] movingEffect = null,
            IEffect[] damagingEffect = null,
            IEffect[] damagedEffect = null,
            IEffect[] movedEffect = null)
        {
            #region init pokemons and trainers

            this.alliesPokemons = alliesPokemons;
            this.opponentsPokemons = opponentsPokemons;
            this.alliesTrainers = alliesTrainers;
            this.opponentTrainers = opponentTrainers;
            UserTrainer = Game.Instance.trainer;
            Pokemons = alliesPokemons?.Union(opponentsPokemons).ToList();
            Pokemons.Sort((p1,p2)=>p1.SPE-p2.SPE);
            currentPokemonIndex = 0;
            #endregion

            #region init effects

            this.ChoosingEffect = new List<IEffect>();
            this.ChoosingEffect.AddRange(choosingEffect);
            this.MovingEffect = new List<IEffect>();
            this.MovingEffect.AddRange(movingEffect);
            this.DamagingEffect = new List<IEffect>();
            this.DamagingEffect.AddRange(damagingEffect);
            this.DamagedEffect = new List<IEffect>();
            this.DamagedEffect.AddRange(damagedEffect);
            this.MovedEffect = new List<IEffect>();
            this.MovedEffect.AddRange(movedEffect);
            #endregion

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
                    break;
                case BattleActions.Moving:
                    mBattleActions = BattleActions.Damaging;
                    break;
                case BattleActions.Damaging:
                    mBattleActions = BattleActions.Damaged;
                    break;
                case BattleActions.Damaged:
                    mBattleActions = BattleActions.Moved;
                    break;
                case BattleActions.Moved:
                    mBattleActions = BattleActions.Choosing;
                    break;
                        
            }
        }
        
        void UpdateBattle()
        {
            if (mBattleResults == BattleResults.Continue)
            {
                if (currentPokemonIndex == Pokemons.Count)
                {
                    currentPokemonIndex = 0;
                    turnCount++;
                }
                Pokemon battler = Pokemons[currentPokemonIndex];
                currentPokemonIndex++;
                if (battler.TrainerID == this.UserTrainer.id)
                {
                    
                }
                else
                {
                    
                }
                
            }
        }

        #region MoveAction
        
        /// <summary>
        /// 选择招式前生效的Effect
        /// </summary>
        void Choosing()
        {
            
        }

        /// <summary>
        /// 招式生效之前的Effect->可以修改招式的Type,威力,Target等
        /// </summary>
        /// <param name="cm"></param>
        /// <returns>因为可能命中多个目标，所以返回时一个列表</returns>
        List<CombatMove> Moving(CombatMove cm)
        {
            return new List<CombatMove>();
        }

        CombatMove Damaging(CombatMove cm)
        {
            return cm;
        }

        CombatMove Damaged(CombatMove cm)
        {
            return cm;
        }

        CombatMove Moved(CombatMove cm)
        {
            return cm;
        }
        
        #endregion

        public bool ReceiveInstruction(Instruction ins)
        {
            if (ins.TrainerID != CurrentPokemon.TrainerID || mBattleActions!=BattleActions.Choosing)
                return false;
            switch (ins.command)
            {
                case Command.Move:
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
            return true;
        }
        
        
    }
}