using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Map
{
    public class ResourceGeneratingBuilding : Building
    {
        [Header("Resource Settings")] 
        public ResourceType resourceType;
        public int maxStorage = 20;
        public float generationInterval = 2f;
        public int resourcePerTick = 1;

        [Header("UI")]
        [SerializeField] private RectTransform collectPanel;
        [SerializeField] private Button collectButton;
        [SerializeField] private Vector3 panelOffset = new Vector3(0, 2f, 0);

        private float timer = 0f;
        public int storedAmount = 0;

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;

            if (collectPanel != null)
                collectPanel.gameObject.SetActive(false);

            if (collectButton != null)
                collectButton.onClick.AddListener(CollectResources);
        }

        private void Update()
        {
            HandleResourceGeneration();
            UpdateCollectPanelVisibility();
            UpdatePanelPosition();
        }

        private void HandleResourceGeneration()
        {
            if (storedAmount >= maxStorage) return;

            timer += Time.deltaTime;
            if (timer >= generationInterval)
            {
                timer = 0f;
                storedAmount = Mathf.Min(storedAmount + resourcePerTick, maxStorage);
            }
        }

        private void CollectResources()
        {
            if (storedAmount <= 0) return;

            Debug.Log($"Earned {storedAmount} {resourceType}");
            EconomyManager.Instance.AddResource(resourceType, storedAmount);

            storedAmount = 0;
            timer = 0f;

            UpdateCollectPanelVisibility();
        }

        private void UpdateCollectPanelVisibility()
        {
            if (collectPanel == null) return;

            bool shouldShow = storedAmount >= 5;
            if (collectPanel.gameObject.activeSelf != shouldShow)
            {
                collectPanel.gameObject.SetActive(shouldShow);
            }
        }

        private void UpdatePanelPosition()
        {
            if (collectPanel == null || !collectPanel.gameObject.activeSelf) return;

            Vector3 worldPos = transform.position + panelOffset;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

            if (screenPos.z > 0)
            {
                collectPanel.position = screenPos;
            }
        }
    }
}
