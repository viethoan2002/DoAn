using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopupLevel : BasePopup
    {
        [SerializeField] private List<LevelContent> levelContents = new List<LevelContent>();

        public override void ShowImmediately(bool showImmediately, Action actionComplete = null)
        {
            base.ShowImmediately(showImmediately, actionComplete);
            UpdateLockLevel();
        }

        public void UpdateLockLevel()
        {
            foreach (var p in levelContents)
            {
                p.UpdateLevelButtons();
            }
        }

        public void UpdateHighlightLevel(int indexGame, int indexLevel)
        {
            foreach (var p in levelContents)
            {
                if (p.GetIndexGame() == indexGame)
                {
                    p.ShowHighLightByIndex(indexLevel);
                }
            }
        }
    }
}
