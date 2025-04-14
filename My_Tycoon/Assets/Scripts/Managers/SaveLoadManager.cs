using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class SaveLoadManager : MonoBehaviour
    {
        public static SaveLoadManager Instance;

        public Dictionary<int, GameObject> placedBuildings = new Dictionary<int, GameObject>();
        [SerializeField] private List<GameObject> buildingPrefabs;
        [SerializeField] private float autosaveInterval = 30f;
        private float autosaveTimer = 0f;

        private string savePath;
        private int lastUsedId = 0;

        [SerializeField] private bool autoSave = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                savePath = Path.Combine(Application.persistentDataPath, "buildings.json");
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
                SaveGame();

            if (Input.GetKeyDown(KeyCode.F9))
                LoadGame();
            
            
            autosaveTimer += Time.deltaTime;
            if (autosaveTimer >= autosaveInterval && autoSave)
            {
                SaveGame();
                autosaveTimer = 0f;
                Debug.Log("Saved automatically");
            }
        }


        [System.Serializable]
        private class BuildingDataList
        {
            public List<BuildingData> buildings;
        }

        [System.Serializable]
        public class BuildingData
        {
            public int id;
            public string prefabName;
            public Vector3 position;
            public Quaternion rotation;
            public string tag;
        }

        public void SaveGame()
        {
            var buildingData = placedBuildings.Select(pair =>
            {
                GameObject obj = pair.Value;
                return new BuildingData
                {
                    id = pair.Key,
                    prefabName = obj.name.Replace("(Clone)", ""),
                    position = obj.transform.position,
                    rotation = obj.transform.rotation,
                    tag = obj.tag
                };
            }).ToList();

            var resourceData = EconomyManager.Instance.Resources.Select(pair => new ResourceEntry
            {
                type = pair.Key,
                amount = pair.Value
            }).ToList();

            var gameData = new GameData
            {
                buildings = buildingData,
                resources = resourceData
            };

            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(savePath, json);
        }

        public void LoadGame()
        {
            if (!File.Exists(savePath)) return;

            string json = File.ReadAllText(savePath);
            var gameData = JsonUtility.FromJson<GameData>(json);

            placedBuildings.Clear();
            lastUsedId = 0;

            foreach (var building in gameData.buildings)
            {
                GameObject prefab = buildingPrefabs.FirstOrDefault(p => p.name == building.prefabName);
                if (prefab == null)
                {
                    Debug.LogWarning($"Prefab not found: {building.prefabName}");
                    continue;
                }

                GameObject obj = Instantiate(prefab, building.position, building.rotation);
                obj.name = prefab.name;
                obj.tag = building.tag;
                obj.layer = LayerMask.NameToLayer(building.tag);

                placedBuildings[building.id] = obj;

                if (building.id > lastUsedId)
                    lastUsedId = building.id;
            }

            EconomyManager.Instance.Resources.Clear();
            foreach (var entry in gameData.resources)
            {
                EconomyManager.Instance.Resources[entry.type] = entry.amount;
            }
        }
        
        public void ClearSave()
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("Save file deleted.");
            }
            else
            {
                Debug.Log("No save file found to delete.");
            }
        }


        public int GetNextId()
        {
            lastUsedId++;
            return lastUsedId;
        }
    }
    
    [System.Serializable]
    public class ResourceEntry
    {
        public ResourceType type;
        public int amount;
    }

    [System.Serializable]
    public class GameData
    {
        public List<SaveLoadManager.BuildingData> buildings;
        public List<ResourceEntry> resources;
    }

}
