using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Monster.Data;
using PokemonCore.Utility;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Types = PokemonCore.Types;

namespace ConsoleDebug
{
    public class ConsoleTestMono : MonoBehaviour
    {
        private Game game;
        private Battle battle;
        private void Awake()
        {
            ConsoleDebug.Console.Init();

            ConsoleDebug.Console.OnMessageEntered += OnReceiveInstruction;
                    
            Application.quitting += () =>
            {
                ConsoleDebug.Console.OnDestroy();
            };
        }

        private void Start()
        {
            orders = new List<string>();
            StartCoroutine(StartBattle());
            
        }


        public IEnumerator StartBattle()
        {
            game = new Game();
            Debug.Log("-------Game Start----------");


            Trainer t1 = new Trainer("Yes!", 10086);
            Trainer t2 = new Trainer("Nope!", 10);
            Game.trainer = t1;

            Debug.Log("Please Choose Your Pokemon:");
            var keys= Game.PokemonsData.Keys;
            foreach (var v in keys.OrEmptyIfNull())
            {
                Debug.Log($"No.{v} {Game.PokemonsData[v].innerName}");
            }
            
            yield return new WaitUntil(()=>orders.Count != 0);
            int Poke = int.Parse(orders[0]);
            orders.Remove(Poke.ToString());

            Pokemon p1 = new Pokemon(Game.PokemonsData[Poke], "",t1, 50, 0);
            Pokemon p2 = new Pokemon(Game.PokemonsData[4], "",t2, 30, 0);

            battle = new Battle(true);
            BattleReporter battleReporter = new BattleReporter(battle);
            battle.StartBattle(new List<Pokemon>(){p1},new List<Pokemon>(){p2},
                new List<Trainer>(){t1},new List<Trainer>(){t2});
            Debug.Log(battle.GetBattleInfo());
            while (true)
            {
                Debug.Log("Your move:");

                List<CombatPokemon> myPoke = battle.MyPokemons;
                foreach (var p in myPoke.OrEmptyIfNull())
                {
                    Debug.Log($"Choose a move for pokemon {p.Name}");
                    Move[] moves = p.pokemon.moves;
                    for (int i = 0; i < p.pokemon.moves.Length; i++)
                    {
                        if (moves[i] == null) break;
                        Debug.Log($"No.{i} {moves[i]._baseData.innerName}");
                    }
                    yield return new WaitUntil(()=>orders.Count != 0);
                    int move = int.Parse(orders[0]);
                    orders.Remove(move.ToString());
                    battle.ReceiveInstruction(new Instruction(p.CombatID,Command.Move,move,
                                                                       new List<int>(){battle.opponentsPokemons[0].CombatID}));
                }

                foreach (var otherPoke in battle.Pokemons.Except(myPoke))
                {
                    battle.ReceiveInstruction(new Instruction(battle.opponentsPokemons[0].CombatID,Command.Move,0,
                                        new List<int>(){battle.alliesPokemons[0].CombatID}));
                }

                
            }

            

            Debug.Log(battle.GetBattleInfo());

        }

        private List<string> orders;
        public void OnReceiveInstruction(string str)
        {
            orders.Add(str);
        }
        
        
    }
}