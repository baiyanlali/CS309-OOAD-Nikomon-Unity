using System;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Monster.Data;
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
            game = new Game();
            Debug.Log("-------Game Start----------");

            Trainer t1 = new Trainer("Yes!", 10086);
            Trainer t2 = new Trainer("Nope!", 10);
            Game.trainer = t1;

            Pokemon p1 = new Pokemon(Game.PokemonsData[1], "Yahaha", 50, 0);
            Pokemon p2 = new Pokemon(Game.PokemonsData[4], "", 30, 0);

            battle = new Battle(true);
            battle.StartBattle(new List<Pokemon>(){p1},new List<Pokemon>(){p2},
                new List<Trainer>(){t1},new List<Trainer>(){t2});
            Debug.Log(battle.GetBattleInfo());
            battle.ReceiveInstruction(new Instruction(battle.alliesPokemons[0].CombatID,Command.Move,0,
                new List<int>(){battle.opponentsPokemons[0].CombatID}));
            
            battle.ReceiveInstruction(new Instruction(battle.opponentsPokemons[0].CombatID,Command.Move,0,
                new List<int>(){battle.alliesPokemons[0].CombatID}));
                
            Debug.Log(battle.GetBattleInfo());
            
            
        }

        public void OnReceiveInstruction(string str)
        {
            int id = Convert.ToInt32(str);

            // PokemonData pd;
            // if (Game.PokemonsData.TryGetValue(id, out pd))
            // {
            //     Debug.Log(pd);
            // }
            // else
            // {
            //     Debug.Log("No such pokemon");
            // }

            // MoveData md;
            // if (Game.MovesData.TryGetValue(id,out md))
            // {
            //     Debug.Log(md);
            // }
            // else
            // {
            //     Debug.Log("No Such Move");
            // }
            
            // PokemonCore.Types t;
            // if (Game.TypesMap.TryGetValue(id,out t))
            // {
            //     Debug.Log(t);
            // }
        }
        
        
    }
}