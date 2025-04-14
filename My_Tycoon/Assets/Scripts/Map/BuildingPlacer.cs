using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

using Managers;
using ResourceType = Managers.ResourceType;

namespace Map
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask buildingLayer;
        [SerializeField] private float gridSize = 1f;
        [SerializeField] private Material validMaterial, invalidMaterial;
        [SerializeField] private LayerMask roadLayer;
        [SerializeField] private GameObject tradingPointPrefab;

        private ResourceType currentBuildingType;
        private GameObject ghostBuilding;
        private GameObject selectedBuilding;
        private bool isDragging = false;
        private GameObject currentBuildingPrefab;

        public bool _isBuilding = false;
        public bool _isReplacing = false;
        public bool _isAdjusting = false;
        
        Vector3 position;

        bool validGround, isFree, nearRoad, isRoad;

        private void Start()
        {
            if (ghostBuilding != null)
                ghostBuilding.SetActive(false);

            //Instantiate(tradingPointPrefab, new Vector3(0, 1, 1), quaternion.identity);
        }

        private void Update()
        {
            if (_isBuilding) HandleBuildingMode();
            else if (_isReplacing) HandleReplacingMode();
            else if (_isAdjusting) HandleAdjustingMode();
            else
            {
                if (ghostBuilding != null)
                {
                    Destroy(ghostBuilding);
                }
            }
        }

        public void StartBuildingMode(GameObject newBuildingPrefab, ResourceType resourceType)
        {
            Debug.Log("StartBuildingMode: " + newBuildingPrefab.name);
            if (ghostBuilding != null)
                Destroy(ghostBuilding);

            currentBuildingPrefab = newBuildingPrefab;
            currentBuildingType = resourceType;

            ghostBuilding = Instantiate(currentBuildingPrefab);
            DisableColliders(ghostBuilding);
            ApplyGhostMaterial(ghostBuilding, validMaterial);
            ghostBuilding.SetActive(true);

            SetBuildingMode(true);
        }

        public void StartReplacingMode()
        {
            SetReplacingMode(true);
        }

        public void StartAdjustingMode()
        {
            SetAdjustingMode(true);
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

        // ========== REPLACING MODE ==========
        private void HandleReplacingMode()
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildingLayer | roadLayer))
                {
                    var obj = hit.collider.gameObject;
                    var toRemove = Managers.SaveLoadManager.Instance.placedBuildings
                        .FirstOrDefault(kv => kv.Value == obj).Key;
                    if (toRemove != 0)
                    {
                        Managers.SaveLoadManager.Instance.placedBuildings.Remove(toRemove);
                    }
                    Destroy(obj);
                    Managers.SaveLoadManager.Instance.SaveGame();
                }
            }
        }


        // ========== ADJUSTING MODE ==========
        private void HandleAdjustingMode()
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                if (!isDragging)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildingLayer | roadLayer))
                    {
                        selectedBuilding = hit.collider.gameObject;
                        currentBuildingPrefab = selectedBuilding;
                        if (ghostBuilding != null) Destroy(ghostBuilding);
                        ghostBuilding = Instantiate(selectedBuilding);
                        DisableColliders(ghostBuilding);
                        ApplyGhostMaterial(ghostBuilding, invalidMaterial);
                        ghostBuilding.SetActive(true);
                        selectedBuilding.SetActive(false);
                        isDragging = true;
                    }
                }
                else
                {
                    Vector3 newPosition = GetSnappedMousePosition();
                    if (!IsOccupied(newPosition))
                    {
                        selectedBuilding.transform.position = newPosition;
                        Managers.SaveLoadManager.Instance.SaveGame();
                        selectedBuilding.SetActive(true);
                        Destroy(ghostBuilding);
                        selectedBuilding = null;
                        isDragging = false;
                    }
                }
            }

            if (isDragging && selectedBuilding != null)
            {
                Vector3 ghostPos = GetSnappedMousePosition();
                ghostBuilding.transform.position = ghostPos;
                ApplyGhostMaterial(ghostBuilding, !IsOccupied(ghostPos) ? validMaterial : invalidMaterial);
            }
        }


        // ========== ФУНКЦІЇ ==========

        private void MoveGhostToMouse()
        {
            position = GetSnappedMousePosition();

            validGround = IsMouseOverTerrain();
            isFree = !IsOccupied(position);
            nearRoad = IsNearRoad(position);
            isRoad = currentBuildingPrefab.CompareTag("Road");

            bool canBuild;

            if (isRoad)
            {
                canBuild = validGround && isFree;
            }
            else
            {
                canBuild = validGround && isFree && nearRoad;
            }

            if (ghostBuilding != null)
            {
                ghostBuilding.SetActive(true);
                ghostBuilding.transform.position = position;
                ApplyGhostMaterial(ghostBuilding, canBuild ? validMaterial : invalidMaterial);
            }
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
            bool canPlace = isRoad ? (validGround && isFree) : (validGround && isFree && nearRoad);
            if (!canPlace) return;

            if (!EconomyManager.Instance.HasEnough(currentBuildingType, 1))
            {
                return;
            }

            if (!EconomyManager.Instance.SpendResource(currentBuildingType, 1))
            {
                return;
            }

            GameObject newObj = Instantiate(currentBuildingPrefab, position, Quaternion.identity);
            newObj.layer = LayerMask.NameToLayer(isRoad ? "Road" : "Building");

            int id = SaveLoadManager.Instance.GetNextId();
            SaveLoadManager.Instance.placedBuildings[id] = newObj;

            SaveLoadManager.Instance.SaveGame();
        }
        
        private bool IsMouseOverTerrain()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer)
                   && hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain");
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
            if (ghostBuilding != null) ghostBuilding.SetActive(isBuilding);
        }

        public void SetReplacingMode(bool isReplacing)
        {
            _isBuilding = false;
            _isReplacing = isReplacing;
            _isAdjusting = false;
            isDragging = false;
            selectedBuilding = null;
            if (ghostBuilding != null) ghostBuilding.SetActive(false);
            HandleReplacingMode();
        }

        public void SetAdjustingMode(bool isAdjusting)
        {
            _isBuilding = false;
            _isReplacing = false;
            _isAdjusting = isAdjusting;
            isDragging = false;
            selectedBuilding = null;
            if (ghostBuilding != null) ghostBuilding.SetActive(false);
            HandleAdjustingMode();
        }

        private bool IsNearRoad(Vector3 position)
        {
            float offset = gridSize;
            Vector3[] offsets = new Vector3[]
            {
                new Vector3(offset, 0, 0),
                new Vector3(-offset, 0, 0),
                new Vector3(0, 0, offset),
                new Vector3(0, 0, -offset),
            };

            foreach (var dir in offsets)
            {
                if (Physics.CheckSphere(position + dir, 0.3f, roadLayer))
                    return true;
            }

            return false;
        }
        
        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
        
        private void DisableColliders(GameObject obj)
        {
            foreach (var collider in obj.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }
        
        private bool IsValidAdjustingPosition(Vector3 targetPos, GameObject ignoreObject)
        {
            Collider[] colliders = Physics.OverlapSphere(targetPos, 0.4f, buildingLayer);
            foreach (var col in colliders)
            {
                if (col.gameObject != ignoreObject)
                    return false;
            }

            return IsMouseOverTerrain();
        }
    }
} 