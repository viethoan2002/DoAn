using DG.Tweening;
using Game0.Game_02.Scripts.UI;
using Gameplay;
using UI;
using UnityEngine;

namespace Card
{
    public class G2_CompassCard : G2_BaseCard,ICardInteract
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
    }
}
