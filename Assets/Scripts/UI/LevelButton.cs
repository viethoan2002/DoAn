using UnityEngine;

namespace UI
{
    public class LevelButton : ActionButton
    {
        [SerializeField] private GameObject lockObj;
        [SerializeField] private GameObject hightLight;

        private int _indexGame, _indexLevel;

        protected override bool CheckCondition()
        {
            if (UserData.GetLevelLock(_indexGame, _indexLevel))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void UpdateButton(int indexGame,int indexLevel)
        {
            _indexGame = indexGame;
            _indexLevel = indexLevel;
            lockObj.gameObject.SetActive(UserData.GetLevelLock(indexGame, indexLevel));
        }

        public void SetHighlight(bool en)
        {
            hightLight.SetActive(en);
        }
    }
}
