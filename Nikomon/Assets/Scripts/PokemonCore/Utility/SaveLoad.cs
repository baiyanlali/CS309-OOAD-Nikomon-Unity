using System.IO;
using Newtonsoft.Json;

namespace PokemonCore.Utility
{
    public static class SaveLoad
    {
        public static readonly string path = "Assets//Data//";
        public static T Load<T>(string fileName)
        {
            if (!fileName.Contains(".")) fileName += ".json";
            string filePath = path + fileName;
            
            
            if (!File.Exists(filePath)) return default(T);
            StreamReader sr = File.OpenText(filePath);
            string data = sr.ReadToEnd();
            T obj = JsonConvert.DeserializeObject<T>(data);
            return obj;
        }

        public static bool Save<T>(string fileName,T data)
        {
            if (!fileName.Contains(".")) fileName += ".json";
            string filePath = path + fileName;
            FileStream fs;
            if (File.Exists(filePath))
            {
                fs = File.Create(filePath);
            }
            else
            {
                fs = File.OpenWrite(filePath);
            }

            string jsonData = JsonConvert.SerializeObject(data);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(jsonData);
            sw.Flush();
            sw.Close();
            fs.Close();
            UnityEngine.Debug.Log($"{filePath} has saved!");
            return true;
        }
    }
}