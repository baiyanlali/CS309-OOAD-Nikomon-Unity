﻿using Newtonsoft.Json;
using PokemonCore.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class SaveData
    {
        public GameState GameState;
        public string Location;
        
        public Vector3 PlayerPosition;
        public int SceneLoaded;

        [JsonConstructor]
        public SaveData(GameState gameState, string location, Vector3 playerPosition, int sceneLoaded)
        {
            this.GameState = gameState;
            this.Location = location;
            this.PlayerPosition = playerPosition;
            this.SceneLoaded = sceneLoaded;
        }
        
    }
}