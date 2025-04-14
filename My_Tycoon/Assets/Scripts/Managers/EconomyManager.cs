using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public enum ResourceType
    {
        Wood,
        Stone,
        Iron,
        Coal,
        Road,
        SmallHouse,
        MidHouse,
        BigHouse,
        Smithy,
        Blacksmith,
        CoalMine,
        SawMill,
        TradingPoint,
        Residents
    }

    public class EconomyManager : Singleton<EconomyManager>
    {
        public int Money { get; private set; }
        public int CurrentLevel { get; private set; } = 1;


        public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 20 },
            { ResourceType.Stone, 20 },
            { ResourceType.Iron, 20 },
            { ResourceType.Coal, 20 },
            { ResourceType.Road, 1 },
            { ResourceType.SmallHouse, 1 },
            //{ ResourceType.MidHouse, 1 },
            //{ ResourceType.BigHouse, 1 },
            { ResourceType.SawMill, 1 },
            { ResourceType.Smithy, 1 },
            { ResourceType.Blacksmith, 1 },
            { ResourceType.CoalMine, 1 },
            { ResourceType.TradingPoint, 1},
            { ResourceType.Residents, 0}
        };

        // private void Start()
        // {
        //     AddResource(ResourceType.TradingPoint, 1);
        // }

        public void AddResource(ResourceType type, int amount)
        {
            if (Resources.ContainsKey(type))
            {
                Resources[type] += amount;
            }
            else
            {
                Resources[type] = amount;
            }

        }

        public bool SpendResource(ResourceType type, int amount)
        {
            if (Resources.ContainsKey(type) && Resources[type] >= amount)
            {
                Resources[type] -= amount;
                return true;
            }

            return false;
        }

        public int GetResourceAmount(ResourceType type)
        {
            return Resources.ContainsKey(type) ? Resources[type] : 0;
        }

        public bool HasEnough(ResourceType type, int amount)
        {
            return GetResourceAmount(type) >= amount;
        }
        
        public void AddResidents(int amount)
        {
            AddResource(ResourceType.Residents, amount);
            EconomyEvents.OnResidentsChanged?.Invoke();
            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            if (GetResourceAmount(ResourceType.Residents) >= GetResidentsNeededForLevel(CurrentLevel + 1))
            {
                CurrentLevel++;
                Debug.Log($"Level up! Current level: {CurrentLevel}");
                EconomyEvents.OnLevelChanged?.Invoke();
            }
        }

        private int GetResidentsNeededForLevel(int level)
        {
            return level * 25;
        }
        
        public static class EconomyEvents
        {
            public static Action OnResidentsChanged;
            public static Action OnLevelChanged;
        }
    }
}
