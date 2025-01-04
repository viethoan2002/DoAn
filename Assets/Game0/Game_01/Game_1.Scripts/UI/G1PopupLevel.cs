using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game_01.Game_1.Scripts.UI
{
    public class G1PopupLevel : G1BasePopup
    {
        [SerializeField] private List<G1LevelBtn> levelBtns = new List<G1LevelBtn>();
        [SerializeField] private Button btnHome;

        private void Awake()
        {
            for(int i = 0; i < levelBtns.Count; i++)
            {
                int index = i;
                levelBtns[i].AddListener(() =>
                {

                });
            }

            btnHome.onClick.AddListener(() =>
            {
                HideImmediately(true);
                G1PopupCtrl.Instance.GetPopupByType<G1PopupHome>().ShowImmediately(false);
            });
        }

        public override void ShowImmediately(bool showImmediately, Action actionComplete = null)
        {
            base.ShowImmediately(showImmediately, actionComplete);
            UpdateLockLevel();
        }

        public void UpdateLockLevel()
        {

        }
    }
}
