using System;
using System.Collections.Generic;
using MiniGame;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class MiniGameManager : SingletonDontDestroy<MiniGameManager>
    {
        [SerializeField] private MiniGameData gameData;
        private GameObject _currentMiniGame;
        private int _indexMiniGame;

        public void LoadGame(int indexScene)
        {
            GameData data=gameData.GameData[indexScene];
            BookUI.Instance.gameObject.SetActive(false);
            
            CircleOutline.Instance.ScaleIn(() =>
            {
                if(_currentMiniGame != null)
                    Destroy(_currentMiniGame);
                
                var handle = Addressables.InstantiateAsync(data.miniGameAsset);
                BackgroundManager.Instance.ActiveBackground(data.useBackground);
                if(data.useBackground)
                    BackgroundManager.Instance.SetupBackGroundImage(data.backgroundSprite);
                handle.Completed += x =>
                {
                    _currentMiniGame = x.Result;
                    _indexMiniGame = indexScene;
                    PopupCtrl.Instance.GetPopupByType<PopupGameplay>().UpdateNameGameTxt(data.nameGame);
                    PopupCtrl.Instance.GetPopupByType<PopupGameplay>().ShowImmediately(false);
                    CircleOutline.Instance.ScaleOut();
                };
            });
        }

        public void BackToMainMenu()
        {
            CircleOutline.Instance.ScaleIn(() =>
            {
                if (_currentMiniGame != null)
                    Destroy(_currentMiniGame);

                CircleOutline.Instance.ScaleOut(() =>
                {
                    PopupCtrl.Instance.GetPopupByType<PopupLevel>().UpdateLockLevel();
                    BackgroundManager.Instance.SetDefaultBackground();
                    BookUI.Instance.gameObject.SetActive(true);
                });
            });
        }
    }
}
