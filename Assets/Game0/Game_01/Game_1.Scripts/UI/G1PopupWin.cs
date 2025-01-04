using Game_01.Game_1.Scripts.Gameplay;
using Game0.Game_01.Game_1.Scripts.Gameplay;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game_01.Game_1.Scripts.UI
{
    public class G1PopupWin : G1BasePopup
    {
        [SerializeField] private Button btnReplay, btnHome,btnNext;

        private void Awake()
        {
            btnReplay.onClick.AddListener(() =>
            {
                CircleOutline.Instance.ScaleIn(() =>
                {                
                    HideImmediately(true);
                    G1GameManager.Instance.LoadGame(G1GameManager.Instance.indexCurrentLevel);
                    CircleOutline.Instance.ScaleOut();
                });
            });

            btnHome.onClick.AddListener(() =>
            {
                CircleOutline.Instance.ScaleIn(() =>
                {                
                    HideImmediately(true);
                    G1PopupCtrl.Instance.GetPopupByType<G1PopupHome>().ShowImmediately(false);
                    CircleOutline.Instance.ScaleOut();
                });
            });
            
            btnNext.onClick.AddListener(() =>
            {
                CircleOutline.Instance.ScaleIn(() =>
                {                
                    HideImmediately(true);
                    G1GameManager.Instance.LoadGame(G1GameManager.Instance.indexCurrentLevel + 1);
                    CircleOutline.Instance.ScaleOut();
                });
            });
        }
    }
}
