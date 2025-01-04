using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class G3_HandMovement : MonoBehaviour
    {
        [FormerlySerializedAs("handType")] [SerializeField] private G3_HandType g3HandType;
        [FormerlySerializedAs("handNode")] [FormerlySerializedAs("_handNode")] [SerializeField] private G3_HandNode g3HandNode;
        [SerializeField] private List<G3_BaseNode> nodes = new List<G3_BaseNode>();
        public Animator eyesAnm;
        [SerializeField] private int amountTurn;
        [FormerlySerializedAs("uiTurn")] [SerializeField] private G3_UiTurn g3UiTurn;
        
        private bool _isCutting = false;
        
        private void OnEnable()
        {
            G3_MoveBtn.OnMove += MoveHand;
            G3_BarrierCtrl.CutHand += CheckHandCut;
        }

        private void OnDisable()
        {
            G3_MoveBtn.OnMove -= MoveHand;
            G3_BarrierCtrl.CutHand -= CheckHandCut;
        }

        private void Start()
        {
            g3UiTurn.UpdateScore(amountTurn);
            g3HandNode.g3HandMovement = this;
        }

        private void MoveHand(G3_HandType g3Hand,G3_DirectionType g3Direction)
        {
            if (g3Hand != g3HandType)
            {
                return;
            }

            if (g3HandNode.CheckCanMove(g3Direction) && amountTurn > 0)
            {
                eyesAnm.CrossFade("Move",0);
                var newNode = G3_ObjectPool.Instance.Get(G3_ObjectPool.Instance.body).GetComponent<G3_BodyNode>();
                newNode.transform.position = g3HandNode.transform.position;
                newNode.SetupSprite(g3HandNode.GetDirection(),g3Direction);
                newNode.SetDirection(g3HandNode.GetDirection());
                nodes.Add(newNode);
                
                g3HandNode.transform.up = (Vector2)newNode.GetVectorByOrder(g3Direction);
                g3HandNode.transform.localEulerAngles = new Vector3(0, 0, g3HandNode.transform.localEulerAngles.z);
                g3HandNode.transform.position += (Vector3)newNode.GetVectorByOrder(g3Direction);
                g3HandNode.SetDirection(g3Direction);

                if (g3HandNode.boxCtrl != null)
                    g3HandNode.boxCtrl.transform.position += (Vector3)newNode.GetVectorByOrder(g3Direction);

                amountTurn -= 1;
                g3UiTurn.UpdateScore(amountTurn);
            }
            else if(g3HandNode.GetBodyNode() != null 
                    && g3HandNode.GetBodyNode() == nodes[^1]) 
            {
                var lastNode = nodes[^1];
                g3HandNode.transform.up = lastNode.GetVectorByOrder(lastNode.GetDirection());
                g3HandNode.transform.localEulerAngles = new Vector3(0, 0, g3HandNode.transform.localEulerAngles.z);
                g3HandNode.transform.position = lastNode.transform.position;
                g3HandNode.SetDirection(lastNode.GetDirection());
                
                nodes.Remove(lastNode);
                G3_ObjectPool.Instance.Return(lastNode.gameObject, true);
                
                amountTurn += 1;
                g3UiTurn.UpdateScore(amountTurn);
            }
        }

        private void CheckHandCut(G3_BaseNode cutNode)
        {
            if (_isCutting)
                return;
            
            if (nodes.Contains(cutNode))
            {
                var handDie = G3_ObjectPool.Instance.Get(G3_ObjectPool.Instance.handdie);
                handDie.transform.position=g3HandNode.transform.position;
                handDie.transform.up = -g3HandNode.transform.up;
                CutHand();
            }
            else if(cutNode == g3HandNode)
            {
                CutHand();
            }
        }

        public void CutHand()
        {
            _isCutting = true;
            g3HandNode.gameObject.SetActive(false);
            eyesAnm.CrossFade("Cut",0);
            StartCoroutine(CutNode());
        }

        IEnumerator CutNode()
        {
            while (nodes.Count>0)
            {
                var lastNode = nodes[^1];
                
                g3HandNode.transform.up = lastNode.GetVectorByOrder(lastNode.GetDirection());
                g3HandNode.transform.localEulerAngles = new Vector3(0, 0, g3HandNode.transform.localEulerAngles.z);
                g3HandNode.transform.position = lastNode.transform.position;
                g3HandNode.SetDirection(lastNode.GetDirection());
                G3_ObjectPool.Instance.Get(G3_ObjectPool.Instance.smoke).transform.position = lastNode.transform.position;
                
                nodes.Remove(lastNode);
                G3_ObjectPool.Instance.Return(lastNode.gameObject, true);
                
                amountTurn += 1;
                g3UiTurn.UpdateScore(amountTurn);
                yield return new WaitForSeconds(0.15f);
            }
            
            _isCutting = false;
            g3HandNode.gameObject.SetActive(true);
            eyesAnm.CrossFade("Idle",0);
        }
    }
}
