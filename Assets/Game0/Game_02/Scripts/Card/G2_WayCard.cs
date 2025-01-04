using System;
using System.Collections.Generic;
using DG.Tweening;
using Game0.Game_02.Scripts.UI;
using Gameplay;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Card
{
    public enum G2_DirectionType
    {
        None = -1,
        Top = 0,
        Right = 1,
        Bottom = 2,
        Left = 3,
    }
    public class G2_WayCard : G2_BaseCard,ICardInteract
    {
        public List<G2_DirectionType> directionsCanMove = new List<G2_DirectionType>();
        [FormerlySerializedAs("interactCardBtn")] [FormerlySerializedAs("spellCardBtn")] [SerializeField] private G2_InteractCardBtn g2InteractCardBtn;
        [SerializeField] private SpriteRenderer modelRenderer;
        [SerializeField] private SpriteRenderer outline;
        private Collider2D _col;

        public bool isDeadEnd;
        private bool _isInteracting;

        private void Awake()
        {
            _col = GetComponent<Collider2D>();
        }

        public Vector2Int GetVector2ByDirectionType(G2_DirectionType g2DirectionType)
        {
            switch (g2DirectionType)
            {
                case G2_DirectionType.Top:
                    return Vector2Int.RoundToInt(transform.up);
                case G2_DirectionType.Right:
                    return Vector2Int.RoundToInt(transform.right);
                case G2_DirectionType.Bottom:
                    return Vector2Int.RoundToInt(-transform.up);
                case G2_DirectionType.Left:
                    return Vector2Int.RoundToInt(-transform.right);
            }
            
            return Vector2Int.zero;
        }

        public bool IsOpen(Vector2Int direction)
        {
            foreach (var di in directionsCanMove)
            {
                if (direction == GetVector2ByDirectionType(di))
                {
                    return true;
                }
            }

            return false;
        }

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

        public void MoveToBoard(Vector3 position,Action action=null)
        {
            outline.enabled = false;
            _isInteracting = true;
            transform.DOMove(position, 0.5f).OnComplete(() =>
            {
                modelRenderer.sortingOrder = 2;
                _col.enabled = false;
                action?.Invoke();
            });
            transform.DOScale(Vector3.one, 0.5f);
        }

        public void OnClickObject()
        {
            if (!_isInteracting)
            {
                _isInteracting = true;
                Vector3 angles = transform.eulerAngles;
          
                transform.DORotate(new Vector3(angles.x, angles.y, angles.z + 90), 0.25f).OnComplete(() =>
                {
                    _isInteracting = false;
                    G2_BoardCtrl.Instance.ShowNodeCanThrowWayCard(this);
                });
            }
        }
    }
}
