using System.Collections.Generic;
using UnityEngine;
using Managers;
using Map;

namespace Managers
{
    public class CraftingManager : MonoBehaviour
    {
        public static CraftingManager Instance;
        [SerializeField] private int smallHouseResidents = 10;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            InitializeRecipes();
        }

        private Dictionary<ResourceType, Dictionary<ResourceType, int>> recipes = new Dictionary<ResourceType, Dictionary<ResourceType, int>>();

        private void InitializeRecipes()
        {
            recipes[ResourceType.Road] = new Dictionary<ResourceType, int>
            {
                { ResourceType.Stone, 3 }
            };

            recipes[ResourceType.SmallHouse] = new Dictionary<ResourceType, int>
            {
                { ResourceType.Stone, 2 },
                { ResourceType.Wood, 2 }
            };
            
            recipes[ResourceType.SawMill] = new Dictionary<ResourceType, int>
            {
                { ResourceType.Stone, 2 },
                { ResourceType.Wood, 2 },
                { ResourceType.Iron, 2 }
            };
            
            recipes[ResourceType.CoalMine] = new Dictionary<ResourceType, int>
            {
                { ResourceType.Stone, 2 },
                { ResourceType.Wood, 2 },
                { ResourceType.Iron, 2 }
            };
            
            recipes[ResourceType.Smithy] = new Dictionary<ResourceType, int>
            {
                { ResourceType.Stone, 2 },
                { ResourceType.Wood, 2 },
                { ResourceType.Iron, 2 }
            };
            
            recipes[ResourceType.Blacksmith] = new Dictionary<ResourceType, int>
            {
                { ResourceType.Coal, 2 },
                { ResourceType.Wood, 2 },
                { ResourceType.Iron, 2 }
            };
        }

        public bool CanCraft(ResourceType item)
        {
            if (!recipes.ContainsKey(item)) return false;

            foreach (var ingredient in recipes[item])
            {
                if (!EconomyManager.Instance.HasEnough(ingredient.Key, ingredient.Value))
                    return false;
            }
            return true;
        }

        public bool Craft(ResourceType item)
        {
            if (!CanCraft(item)) return false;

            foreach (var ingredient in recipes[item])
            {
                EconomyManager.Instance.SpendResource(ingredient.Key, ingredient.Value);
            }

            EconomyManager.Instance.AddResource(item, 1);
            Debug.Log($"Crafted: {item}");

            switch (item)
            {
                case ResourceType.SmallHouse:
                    EconomyManager.Instance.AddResidents(smallHouseResidents);
                    break;
            }

            return true;
        }

        public Dictionary<ResourceType, int> GetRecipe(ResourceType item)
        {
            return recipes.ContainsKey(item) ? recipes[item] : null;
        }
    }
}
