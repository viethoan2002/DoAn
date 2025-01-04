using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Bin
{
    public class G4_BinCtrl : MonoBehaviour
    {
        public static G4_BinCtrl Instance;
        [SerializeField] private List<G4_BinItemUI> bins = new List<G4_BinItemUI>();
        [SerializeField] private RectTransform rectContent;
        private int _indexBin = 0;

        private void Awake()
        {
            Instance = this;
        }

        public void SetupBin(int amount)
        {
            rectContent.anchoredPosition = new Vector2(-100, -(120 * amount) / 2);
            _indexBin = 0;
            for (int i = 0; i < amount; i++)
            {
                bins[i].ActiveIcon(true);
                bins[i].gameObject.SetActive(i < amount);
            }
        }

        public void UseBin()
        {
            bins[_indexBin].ActiveIcon(false);
            _indexBin += 1;
        }
    }
}
