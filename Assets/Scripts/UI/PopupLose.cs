using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopupLose : BasePopup
    {
        [SerializeField] private Button btnHome, btnReplay;

        private void Awake()
        {
            btnHome.onClick.AddListener(() =>
            {
                HideImmediately(true);
                PopupCtrl.Instance.GetPopupByType<PopupHome>().ShowImmediately(false);
            });

            btnReplay.onClick.AddListener(() =>
            {
                HideImmediately(true);
            });
        }
    }
}
