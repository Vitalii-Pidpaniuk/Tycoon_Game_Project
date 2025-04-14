using UnityEngine;
using UnityEngine.UI;
using Managers;
using Map;

namespace Managers
{
    public class BuildButton : MonoBehaviour
    {
        public GameObject prefab;
        public ResourceType resourceType;

        [SerializeField] private BuildingPlacer buildPlacer;

        public void OnClickBuild()
        {
            if (!EconomyManager.Instance.HasEnough(resourceType, 1))
            {
                Debug.LogWarning($"Недостатньо {resourceType} для будівництва.");
                return;
            }

            buildPlacer.StartBuildingMode(prefab, resourceType);
        }
    }   
}