using System;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopupPause : BasePopup
    {
        [SerializeField] private Button btnReStart, btnResume, btnExit;
        public static Action ReStartLevel;

        private void Awake()
        {
            btnResume.onClick.AddListener(() =>
                {
                    HideImmediately(false, () =>
                    {
                        PopupCtrl.Instance.GetPopupByType<PopupGameplay>().ShowImmediately(true);
                    });
                }
            );
            btnReStart.onClick.AddListener(() =>
            {
                HideImmediately(true);
                PopupCtrl.Instance.GetPopupByType<PopupGameplay>().ShowImmediately(true);
                ReStartLevel?.Invoke();
            });
            
            btnExit.onClick.AddListener(() =>
            {
                HideImmediately(true);
                MiniGameManager.Instance.BackToMainMenu();
            });
        }
    }
}
