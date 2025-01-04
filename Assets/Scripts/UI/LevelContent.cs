using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LevelContent : MonoBehaviour
    {
        [SerializeField] private List<LevelButton> levelButtons = new List<LevelButton>();
        [SerializeField] private int indexGame;
        private void Awake()
        {
            for(int i = 0; i < levelButtons.Count; i++)
            {
                int index = i;
                levelButtons[i].AddListener(() =>
                {
                    foreach (var l in levelButtons)
                    {
                        l.SetHighlight(false);
                    }
                    
                    levelButtons[index].SetHighlight(true);
                    UserData.SetLevelPlay(indexGame,index);
                });
            }

            UpdateLevelButtons();
            ShowHighlightButton();
        }

        public int GetIndexGame()
        {
            return indexGame;
        }

        public void UpdateLevelButtons()
        {
            for (int i = 0; i < levelButtons.Count; i++)
            {
                levelButtons[i].UpdateButton(indexGame,i);
            }
        }

        private void ShowHighlightButton()
        {
            if (levelButtons.Count == 1)
            {
                levelButtons[0].SetHighlight(true);
                return;
            }
            
            for (int i = 0; i < levelButtons.Count; i++)
            {
                if (UserData.GetLevelLock(indexGame, i))
                {
                    levelButtons[i - 1].SetHighlight(true);
                    UserData.SetLevelPlay(indexGame, i - 1);
                    return;
                }
            }
        }

        public void ShowHighLightByIndex(int index)
        {
            foreach (var t in levelButtons)
            {
                t.SetHighlight(false);
            }
            
            levelButtons[index].SetHighlight(true);
            UserData.SetLevelPlay(indexGame,index);
        }
    }
}
