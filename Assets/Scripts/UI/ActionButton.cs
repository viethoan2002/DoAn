using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private Button btn;

        private void Awake()
        {
            btn = GetComponent<Button>();
        }

        public void AddListener(Action action)
        {
            btn.onClick.AddListener(() =>
            {
                if(CheckCondition())
                    action?.Invoke();
            });
        }

        protected virtual bool CheckCondition()
        {
            return true;
        }
    }
}
