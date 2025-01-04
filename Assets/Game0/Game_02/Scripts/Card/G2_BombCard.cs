using DG.Tweening;
using Game0.Game_02.Scripts.UI;
using Gameplay;
using PathFinding;
using UI;
using UnityEngine;

namespace Card
{
    public class G2_BombCard : G2_BaseCard,ICardInteract
    {
        [SerializeField] private SpriteRenderer modelRenderer;
        [SerializeField] private SpriteRenderer outline;
        private bool _isInteracting;
        public bool IsInteractable()
        {
            return _isInteracting;
        }

        public void UseCard()
        {
            if (!_isInteracting)
            {
                _isInteracting = true;
                transform.parent = G2_BoardCtrl.Instance.transform;
                G2_BoardCtrl.Instance.SetSelectedCard(this);
                transform.DORotate(Vector3.zero, 0.25f).OnComplete(() =>
                {
                    _isInteracting = false;
                    modelRenderer.sortingOrder = 4;
                    G2_PopupCtrl.Instance.ActiveButtonRevert(true);
                });
            }

        }

        public void OnClickObject()
        {

        }
        
        public void MoveToBoard(G2_PathNode node)
        {
            if (!_isInteracting)
            {
                _isInteracting = true;
                outline.enabled = false;
                Vector3 pos = new Vector3(node.transform.position.x, node.transform.position.y, -1);
                transform.DOMove(pos, 0.5f).OnComplete(() =>
                {
                    //modelRenderer.sortingOrder = 2;
                    _isInteracting = false;
                    var effect = G2_ObjectPool.Instance.Get(G2_ObjectPool.Instance.fire);
                    effect.transform.position = node.transform.position;
                    G2_SpawnCard.Instance.ReturnInteractCard(GetComponent<G2_InteractCardBtn>());
                    G2_WayCard card = node.cardInNode.GetComponent<G2_WayCard>();
                    if (card != null && card.directionsCanMove.Count > 0)
                    {
                        G2_SpawnCard.Instance.ReturnInteractCard(card.GetComponent<G2_InteractCardBtn>());
                    }
                    else
                    {
                        G2_ObjectPool.Instance.Return(card.gameObject,true);
                    }
                });
                transform.DOScale(Vector3.one, 0.5f);
            }
        }
    }
}
