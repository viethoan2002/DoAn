using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game_01.Game_1.Scripts.UI
{
    public class G1PopupGuid : G1BasePopup
    {
        [SerializeField] private Button btnExit;

        private void Awake()
        {
            btnExit.onClick.AddListener(() =>
            {
                HideImmediately(true);
                G1PopupCtrl.Instance.GetPopupByType<G1PopupHome>().ShowImmediately(false);
            });
        }
    }
}
