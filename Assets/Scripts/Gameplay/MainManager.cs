using UnityEngine;

namespace Gameplay
{
    public class MainManager : MonoBehaviour
    {
        public static MainManager Instance;
        
        public Camera mainCamera;
        public Camera uiCamera;

        private void Awake()
        {
            Instance = this;
        }

        public void LoadGame()
        {

        }

        public void ResetLevel()
        {

        }

        public void CheckGame()
        {

        }
    }
}
