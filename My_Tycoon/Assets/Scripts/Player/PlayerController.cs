using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Pan Settings")]
        [SerializeField] private float panSpeed = 0.5f;
        [SerializeField] private Vector2 panLimitX = new Vector2(-50f, 50f);
        [SerializeField] private Vector2 panLimitZ = new Vector2(-50f, 50f);

        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float minZoom = 10f;
        [SerializeField] private float maxZoom = 50f;

        private Vector3 lastMousePosition;
        public bool isDragging = false;

        private void Update()
        {
            HandleMousePan();
            HandleZoom();
        }

        private void HandleMousePan()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 move = new Vector3(-delta.x * panSpeed * Time.deltaTime, 0, -delta.y * panSpeed * Time.deltaTime);

                transform.position += move;
                lastMousePosition = Input.mousePosition;

                Vector3 pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, panLimitX.x, panLimitX.y);
                pos.z = Mathf.Clamp(pos.z, panLimitZ.x, panLimitZ.y);
                transform.position = pos;
            }
        }

        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Vector3 pos = transform.position;
            pos.y -= scroll * zoomSpeed * 100f * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);
            transform.position = pos;
        }
    }
   
}