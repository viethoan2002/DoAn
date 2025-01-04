using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Bin
{
    public class G4_BinItemUI : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public void ActiveIcon(bool en)
        {
            icon.enabled = en;
        }
    }
}
