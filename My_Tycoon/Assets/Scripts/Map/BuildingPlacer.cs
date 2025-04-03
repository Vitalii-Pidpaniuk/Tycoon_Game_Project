using UnityEngine;
using UnityEngine.EventSystems;

namespace Map
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject buildingPrefab;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask buildingLayer;
        [SerializeField] private float gridSize = 1f;
        [SerializeField] private Material validMaterial, invalidMaterial;

        private GameObject ghostBuilding;
        private GameObject selectedBuilding;
        private bool isDragging = false;

        private bool _isBuilding = false;
        private bool _isReplacing = false;
        private bool _isAdjusting = false;

        private void Start()
        {
            CreateGhostBuilding();
            ghostBuilding.SetActive(false);
        }

        private void Update()
        {
            if (_isBuilding)
            {
                HandleBuildingMode();
            }
            else if (_isReplacing)
            {
                HandleReplacingMode();
            }
            else if (_isAdjusting)
            {
                HandleAdjustingMode();
            }
        }

        // ========== BUILDING MODE ==========
        private void HandleBuildingMode()
        {
            MoveGhostToMouse();

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                TryPlaceBuilding();
            }
        }

        // ========== REPLACING MODE (Видалення будівлі) ==========
        private void HandleReplacingMode()
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildingLayer))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Building"))
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }

        // ========== ADJUSTING MODE (Переміщення будівлі) ==========
        private void HandleAdjustingMode()
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                if (!isDragging)
                {
                    // Піднімаємо будівлю
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildingLayer))
                    {
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Building"))
                        {
                            selectedBuilding = hit.collider.gameObject;
                            isDragging = true;
                        }
                    }
                }
                else
                {
                    // Фіксуємо будівлю на місці
                    if (selectedBuilding != null)
                    {
                        Vector3 snappedPosition = GetSnappedMousePosition();
                        selectedBuilding.transform.position = snappedPosition;
                        selectedBuilding = null;
                        isDragging = false;
                    }
                }
            }

            // Переміщуємо будівлю за курсором
            if (isDragging && selectedBuilding != null)
            {
                selectedBuilding.transform.position = GetSnappedMousePosition();
            }
        }

        // ========== ФУНКЦІЇ ==========

        private void CreateGhostBuilding()
        {
            ghostBuilding = Instantiate(buildingPrefab);
            ApplyGhostMaterial(ghostBuilding, validMaterial);
        }

        private void MoveGhostToMouse()
        {
            Vector3 position = GetSnappedMousePosition();
            bool canBuild = !IsOccupied(position);
            ApplyGhostMaterial(ghostBuilding, canBuild ? validMaterial : invalidMaterial);
            ghostBuilding.transform.position = position;
        }

        private Vector3 GetSnappedMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                return SnapToGrid(hit.point);
            }
            return Vector3.zero;
        }

        private Vector3 SnapToGrid(Vector3 position)
        {
            return new Vector3(
                Mathf.Round(position.x / gridSize) * gridSize,
                1,
                Mathf.Round(position.z / gridSize) * gridSize
            );
        }

        private bool IsOccupied(Vector3 position)
        {
            return Physics.CheckSphere(position, 0.4f, buildingLayer);
        }

        private void TryPlaceBuilding()
        {
            Vector3 position = ghostBuilding.transform.position;
            if (!IsOccupied(position))
            {
                GameObject newBuilding = Instantiate(buildingPrefab, position, Quaternion.identity);
                newBuilding.layer = LayerMask.NameToLayer("Building");
            }
        }

        private void ApplyGhostMaterial(GameObject obj, Material mat)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.material = mat;
            }
        }

        public void SetBuildingMode(bool isBuilding)
        {
            _isBuilding = isBuilding;
            _isReplacing = false;
            _isAdjusting = false;
            isDragging = false;
            selectedBuilding = null;
            ghostBuilding.SetActive(isBuilding);
        }

        public void SetReplacingMode(bool isReplacing)
        {
            _isBuilding = false;
            _isReplacing = isReplacing;
            _isAdjusting = false;
            isDragging = false;
            selectedBuilding = null;
            ghostBuilding.SetActive(false);
        }

        public void SetAdjustingMode(bool isAdjusting)
        {
            _isBuilding = false;
            _isReplacing = false;
            _isAdjusting = isAdjusting;
            isDragging = false;
            selectedBuilding = null;
            ghostBuilding.SetActive(false);
        }

        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}
