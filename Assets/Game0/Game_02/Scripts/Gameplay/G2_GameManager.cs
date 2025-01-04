using System;
using _Camera;
using Card;
using DG.Tweening;
using Game0.Game_02.Scripts.UI;
using Gameplay;
using PathFinding;
using UI;
using UnityEditor.Rendering;
using UnityEngine;

namespace Game0.Game_02.Scripts.Gameplay
{
    public class G2_GameManager : MonoBehaviour
    {
        public static G2_GameManager Instance;
        private bool _isComplete = false;

        private void OnEnable()
        {
            PopupPause.ReStartLevel += Restart;
        }

        private void OnDisable()
        {
            PopupPause.ReStartLevel -= Restart;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            CameraManager.Instance.CameraZoomIn();
            Invoke(nameof(LoadGame),1f);
        }

        private void Restart()
        {
            CircleOutline.Instance.ScaleIn(() =>
            {
                ResetLevel();
                Start();
                CircleOutline.Instance.ScaleOut();
            });
        }

        private void LoadGame()
        {
            InteractCardManager.Instance.ReturnAllInteractCard();
            G2_SpawnCard.Instance.SetupSpawnCard();
            InteractCardManager.Instance.FillCardInBoard();
            G2_BoardCtrl.Instance.SetupDefaultBoard();
        }

        private void ResetLevel()
        {
            InteractCardManager.Instance.ReturnAllInteractCard();
            G2_SpawnCard.Instance.SetupSpawnCard();
            G2_BoardCtrl.Instance.SetupDefaultBoard();
            G2_PopupCtrl.Instance.ActiveButtonRevert(false);
            G2_PopupCtrl.Instance.ActiveButtonUse(false);
        }

        public void GameWin()
        {
            if (!_isComplete)
            {
                _isComplete = true;
                DOVirtual.DelayedCall(2, () =>
                {
                });
            }
        }

        public void GameLose()
        {
            if (!_isComplete)
            {
                _isComplete = true;
                DOVirtual.DelayedCall(1, () =>
                {
                });
            }
        }
    }
}
