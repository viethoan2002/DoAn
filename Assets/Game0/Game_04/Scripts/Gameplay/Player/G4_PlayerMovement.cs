using System;
using DG.Tweening;
using Gameplay.Map;
using Gameplay.Rope;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Player
{
    public class G4_PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [FormerlySerializedAs("grapplingGun")] [SerializeField] private G4_GrapplingGun g4GrapplingGun;
        [SerializeField] private float movementSpeed = 1.5f;
        private int _x, _y;
        private bool _canMove = true;
        [SerializeField] private LayerMask targetLayer;
        private Tween _currentTween;
        private G4_ChickenNode _currentG4Chicken;
        
        private void OnEnable()
        {
            G4_UICtrl.OnMove += Movement;
            G4_UICtrl.OnSwing += Swing;
        }

        private void OnDisable()
        {
            G4_UICtrl.OnMove -= Movement;
            G4_UICtrl.OnSwing -= Swing;
        }

        public void SetCanMove(bool en)
        {
            _canMove = en;
        }

        private void Movement(Vector2Int direction)
        {
            if (!_canMove || !CheckDirection(direction))
                return;
            
            _canMove = false;
            animator.CrossFade("Run",0);
            _currentTween = transform.DOMove((Vector2)transform.position+(Vector2)direction, movementSpeed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _canMove = true;
                    animator.CrossFade("Idle",0);
                    G4_GameManager.Instance.CheckGame();
                });

            if (_currentG4Chicken != null)
                _currentG4Chicken.Movement(direction, movementSpeed);
        }

        private bool CheckDirection(Vector2Int direction)
        {
            var hit=Physics2D.Raycast((Vector2)transform.position + 0.5f * (Vector2)direction, (Vector2)direction, .5f,targetLayer);


            if (hit.collider == null)
            {
                _currentG4Chicken = null;
                return true;
            }
            else 
            {
                _currentG4Chicken = hit.transform.GetComponent<G4_ChickenNode>();
                if (_currentG4Chicken == null)
                {
                    return false;
                }
                else
                {
                    return _currentG4Chicken.CheckDirection(direction);
                }
            }
        }

        private void Swing(Vector2Int direction)
        {
            if(G4_PlayerCtrl.Instance.g4PlayerStats.HasBin())
                g4GrapplingGun.Shooting(direction);
        }

        public void ResetMovement()
        {
            _canMove = true;
            _currentG4Chicken = null;
            CircleOutline.Instance.ScaleOut();
        }
    }
}
