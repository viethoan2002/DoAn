using System;
using System.Collections.Generic;
using _Camera;
using DG.Tweening;
using UI;
using UnityEngine;

namespace Gameplay
{
    public class G3GameManager : MonoBehaviour
    {
        public static G3GameManager Instance;
        [SerializeField] private ParticleSystem effectWin;
        
        [SerializeField] private List<GameObject> prefLevels=new List<GameObject>();
        [field: SerializeField] public int indexCurLevel { get; private set; }
        [SerializeField] private GameObject curLevel;
    
        private bool _isEndGame = false;
        private void Awake()
        {
            Instance = this;
            CameraManager.Instance.CameraZoomOut();
            DOVirtual.DelayedCall(0.05f, () =>
            {
                LoadGame(UserData.GetLevelPlay(2));
            });
        }

        private void OnEnable()
        {
            PopupPause.ReStartLevel += ResetLevel;
        }

        private void OnDisable()
        {
            PopupPause.ReStartLevel -= ResetLevel;
        }

        private void LoadGame(int indexLevel)
        {
            if(curLevel != null)
                Destroy(curLevel);

            G3_ObjectPool.Instance.ReturnAllPool();
            indexCurLevel = indexLevel < prefLevels.Count ? indexLevel : prefLevels.Count - 1;
            curLevel=Instantiate(prefLevels[indexCurLevel],transform);
            _isEndGame = false;
        }

        public void ResetGame()
        {
            
        }

        public void ResetLevel()
        {
            CircleOutline.Instance.ScaleIn(() =>
            {
                LoadGame(indexCurLevel);
                CircleOutline.Instance.ScaleOut();
            });
        }

        public void WinGame()
        {
            if (!_isEndGame)
            {
                _isEndGame = true;
                effectWin.Play();
                UserData.SetLevelLock(2,indexCurLevel+1,false);
                PopupCtrl.Instance.GetPopupByType<PopupLevel>().UpdateHighlightLevel(2, indexCurLevel + 1);
                DOVirtual.DelayedCall(1.25f, () =>
                {
                    CircleOutline.Instance.ScaleIn(() =>
                    {
                        LoadGame(indexCurLevel+1);
                        CircleOutline.Instance.ScaleOut();
                    });
                });
            }
        }

        public void CheckGame()
        {

        }
    }
}
