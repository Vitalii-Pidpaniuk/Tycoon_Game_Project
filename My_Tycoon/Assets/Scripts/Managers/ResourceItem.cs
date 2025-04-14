using TMPro;
using UnityEngine;

namespace Managers
{
    public class ResourceItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI quantityText;
        public ResourceType itemType;

        private void OnEnable()
        {
            UpdateQuantity();
        }

        public void UpdateQuantity()
        {
            if (EconomyManager.Instance != null && quantityText != null)
            {
                quantityText.text = EconomyManager.Instance.GetResourceAmount(itemType).ToString();
            }
        }
    }
}