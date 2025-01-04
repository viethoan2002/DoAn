using System;
using System.Collections.Generic;
using DG.Tweening;
using Game0.Game_02.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Card
{
    public class G2_SpawnCard : MonoBehaviour
    {
        public static G2_SpawnCard Instance;
        [SerializeField] private Transform start, end;
        [SerializeField] private List<G2_InteractCardBtn> cardBtns = new List<G2_InteractCardBtn>();
        [SerializeField] private List<G2_InteractCardBtn> cardBtnsQueue = new List<G2_InteractCardBtn>();
        [SerializeField] private Text txtCount;

        private void Awake()
        {
            Instance = this;
        }
        
        public void SetupSpawnCard()
        {
            cardBtns.AddRange(cardBtnsQueue);
            cardBtnsQueue.Clear();
            Shuffle(cardBtns);
            txtCount.text = $"{cardBtns.Count}/40";
            for (int i = 0; i < cardBtns.Count; i++)
            {
                cardBtns[i].transform.SetParent(start);
                cardBtns[i].ResetCard();
                cardBtns[i].transform.localScale=new Vector3(2, 2, 2);
                cardBtns[i].transform.eulerAngles = new Vector3(0, 180, -20);
                cardBtns[i].transform.localPosition = new Vector3(0, 0, 0.001f * i);
            }
        }
        
        void Shuffle<T>(List<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public bool HadCard()
        {
            return cardBtns.Count > 0;
        }

        public G2_InteractCardBtn GetInteractCard()
        {
            var card = cardBtns[0];
            card.transform.position -= new Vector3(0, 0, 1);
            cardBtns.RemoveAt(0);
            txtCount.text = $"{cardBtns.Count}/40";
            cardBtnsQueue.Add(card);
            
            if(cardBtns.Count==0)
                G2_GameManager.Instance.GameLose();
            return card;
        }

        public void ReturnInteractCard(G2_InteractCardBtn g2InteractCardBtn,float duration=0.5f)
        {
            //cardBtnsQueue.Add(g2InteractCardBtn);
            g2InteractCardBtn.transform.SetParent(end);
            g2InteractCardBtn.ActiveOutline(false);
            g2InteractCardBtn.SetupOrderSort(2);
            g2InteractCardBtn.GetComponent<Collider2D>().enabled = false;

            if (duration > 0)
            {
                g2InteractCardBtn.transform.DOScale(new Vector3(2, 2, 2), duration);
                g2InteractCardBtn.transform.DOLocalMove(new Vector3(0, 0, -0.001f * cardBtnsQueue.Count), duration);
                g2InteractCardBtn.transform.DORotate(new Vector3(0, 180, 20), duration);
            }
            else
            {
                g2InteractCardBtn.transform.localScale=new Vector3(2, 2, 2);
                g2InteractCardBtn.transform.localPosition=new Vector3(0, 0, -0.001f * cardBtnsQueue.Count);
                g2InteractCardBtn.transform.eulerAngles=new Vector3(0, 180, 20);
            }
        }
    }
}
