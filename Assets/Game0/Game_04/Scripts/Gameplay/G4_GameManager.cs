using System;
using System.Collections.Generic;
using _Camera;
using DG.Tweening;
using Gameplay.Level;
using Gameplay.Player;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class G4_GameManager : MonoBehaviour
    {
        public static G4_GameManager Instance;
        [SerializeField] private List<GameObject> prefabsLevel = new List<GameObject>();
        [FormerlySerializedAs("currentLevelCtrl")] [SerializeField] private G4_LevelCtrl currentG4LevelCtrl;
        public int indexCurrentLevel;

        private bool _isGameOver;

        private void OnEnable()
        {
            PopupPause.ReStartLevel += ResetLevel;
        }

        private void OnDisable()
        {
            PopupPause.ReStartLevel -= ResetLevel;
        }

        private void Awake()
        {
            Instance = this;
            CameraManager.Instance.CameraZoomOut();
            DOVirtual.DelayedCall(0.05f, () =>
            {
                LoadGame(UserData.GetLevelPlay(3));
            });
        }

        public void LoadGame(int indexLevel)
        {
            if(currentG4LevelCtrl != null)
                Destroy(currentG4LevelCtrl.gameObject);

            _isGameOver = false;
            
            indexCurrentLevel = indexLevel < prefabsLevel.Count ? indexLevel : prefabsLevel.Count - 1;
            currentG4LevelCtrl = Instantiate(prefabsLevel[indexCurrentLevel], transform).GetComponent<G4_LevelCtrl>();
            
            G4_PlayerCtrl.Instance.transform.position = currentG4LevelCtrl.posPlayer.position;
            G4_PlayerCtrl.Instance.ResetPlayer();
            G4_PlayerCtrl.Instance.UpdateBin(currentG4LevelCtrl.binInLevel);
            G4_UICtrl.Instance.ResetButtonCtrl();
        }

        public void ResetLevel()
        {
            CircleOutline.Instance.ScaleIn(() => { LoadGame(indexCurrentLevel); });
        }

        public void CheckGame()
        {
            if (currentG4LevelCtrl.CheckLevelDone() && !_isGameOver)
            {
                _isGameOver = true;
                G4_PlayerCtrl.Instance.g4PlayerMovement.SetCanMove(false);
                UserData.SetLevelLock(3,indexCurrentLevel+1,false);
                PopupCtrl.Instance.GetPopupByType<PopupLevel>().UpdateHighlightLevel(3, indexCurrentLevel + 1);
                CircleOutline.Instance.ScaleIn( () =>
                {
                    LoadGame(indexCurrentLevel + 1);
                });
            }
        }
    }
}
