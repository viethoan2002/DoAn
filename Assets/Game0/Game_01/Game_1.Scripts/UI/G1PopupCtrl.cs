using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game_01.Game_1.Scripts.UI
{
    public class G1PopupCtrl : MonoBehaviour
    {
        public static G1PopupCtrl Instance;

        [FormerlySerializedAs("_popups")] [SerializeField] private List<G1BasePopup> popups=new List<G1BasePopup>();

        private void Awake()
        {
            Instance = this;
        }

        public T GetPopupByType<T>() where T : G1BasePopup
        {
            foreach (var popup in popups)
            {
                if(popup is T basePopup)
                    return basePopup;
            }

            return null;
        }
    }
}
