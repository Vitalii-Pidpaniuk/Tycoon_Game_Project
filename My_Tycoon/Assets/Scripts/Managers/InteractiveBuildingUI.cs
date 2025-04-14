using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class InteractiveBuildingUI : MonoBehaviour
    {
        [SerializeField] private RectTransform uiPanel;
        [SerializeField] private Vector3 panelOffset = new Vector3(0, 2f, 0);

        private Camera mainCamera;
        private bool isVisible = false;

        void Start()
        {
            mainCamera = Camera.main;
            if (uiPanel != null)
                uiPanel.gameObject.SetActive(false);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUI())
                {
                    return;
                }

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        ShowPanel();
                    }
                    else
                    {
                        HidePanel();
                    }
                }
                else
                {
                    HidePanel();
                }
            }

            if (isVisible && uiPanel != null)
            {
                Vector3 worldPos = transform.position + panelOffset;
                Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
                uiPanel.position = screenPos;
            }
        }

        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        public void ShowPanel()
        {
            if (uiPanel != null)
            {
                uiPanel.gameObject.SetActive(true);
                isVisible = true;
            }
        }

        public void HidePanel()
        {
            if (uiPanel != null)
            {
                uiPanel.gameObject.SetActive(false);
                isVisible = false;
            }
        }
    }
}
