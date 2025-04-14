using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class InventoryPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private ResourceItem[] resourceItems;

        public void ToggleInventory()
        {
            UpdateAllResources();
        }

        private void UpdateAllResources()
        {
            foreach (var item in resourceItems)
            {
                item.UpdateQuantity();
            }
        }
    }
}