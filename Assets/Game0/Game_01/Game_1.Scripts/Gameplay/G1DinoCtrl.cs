using System.Collections.Generic;
using DG.Tweening;
using Game_01.Game_1.Scripts.Gameplay;
using Game_01.Game_1.Scripts.Gameplay.Block;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game0.Game_01.Game_1.Scripts.Gameplay
{
    public class G1DinoCtrl : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [FormerlySerializedAs("blocks")] [SerializeField] private List<G1MiniBlock> miniBlocks = new List<G1MiniBlock>();

        public void SetupDino(Vector3 position)
        {
            transform.position = position;
            animator.CrossFade("Idle",0);
        }

        public void MoveToStation()
        {
            miniBlocks = G1GameManager.Instance.curG1LevelCtrl.GetPathToStation();
            Vector3[] path=new Vector3[miniBlocks.Count];
            for (int i = 0; i < miniBlocks.Count; i++)
                path[i] = miniBlocks[i].transform.position;
            
            gameObject.SetActive(true);
            animator.CrossFade($"Run",0);
            DOVirtual.DelayedCall(0.35f, () =>
            { 
                animator.CrossFade($"Run",0);
                transform.DOPath(path, 2f).SetSpeedBased(true).SetEase(Ease.Linear)
                    .OnComplete(()=>
                    {
                        animator.CrossFade("Idle",0);
                        G1GameManager.Instance.CheckGame(miniBlocks);
                    });
            });
        }
    }
}
