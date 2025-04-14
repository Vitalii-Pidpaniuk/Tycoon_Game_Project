using Managers;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class EconomyUIUpdater : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text residentsText;

        private void Start()
        {
            UpdateUI();
        }

        private void OnEnable()
        {
            EconomyManager.EconomyEvents.OnResidentsChanged += UpdateUI;
            EconomyManager.EconomyEvents.OnLevelChanged += UpdateUI;
        }

        private void OnDisable()
        {
            EconomyManager.EconomyEvents.OnResidentsChanged -= UpdateUI;
            EconomyManager.EconomyEvents.OnLevelChanged -= UpdateUI;
        }

        private void UpdateUI()
        {
            levelText.text = $"{EconomyManager.Instance.CurrentLevel}";
            residentsText.text = $"{EconomyManager.Instance.GetResourceAmount(ResourceType.Residents)}";
        }
    }
}