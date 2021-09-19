using System;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Monster.Data;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Types = PokemonCore.Types;

namespace ConsoleDebug
{
    public class ConsoleTestMono : MonoBehaviour
    {
        private Game game;
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
            Debug.Log("Please check");
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

            MoveData md;
            if (Game.MovesData.TryGetValue(id,out md))
            {
                Debug.Log(md);
            }
            else
            {
                Debug.Log("No Such Move");
            }

            // PokemonCore.Types t;
            // if (Game.TypesMap.TryGetValue(id,out t))
            // {
            //     Debug.Log(t);
            // }
        }
        
        
    }
}