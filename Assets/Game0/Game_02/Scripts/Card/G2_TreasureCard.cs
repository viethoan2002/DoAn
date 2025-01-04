using System;
using DG.Tweening;
using Game0.Game_02.Scripts.Gameplay;
using Gameplay;
using UnityEngine;
using UnityEngine.Serialization;

namespace Card
{
    public class G2_TreasureCard : G2_BaseCard
    {
        [SerializeField] private Transform mask;
        [SerializeField] private Transform island;
        [SerializeField] private ParticleSystem effect;
        [FormerlySerializedAs("wayCard")] [SerializeField] private G2_WayCard g2WayCard;

        [SerializeField] private bool isOpen;
        public bool hasTreasure;

        public void SetupTreasure(bool en)
        {
            hasTreasure = en;
            island.gameObject.SetActive(en);
            mask.gameObject.SetActive(true);
            if (!en)
            {
                g2WayCard=G2_ObjectPool.Instance.Get(G2_ObjectPool.Instance.wayCards[UnityEngine.Random.Range(0, G2_ObjectPool.Instance.wayCards.Count)]).GetComponent<G2_WayCard>();
                g2WayCard.transform.position = transform.position + new Vector3(0, 0, 0.5f);
            }
            else
            {
                if (g2WayCard != null)
                {
                    G2_ObjectPool.Instance.Return(g2WayCard.gameObject,true);
                    g2WayCard = null;
                }
            }
        }

        public void ShowTreasure()
        {
            if (isOpen)
                return;
            
            isOpen = true;
            effect.Play();
            mask.gameObject.SetActive(false);
            
            if (hasTreasure)
            {
                G2_GameManager.Instance.GameWin();
            }
            else
            {
                G2_BoardCtrl.Instance.AddWayCardToBoard(g2WayCard);
            }
        }

        public bool IsOpen()
        {
            return isOpen;
        }

        public void SuggestTreasure(Action action = null)
        {
            if(isOpen)
                return;
 
            mask.DOMoveX(mask.transform.position.x + 1, 0.75f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.75f, () =>
                {
                    mask.DOMoveX(mask.transform.position.x - 1, 0.75f).OnComplete(() =>
                    {
                        action?.Invoke();
                    });
                });
            });
        }
    }
}
