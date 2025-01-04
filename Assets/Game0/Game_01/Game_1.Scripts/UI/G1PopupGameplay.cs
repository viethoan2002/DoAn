using DG.Tweening;
using Game_01.Game_1.Scripts.Gameplay;
using Game0.Game_01.Game_1.Scripts.Gameplay;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game_01.Game_1.Scripts.UI
{
    public class G1PopupGameplay : G1BasePopup
    {
        [SerializeField] private Button btnReset, btnHome;
        [SerializeField] private Button btnAccelerator;

        private void Awake()
        {
            btnReset.onClick.AddListener(() =>
            {
                G1GameManager.Instance.LoadGame(G1GameManager.Instance.indexCurrentLevel);
            });

            btnHome.onClick.AddListener(() =>
            {
                G1GameManager.Instance.ResetLevel();
                HideImmediately(true);
                G1PopupCtrl.Instance.GetPopupByType<G1PopupHome>().ShowImmediately(false);
            });
            
            btnAccelerator.onClick.AddListener(HideAccelerator);
        }
        
        public void ShowAccelerator()
        {
            btnAccelerator.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(245, 100, 0);
            btnAccelerator.gameObject.SetActive(true);
            if (G1GameManager.Instance.isComplete)
                return;
            
            btnAccelerator.enabled = true;
            btnAccelerator.GetComponent<RectTransform>().DOAnchorPos3DY(-220, 2f).SetEase(Ease.OutBounce);
        }

        public void DeActiveAccelerator()
        {
            btnAccelerator.gameObject.SetActive(false); 
        }

        private void HideAccelerator()
        {
            btnAccelerator.enabled = false;
            btnAccelerator.GetComponent<RectTransform>().DOAnchorPos3DY(100, 1f).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    G1GameManager.Instance.g1DinoCtrl.MoveToStation();
                });
        }
    }
}
