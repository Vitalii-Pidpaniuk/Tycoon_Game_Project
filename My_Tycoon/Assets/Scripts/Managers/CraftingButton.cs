using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class CraftingButton : MonoBehaviour
    {
        public ResourceType itemToCraft;

        public void OnCraftButtonClicked()
        {
            if (!CraftingManager.Instance.Craft(itemToCraft))
            {
                Debug.Log($"not crafted {itemToCraft}");
            }
            else
            {
            }
        }
    }
}