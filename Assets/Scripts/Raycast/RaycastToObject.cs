using System;
using Gameplay;
using Unity.VisualScripting;
using UnityEngine;

namespace Raycast
{
    public class RaycastToObject : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera cam;
        [SerializeField] private LayerMask layerMask;

        private IClickHandle _currentClickHandle;
        private Vector3 _lastMousePosition;
        private bool _isDown;
        private bool _isDragging;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            
                RaycastHit2D hit = Physics2D.Raycast(_lastMousePosition, Vector2.zero);
                if (hit.collider != null)
                {
                    _isDown = true;
                    if (hit.collider.GetComponent<IClickHandle>() != null)
                    {
                        _currentClickHandle = hit.collider.GetComponent<IClickHandle>();
                        _currentClickHandle.OnClickObject();
                    }
                }
            }

            if (_isDown && !_isDragging)
            {
                Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(mousePosition, _lastMousePosition) > 0.1f)
                {
                    if(_currentClickHandle != null)
                        _isDragging = true;
                }
            }

            if (_isDragging)
            {
                if (_currentClickHandle != null) _currentClickHandle.OnDragObject();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDown = false;
                _isDragging = false;
                if (_currentClickHandle != null)
                {
                    _currentClickHandle.EndObject();
                    _currentClickHandle = null;
                }
            }
        }
    }
}
