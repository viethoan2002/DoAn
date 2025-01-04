using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game_01.Game_1.Scripts.UI
{
    public class G1LevelBtn : MonoBehaviour
    {
        [SerializeField] private Button btn;

        public void AddListener(Action action)
        {
            btn.onClick.AddListener(() =>
            {
                action?.Invoke();
            });
        }

        public void SetLock(bool en)
        {

        }
    }
}
