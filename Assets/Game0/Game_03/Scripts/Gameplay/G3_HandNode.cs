using System;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class G3_HandNode : G3_BaseNode
    {
        [FormerlySerializedAs("curBodyNode")] [SerializeField] private G3_BodyNode curG3BodyNode;
        [SerializeField] private LayerMask layerCheck;
        [SerializeField] private Vector2 vectorTarget = Vector2.zero;
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite defaultSprite,shakeSprite,pointSprite;

        public BoxCtrl boxCtrl;
        [FormerlySerializedAs("handMovement")] public G3_HandMovement g3HandMovement;

        public bool CheckCanMove(G3_DirectionType targetDir)
        {
            switch (targetDir)
            {
                case G3_DirectionType.Up:
                    vectorTarget = Vector2.up;
                    break;
                case G3_DirectionType.Down:
                    vectorTarget = Vector2.down;
                    break;
                case G3_DirectionType.Right:
                    vectorTarget = Vector2.right;
                    break;
                case G3_DirectionType.Left:
                    vectorTarget = Vector2.left;
                    break;
            }

            Collider2D hit = Physics2D.OverlapBox((Vector2)transform.position + vectorTarget, new Vector2(0.3f, 0.3f),
                0, layerCheck);
            if (hit != null)
            {
                boxCtrl=hit.GetComponent<BoxCtrl>();
                if (boxCtrl != null && Physics2D.OverlapBox((Vector2)transform.position + vectorTarget * 2, new Vector2(0.3f, 0.3f),
                        0, layerCheck) == null)
                {
                    return true;
                }
                
                var hand = hit.transform.GetComponent<G3_HandNode>();

                if (hand != null && vectorTarget == -(Vector2)hand.transform.up)
                {
                    return true;
                }
                
                
                curG3BodyNode = hit.transform.GetComponent<G3_BodyNode>();
                
            }
            else
            {
                curG3BodyNode = null;
                boxCtrl=null;
            }
            
            return hit == null;
        }

        public G3_BodyNode GetBodyNode()
        {
            return curG3BodyNode;
        }

        public void MoveForDirection(G3_DirectionType targetDir)
        {
            transform.up = vectorTarget;
            transform.position= (Vector2)transform.position + vectorTarget;
        }

        public void MoveForBodyNode(G3_BodyNode g3BodyNode)
        {
            transform.position=g3BodyNode.transform.position;
            transform.up = g3BodyNode.transform.up;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + transform.up, new Vector3(.3f, 0.3f, 0));
            Gizmos.DrawWireCube(transform.position + transform.right, new Vector3(.3f, 0.3f, 0));
            Gizmos.DrawWireCube(transform.position - transform.up, new Vector3(.3f, 0.3f, 0));
            Gizmos.DrawWireCube(transform.position - transform.right, new Vector3(.3f, 0.3f, 0));
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Hand"))
            {
                spriteRenderer.sprite = shakeSprite;
                g3HandMovement.eyesAnm.CrossFade("Win",0);
                G3GameManager.Instance.WinGame();
            }
            
            G3_SwitchCtrl g3SwitchCtrl= other.gameObject.GetComponent<G3_SwitchCtrl>();
            if (g3SwitchCtrl != null)
            {
                g3SwitchCtrl.Switch();
                spriteRenderer.sprite = pointSprite;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            G3_SwitchCtrl g3SwitchCtrl= other.gameObject.GetComponent<G3_SwitchCtrl>();
            if (g3SwitchCtrl != null)
            {
                spriteRenderer.sprite = defaultSprite;
                if(g3SwitchCtrl.isTrigger)
                    g3SwitchCtrl.Switch();
            }
        }
    }
}
