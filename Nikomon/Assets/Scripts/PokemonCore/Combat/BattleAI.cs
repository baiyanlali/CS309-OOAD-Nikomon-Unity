using System.Collections.Generic;
using System.Linq;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Utility;

namespace PokemonCore.Combat
{
    public class BattleAI
    {
        /// <summary>
        /// 这里是指做AI用的trainer
        /// </summary>
        private List<Trainer> trainers;
        private Battle battle;
        private List<CombatPokemon> pokes;
        public BattleAI(Battle battle,List<Trainer> trainers)
        {
            this.battle = battle;
            this.trainers = trainers;
            pokes = (from pokes in battle.Pokemons
                join trainer in trainers on pokes.TrainerID equals trainer.id
                select pokes).ToList();

            battle.OnTurnBegin += DoAI;
            DoAI();
        }

        public void OnPokemonDied(CombatPokemon pokemon)
        {
            
        }

        public void DoAI()
        {
            foreach (var poke in pokes.OrEmptyIfNull())
            {
                UnityEngine.Debug.Log("AI Move");
                Move[] moves = (from m in poke.pokemon.moves where m != null select m).ToArray();
                int id = Game.Random.Next(0, moves.Length);
                Move move = moves[id];
                if (move.PP == 0)
                {
                    //TODO:这里把pp=0的地方写出来
                }

                int target = 0;
                if (move._baseData.Target == Targets.SELECTED_OPPONENT_POKEMON)
                {
                    if (battle.alliesPokemons.Contains(poke))
                    {
                        target = battle.opponentsPokemons[Game.Random.Next(0,battle.opponentsPokemons.Count)].CombatID;
                    }
                    else if(battle.opponentsPokemons.Contains(poke))
                    {
                        target = battle.alliesPokemons[Game.Random.Next(0,battle.alliesPokemons.Count)].CombatID;
                    }
                }
                Instruction instruction = new Instruction(poke.CombatID, Command.Move,id,target);
                battle.ReceiveInstruction(instruction);
            }

        }
    }
}