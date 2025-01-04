using DG.Tweening;
using Game_01.Game_1.Scripts.Gameplay;
using Game0.Game_01.Game_1.Scripts.Gameplay;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game_01.Game_1.Scripts.UI
{
    public class G1PopupHome : G1BasePopup
    {
        [SerializeField] private Button btnPlay, btnGuid;

        private void Awake()
        {
            btnPlay.onClick.AddListener(() =>
            {
                CircleOutline.Instance.ScaleIn(() =>
                {
                    HideImmediately(true);
                    G1GameManager.Instance.LoadGame(0);
                    G1PopupCtrl.Instance.GetPopupByType<G1PopupGameplay>().ShowImmediately(false);
                    DOVirtual.DelayedCall(0.25f, () =>
                    {
                        CircleOutline.Instance.ScaleOut();
                    });
                });

            });

            btnGuid.onClick.AddListener(() =>
            {
                HideImmediately(true);
                G1PopupCtrl.Instance.GetPopupByType<G1PopupGuid>().ShowImmediately(false);
            });
        }
    }
}
