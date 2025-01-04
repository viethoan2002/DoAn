using DG.Tweening;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Gameplay.Map
{
    public class G4_ChickenNode : G4_BaseNode
    {
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private Animator animator;
        private Tween _currentTween;

        public void Movement(Vector2Int direction,float movementSpeed)
        {
            animator.CrossFade("Idle",0);
            _currentTween = transform.DOMove((Vector2)transform.position+(Vector2)direction, movementSpeed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear);
        }

        public void SheepDance()
        {
            animator.CrossFade("Bouncing",0);
        }
        public bool CheckDirection(Vector2Int direction)
        {
            int x, y;
            G4_Grid.Instance.GetXY(transform.position,out x,out y);
            if (G4_Grid.Instance.GetPathNode(x + direction.x, y + direction.y) == null)
                return false;
            
            var hit = Physics2D.Raycast((Vector2)transform.position + .5f * (Vector2)direction, (Vector2)direction, .5f, targetLayer);
            return hit.collider == null;
        }
    }
}
