using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Managers
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private static string path = Application.persistentDataPath + "/save.json";

        public void SaveGame(GameData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }

        public GameData LoadGame()
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<GameData>(json);
            }
            return new GameData();
        }
    }

    [System.Serializable]
    public class GameData
    {
        public float money;
        //public List<BuildingData> buildings;
    }
}
