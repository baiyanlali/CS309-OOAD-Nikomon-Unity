using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Monster.Data;
using PokemonCore.Network;
using PokemonCore.Utility;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using Types = PokemonCore.Types;

namespace ConsoleDebug
{
    public class ConsoleTestMono : MonoBehaviour
    {
        private Game game;
        private Battle battle;

        public bool enable;
        private void Start()
        {
            if(enable)
                InitConsole();
            // StartNetworkTest();
        }

        public void InitConsole()
        {
            ConsoleDebug.Console.Init();

            ConsoleDebug.Console.OnMessageEntered += OnReceiveInstruction;

            // Application.logMessageReceived += DebugUIHandler.Instance.InsertInfo;
                    
            Application.quitting += () =>
            {
                ConsoleDebug.Console.OnDestroy();
            };
        }
        


        // public void StartNetworkTest()
        // {
        //     Debug.Log("Start to use network");
        //     
        //     NetworkLogic.StartLocalNetwork();
        //
        //     StartCoroutine(Network());
        //
        //     Application.quitting += () =>
        //     {
        //         NetworkLogic.CloseLocalNetwork();
        //     };
        // }

        IEnumerator Network()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                Debug.Log(NetworkLogic.usersBroadcast.ConvertToString());
                
            }
        }
        

        public IEnumerator StartBattle()
        {
            orders = new List<string>();

            game = new Game();
            Debug.Log("-------Game Start----------");


            Trainer t1 = new Trainer("Yes!",true);
            Trainer t2 = new Trainer("Nope!",false);
            Game.trainer = t1;

            var keys= Game.PokemonsData.Keys;
            foreach (var v in keys.OrEmptyIfNull())
            {
                Debug.Log($"No.{v} {Game.PokemonsData[v].innerName}");
            }

            Debug.Log("Please Choose Your Pokemon:");
            yield return new WaitUntil(()=>orders.Count != 0);
            int Poke = int.Parse(orders[0]);
            orders.Remove(Poke.ToString());

            Pokemon p1 = new Pokemon(Game.PokemonsData[Poke], "",t1, 50, 0);
            
            Debug.Log("Please Choose Your Pokemon:");
            yield return new WaitUntil(()=>orders.Count != 0);
            Poke = int.Parse(orders[0]);
            orders.Remove(Poke.ToString());
            Pokemon p11 = new Pokemon(Game.PokemonsData[Poke], "",t1, 50, 0);

            Pokemon p2 = new Pokemon(Game.PokemonsData[4], "",t2, 50, 0);
            Pokemon p22 = new Pokemon(Game.PokemonsData[17], "",t2, 50, 0);

            battle = new Battle(true);
            BattleReporter battleReporter = new BattleReporter(battle);
            battle.StartBattle(new List<Pokemon>(){p1,p11},new List<Pokemon>(){p2,p22},
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

                    Debug.Log("Choose for a target:");
                    foreach (var po in battle.opponentsPokemons)
                    {
                        Debug.Log($"Target ${po.CombatID}, {po.Name}");
                    }

                    yield return new WaitUntil(()=>orders.Count != 0);
                    int target = int.Parse(orders[0]);
                    orders.Clear();

                    battle.ReceiveInstruction(new Instruction(p.CombatID,Command.Move,move,
                                                                       new List<int>(){target}));
                }

                foreach (var otherPoke in battle.Pokemons.Except(myPoke))
                {
                    battle.ReceiveInstruction(new Instruction(otherPoke.CombatID,Command.Move,0,
                                        new List<int>(){battle.alliesPokemons[Random.Range(0,battle.alliesPokemons.Count)].CombatID}));
                }

                
            }

            
        }

        private List<string> orders;
        public void OnReceiveInstruction(string str)
        {
            orders.Add(str);
        }
        
        
    }
}