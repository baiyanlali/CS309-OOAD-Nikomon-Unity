using Newtonsoft.Json;
using PokemonCore.Saving;
using UnityEngine;

namespace GamePlay
{
    public class SaveData
    {
        public struct SeriVec3
        {
            public float x, y, z;

            public SeriVec3(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public SeriVec3(Vector3 vec)
            {
                this.x = vec.x;
                this.y = vec.y;
                this.z = vec.z;
            }

            public Vector3 ToVec3()
            {
                return new Vector3(x, y, z);
            }
            
        }
        
        public GameState GameState;
        public string Location;
        
        public SeriVec3 PlayerPosition;
        public int SceneLoaded;

        public ConfigSettings Settings;

        [JsonConstructor]
        public SaveData(GameState gameState, string location, Vector3 playerPosition, int sceneLoaded,ConfigSettings settings)
        {
            this.GameState = gameState;
            this.Location = location;
            this.PlayerPosition = new SeriVec3(playerPosition);
            this.SceneLoaded = sceneLoaded;
            this.Settings = settings;
        }
        
    }
}