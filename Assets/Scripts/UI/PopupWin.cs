using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopupWin : BasePopup
    {
        [SerializeField] private Button btnReplay, btnHome, btnNext;

        private void Awake()
        {
            btnReplay.onClick.AddListener(() =>
            {
                HideImmediately(true);
                MainManager.Instance.LoadGame();
            });

            btnHome.onClick.AddListener(() =>
            {
                HideImmediately(true);
                PopupCtrl.Instance.GetPopupByType<PopupHome>().ShowImmediately(false);
            });
        }
    }
}
