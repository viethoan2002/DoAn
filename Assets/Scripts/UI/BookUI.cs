using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BookUI : SingletonDontDestroy<BookUI>
    {
        [SerializeField] private List<ActionButton> levelButtons = new List<ActionButton>();

        private void Awake()
        {
            base.Awake();
            for (int i = 0; i < levelButtons.Count; i++)
            {
                int index = i;
                levelButtons[index].AddListener(() =>
                {
                    MiniGameManager.Instance.LoadGame(index);
                });
            }
        }
    }
}
