using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopupGameplay : BasePopup
    {
        [SerializeField] private Button btnReset, btnHome;
        [SerializeField] private Button btnPause;
        [SerializeField] private Text nameGameTxt;

        private void Awake()
        {
            btnReset.onClick.AddListener(() =>
            {
                MainManager.Instance.LoadGame();
            });

            btnHome.onClick.AddListener(() =>
            {
                MainManager.Instance.ResetLevel();
                HideImmediately(true);
                PopupCtrl.Instance.GetPopupByType<PopupHome>().ShowImmediately(false);
            });
            
            btnPause.onClick.AddListener(() =>
            {
                HideImmediately(true);
                PopupCtrl.Instance.GetPopupByType<PopupPause>().ShowImmediately(false);
            });
        }

        public void UpdateNameGameTxt(string newName)
        {
            nameGameTxt.text = newName;
        }
    }
}
