using System;
using DG.Tweening;
using Gameplay.Map;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Rope
{
    public class G4_GrapplingGun : MonoBehaviour
    {
        [FormerlySerializedAs("grappleRope")] [Header("Scripts:")]
        public G4_GrappleRope g4GrappleRope;
        [Header("Layer Settings:")]
        [SerializeField] private bool grappleToAll = false;
        [SerializeField] private LayerMask grappableLayer;
        [SerializeField] private LayerMask chickenLayer ;

        [Header("Main Camera")]
        public Camera m_camera;

        [Header("Transform Refrences:")]
        public Transform gunHolder;
        public Transform gunPivot;
        public Transform firePoint;

        [Header("Rotation:")]
        [SerializeField] private bool rotateOverTime = true;
        [Range(0, 80)] [SerializeField] private float rotationSpeed = 4;

        [Header("Distance:")]
        [SerializeField] private bool hasMaxDistance = true;
        [SerializeField] private float maxDistance = 4;

        [Header("Launching")]
        [SerializeField] private bool launchToPoint = true;
        [Range(0, 10)] [SerializeField] private float launchSpeed = 5;

        [Header("No Launch To Point")]
        [SerializeField] private bool autoCongifureDistance = false;
        [SerializeField] private float targetDistance = 3;
        [SerializeField] private float targetFrequency = 3;

        public Vector2 grapplePoint;
        [FormerlySerializedAs("DistanceVector")] [HideInInspector] public Vector2 distanceVector;
        Vector2 _mouseFirePointDistanceVector;

        private bool _isShooting = false;
        private G4_ChickenNode _currentG4ChickenNode;

        private void Start()
        {
            g4GrappleRope.enabled = false;
        }

        private void Update()
        {
            if (_isShooting)
            {
                if (launchToPoint && g4GrappleRope.isGrappling)
                {
                    if(_currentG4ChickenNode == null)
                        MovePlayer();
                    else
                        MoveChicken();
                }
            }
        }

        private void MovePlayer()
        {
            int x, y;
            _isShooting = false;
            G4_Grid.Instance.GetXY(grapplePoint - 0.1f * _mouseFirePointDistanceVector, out x, out y);
            gunHolder.DOMove(G4_Grid.Instance.GetWorldPosition(x, y),
                    launchSpeed).SetSpeedBased(true)
                .SetEase(Ease.Linear).OnComplete(
                    () =>
                    {
                        g4GrappleRope.enabled = false;
                        G4_GameManager.Instance.CheckGame();
                        G4_UICtrl.Instance.ResetButtonCtrl();
                    });
        }

        private void MoveChicken()
        {
            int x, y;
            _isShooting = false;
            G4_Grid.Instance.GetXY((Vector2)firePoint.position + 0.1f * _mouseFirePointDistanceVector, out x, out y);
            _currentG4ChickenNode.transform.DOMove(G4_Grid.Instance.GetWorldPosition(x, y), launchSpeed).SetSpeedBased(true)
                .SetEase(Ease.Linear).OnUpdate(() =>
                {
                    RaycastHit2D hit = Physics2D.Raycast(firePoint.position, gunPivot.right,20,chickenLayer);
                    grapplePoint = hit.point;
                }).OnComplete(
                    () =>
                    {
                        g4GrappleRope.enabled = false;
                        G4_GameManager.Instance.CheckGame();
                        G4_UICtrl.Instance.ResetButtonCtrl();
                    });
        }

        public void Shooting(Vector2Int direction)
        {
            _currentG4ChickenNode = null;
            gunPivot.right = (Vector2)direction;
            _mouseFirePointDistanceVector = gunPivot.right;
            
            _isShooting = SetGrapplePoint(chickenLayer);
            if (!_isShooting)
                _isShooting = SetGrapplePoint(grappableLayer);
        }

        bool SetGrapplePoint(LayerMask layer)
        {
            if (Physics2D.Raycast(firePoint.position, _mouseFirePointDistanceVector.normalized,float.MaxValue,layer))
            {
                RaycastHit2D hit = Physics2D.Raycast(firePoint.position, _mouseFirePointDistanceVector.normalized,float.MaxValue,layer);
                if (hit.collider != null)
                {
                    _currentG4ChickenNode = hit.transform.GetComponent<G4_ChickenNode>();
                }
                if (((Vector2.Distance(hit.point, firePoint.position) <= maxDistance) || !hasMaxDistance))
                {
                    grapplePoint = hit.point;
                    distanceVector = grapplePoint - (Vector2)gunPivot.position;
                    g4GrappleRope.enabled = true;
                    return true;
                }
                else
                    return false;
            }
            else return false;
        }

        private void OnDrawGizmos()
        {
            if (hasMaxDistance)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(firePoint.position, maxDistance);
            }
        }
    }
}
