using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class InteractiveBuilding : Building
    {
        [SerializeField] private GameObject uiPanel;

        private static InteractiveBuilding activeBuilding;

        private void Start()
        {
            if (uiPanel != null)
                uiPanel.SetActive(false);
        }

        private void OnMouseDown()
        {
            if (activeBuilding != null && activeBuilding != this)
                activeBuilding.ClosePanel();

            if (uiPanel != null)
                uiPanel.SetActive(true);

            activeBuilding = this;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current != null &&
                    UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                if (!IsMouseOverThisBuilding())
                {
                    ClosePanel();
                    if (activeBuilding == this)
                        activeBuilding = null;
                }
            }
        }


        private bool IsMouseOverThisBuilding()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.collider != null && hit.collider.gameObject == gameObject;
            }
            return false;
        }

        public void ClosePanel()
        {
            if (uiPanel != null)
                uiPanel.SetActive(false);
        }

    }
}