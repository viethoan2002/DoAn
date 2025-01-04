using System.Collections;
using System.Collections.Generic;
using _Camera;
using DG.Tweening;
using Game0.Game_02.Scripts.UI;
using Gameplay;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Card
{
    public class InteractCardManager : MonoBehaviour
    {
        public static InteractCardManager Instance;
        [SerializeField] private Transform content;
        [SerializeField] private List<G2_InteractCardBtn> interactCardBtns = new List<G2_InteractCardBtn>();
        [SerializeField] private Transform origin;
        [SerializeField] private Transform posUse;
        private int _center;

        private readonly float _distanceSpellShow = 11.25f;
        private readonly float _distanceSpellHie = 5f;

        private G2_InteractCardBtn _currentG2InteractCardBtn;
        [FormerlySerializedAs("_readyInteract")] [SerializeField] private bool readyInteract;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            G2_InteractCardBtn.OnClick += SelectSpell;
        }

        private void OnDisable()
        {
            G2_InteractCardBtn.OnClick -= SelectSpell;
        }
        
        public void ShowSpellCard()
        {
            _center = interactCardBtns.Count / 2;
            SortSpellCardUI(_distanceSpellShow);
            Invoke(nameof(FillCardInBoard), 0.75f);
        }
        
        private void HideSpellCard()
        {
            _center = interactCardBtns.Count / 2;
            SortSpellCardUI(_distanceSpellHie);
        }

        public void ReturnAllInteractCard()
        {
            foreach (var t in interactCardBtns)
            {
                G2_SpawnCard.Instance.ReturnInteractCard(t,0);
            }
            
            _currentG2InteractCardBtn = null;
            interactCardBtns.Clear();
        }

        public void UseCurrentCard()
        {
            _currentG2InteractCardBtn.transform.DOMove(posUse.position, 0.5f);
            _currentG2InteractCardBtn.GetComponent<ICardInteract>().UseCard();
            _currentG2InteractCardBtn.inBoard = true;
            
            CameraManager.Instance.CameraZoomOut();
            
            interactCardBtns.Remove(_currentG2InteractCardBtn);
            _currentG2InteractCardBtn = null;
            HideSpellCard();
            readyInteract = false;
        }

        private void SelectSpell(G2_InteractCardBtn cardBtn)
        {
            if (!readyInteract)
                return;
            
            if (_currentG2InteractCardBtn != cardBtn)
            {
                var trans = cardBtn.GetComponent<Transform>();
                cardBtn.MoveToPos(trans.up * 1.75f);
                
                if (_currentG2InteractCardBtn != null)
                {
                    trans = _currentG2InteractCardBtn.GetComponent<Transform>();
                    _currentG2InteractCardBtn.MoveToPos(Vector2.zero);
                    _currentG2InteractCardBtn.ActiveOutline(false);
                }
                
                _currentG2InteractCardBtn = cardBtn;
                _currentG2InteractCardBtn.ActiveOutline(true);
            }
            else
            {
                var rect = _currentG2InteractCardBtn.GetComponent<Transform>();
                _currentG2InteractCardBtn.MoveToPos(Vector2.zero);
                _currentG2InteractCardBtn.ActiveOutline(false);
                _currentG2InteractCardBtn = null;
            }

            if (_currentG2InteractCardBtn != null)
                _currentG2InteractCardBtn.DisplayNodeCanMove();
            else
                G2_BoardCtrl.Instance.HideNodeCanThrow();

            ShowButtonUse();
        }

        public void UnSelectSpell()
        {
            var interactCard = G2_BoardCtrl.Instance.GetSelectedCard().GetComponent<G2_InteractCardBtn>();
            interactCard.transform.SetParent(content);
            interactCard.inBoard = false;
            interactCard.ActiveOutline(false);
            interactCardBtns.Add(interactCard);
            
            G2_BoardCtrl.Instance.SetSelectedCard(null);
            G2_BoardCtrl.Instance.HideNodeCanThrow();
            
            CameraManager.Instance.CameraZoomIn();
            SortSpellCardUI(_distanceSpellShow);
            DOVirtual.DelayedCall(0.5f, () => { readyInteract = true;});
        }

        public void RemoveCard()
        {
            interactCardBtns.Remove(_currentG2InteractCardBtn);
            G2_SpawnCard.Instance.ReturnInteractCard(_currentG2InteractCardBtn);
            _currentG2InteractCardBtn = null;
            
            G2_BoardCtrl.Instance.SetSelectedCard(null);
            G2_BoardCtrl.Instance.HideNodeCanThrow();
            
            FillCardInBoard();
        }

        private void ShowButtonUse()
        {
            G2_PopupCtrl.Instance.ActiveButtonUse(_currentG2InteractCardBtn != null);
        }

        public void FillCardInBoard()
        {
            readyInteract = false;
            StartCoroutine(FillCard());
        }

        IEnumerator FillCard()
        {
            while (interactCardBtns.Count < 5 && G2_SpawnCard.Instance.HadCard())
            {
                interactCardBtns.Add(null);
                _center = interactCardBtns.Count / 2;
                SortSpellCardUI(_distanceSpellShow,0.25f);
                yield return new WaitForSeconds(0.35f);
                
                interactCardBtns[^1]=G2_SpawnCard.Instance.GetInteractCard();
                interactCardBtns[^1].transform.position += new Vector3(0, 0, -0.1f * interactCardBtns.Count);
                interactCardBtns[^1].GetComponent<Collider2D>().enabled = true;
                interactCardBtns[^1].transform.parent = content;
                interactCardBtns[^1].SetupOrderSort(4);
                
                SortSpellCardUI(_distanceSpellShow,0.25f);
                yield return new WaitForSeconds(0.35f);
            }
            
            readyInteract = true;
            G2_BoardCtrl.Instance.ThrowEnemy();
        }
        
        private void MoveSpellCardUIOdd(int index, float distance, float duration = 0.5f)
        {
            var trans = interactCardBtns[index].GetComponent<Transform>();
            var amount = index < _center
                ? 6.5f * (interactCardBtns.Count - 1) / 2 - index * 6.5f
                : -6.5f * (interactCardBtns.Count - 1) / 2 + (interactCardBtns.Count - index -1) * 6.5f;
            origin.eulerAngles = new Vector3(0, 0, amount);
            trans.DORotate(new Vector3(0, 0, amount), duration).SetEase(Ease.Linear);
            trans.DOScale(new Vector3(2.5f,2.5f,2.5f), duration).SetEase(Ease.Linear);

            Vector3 cardOrigin = new Vector3(origin.position.x, origin.position.x, -0.1f * index) + origin.up * distance;
            interactCardBtns[index].SetOriginPos(cardOrigin);
            trans.DOLocalMove(cardOrigin, duration).SetEase(Ease.Linear);
        }

        private void MoveSpellCardUIEven(int index, float distance, float duration = 0.5f)
        {
            var trans = interactCardBtns[index].GetComponent<Transform>();

            origin.eulerAngles = new Vector3(0, 0, (_center - index) * 6.5f);
            trans.DORotate(new Vector3(0, 0, (_center - index) * 6.5f), duration).SetEase(Ease.Linear);
            trans.DOScale(new Vector3(2.5f,2.5f,2.5f), duration).SetEase(Ease.Linear);

            Vector3 cardOrigin = new Vector3(origin.position.x, origin.position.x, -0.1f * index) + origin.up * distance;
            interactCardBtns[index].SetOriginPos(cardOrigin);
            trans.DOLocalMove(cardOrigin, duration).SetEase(Ease.Linear);
        }

        private void SortSpellCardUI(float distance, float duration = .5f)
        {
            if (interactCardBtns.Count % 2 == 0)
            {
                for (int i = 0; i < interactCardBtns.Count; i++)
                {
                    if(interactCardBtns[i]!= null)
                        MoveSpellCardUIOdd(i, distance, duration);
                }
            }      
            else {
                for (int i = 0; i < interactCardBtns.Count; i++)
                {
                    if(interactCardBtns[i]!= null)
                        MoveSpellCardUIEven(i, distance, duration);
                }
            }
        }
    }
}
