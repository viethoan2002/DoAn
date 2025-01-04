using System;
using DG.Tweening;
using Gameplay;
using Raycast;
using UnityEngine;

namespace Card
{
    public class G2_InteractCardBtn : MonoBehaviour,IClickHandle
    {
        [SerializeField] private Vector3 originPos;
        [SerializeField] private SpriteRenderer outline;
        public bool inBoard;
        public bool isInteracting;
        
        public static Action<G2_InteractCardBtn> OnClick;
        
        public void OnClickObject()
        {
            if (inBoard)
            {
                GetComponent<ICardInteract>().OnClickObject();
            }
            else if(!isInteracting)
                OnClick?.Invoke(this);
        }

        public void OnDragObject()
        {

        }

        public void EndObject()
        {

        }

        public void ResetCard()
        {
            GetComponent<Collider2D>().enabled = false;
            ActiveOutline(false);
            SetupOrderSort(2);
            inBoard = false;
        }

        public void SetupOrderSort(int index)
        {
            GetComponent<SpriteRenderer>().sortingOrder = index;
            outline.sortingOrder = index;
        }

        public void DisplayNodeCanMove()
        {
            G2_BoardCtrl.Instance.HideNodeCanThrow();
            if (GetComponent<G2_WayCard>() != null)
                G2_BoardCtrl.Instance.ShowNodeCanThrowWayCard(GetComponent<G2_WayCard>());
            if (GetComponent<G2_BombCard>() != null)
                G2_BoardCtrl.Instance.ShowNodeCanThrowBombCard();
            if (GetComponent<G2_CompassCard>() != null)
                G2_BoardCtrl.Instance.ShowNodeTreasure();
            if(GetComponent<G2_AttackCard>() !=null)
                G2_BoardCtrl.Instance.ShowNodeHasEnemy();
        }

        public void SetOriginPos(Vector3 pos)
        {
            originPos = pos;
        }

        public void ActiveOutline(bool en)
        {
            outline.enabled = en;
        }

        public void MoveToPos(Vector3 pos)
        {
            isInteracting = true;
            var trans = GetComponent<Transform>();
            trans.DOLocalMove(originPos + pos, 0.5f).OnComplete(() => { isInteracting = false; });
        }
    }
}
