using UnityEngine;

namespace Gameplay
{
    public class DinoCtrl : MonoBehaviour
    {
        private DinoMovement _movement;

        private void Awake()
        {
            _movement = GetComponent<DinoMovement>();
        }

        public DinoMovement GetDinoMovement()
        {
            return _movement;
        }
    }
}
