using System.Collections.Generic;
using DG.Tweening;
using Game_01.Game_1.Scripts.Gameplay.Block;
using Game_01.Game_1.Scripts.Gameplay.Level;
using Game_01.Game_1.Scripts.UI;
using Game0.Game_01.Game_1.Scripts.Gameplay.Level;
using Gameplay;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Game0.Game_01.Game_1.Scripts.Gameplay
{
    public class G1GameManager : MonoBehaviour
    {
        public static G1GameManager Instance;
        [SerializeField] private Transform posClone;
        [SerializeField] private Sprite backgroundSprite;
        
        [SerializeField] private List<AssetReference> levelAssets=new List<AssetReference>();
        [FormerlySerializedAs("g1TrainCtrl")] [FormerlySerializedAs("trainCtrl")] public G1DinoCtrl g1DinoCtrl;
        public Camera playerCamera;
        public Transform hand;
        [FormerlySerializedAs("curLevelCtrl")] public G1LevelCtrl curG1LevelCtrl;
        
        public int indexCurrentLevel = 0;
        public bool isComplete = false;

        private void Awake()
        {
            Instance = this;
            LoadGame(UserData.GetLevelPlay(0));
        }
        
        private void OnEnable()
        {
            PopupPause.ReStartLevel += Restart;
        }

        private void OnDisable()
        {
            PopupPause.ReStartLevel -= Restart;
        }
        
        private void Restart()
        {
            CircleOutline.Instance.ScaleIn(() =>
            {
                LoadGame(indexCurrentLevel);
                CircleOutline.Instance.ScaleOut();
            });
        }

        public void LoadGame(int indexLevel)
        {
            isComplete = false;
            ResetLevel();

            indexCurrentLevel = indexLevel < levelAssets.Count ? indexLevel : levelAssets.Count - 1;
            var handle = Addressables.InstantiateAsync(levelAssets[indexCurrentLevel]);
            handle.Completed += (x) =>
            {
                curG1LevelCtrl = x.Result.GetComponent<G1LevelCtrl>();
                curG1LevelCtrl.transform.SetParent(posClone);
                curG1LevelCtrl.SetupLevel();
                g1DinoCtrl.SetupDino(curG1LevelCtrl.stationStart.position);
            };
        }

        public void ResetLevel()
        {
            if(curG1LevelCtrl != null)
                Destroy(curG1LevelCtrl.gameObject);
        }

        public void CheckGame(List<G1MiniBlock> miniBlocks)
        {
            if (isComplete == false)
            {
                isComplete = true;
                if (curG1LevelCtrl.CheckLevelComplete(miniBlocks))
                {
                    UserData.SetLevelLock(0,indexCurrentLevel+1,false);
                    PopupCtrl.Instance.GetPopupByType<PopupLevel>().UpdateHighlightLevel(0, indexCurrentLevel + 1);
                    DOVirtual.DelayedCall(0.25f, () =>
                    {
                        CircleOutline.Instance.ScaleIn(() =>
                        {
                            LoadGame(indexCurrentLevel + 1);
                            CircleOutline.Instance.ScaleOut();
                        });
                    });
                }
                else
                {
                    DOVirtual.DelayedCall(0.25f, () =>
                    {
                        LoadGame(indexCurrentLevel);
                    });
                }
            }
        }
    }
}
