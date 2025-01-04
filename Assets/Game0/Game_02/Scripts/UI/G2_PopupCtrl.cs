using System;
using _Camera;
using Card;
using UnityEngine;
using UnityEngine.UI;

namespace Game0.Game_02.Scripts.UI
{
    public class G2_PopupCtrl : MonoBehaviour
    {
       public static G2_PopupCtrl Instance;
       [SerializeField] private Button btnUse,btnRevert,btnThrow;
       [SerializeField] private GameObject centerRight;

       private void Awake()
       {
           Instance = this;
           GetComponent<Canvas>().worldCamera = CameraManager.Instance.GetUICamera();
           
           btnUse.onClick.AddListener(() =>
           {
               InteractCardManager.Instance.UseCurrentCard();
               ActiveButtonUse(false);
           });
            
           btnRevert.onClick.AddListener(() =>
           {
               InteractCardManager.Instance.UnSelectSpell();
               ActiveButtonRevert(false);
           });
            
           btnThrow.onClick.AddListener(() =>
           {
               InteractCardManager.Instance.RemoveCard();
               ActiveButtonUse(false);
           });
       }

       public void ActiveButtonUse(bool en)
       {
           centerRight.SetActive(en);
       }

       public void ActiveButtonRevert(bool en)
       {
           btnRevert.gameObject.SetActive(en);
       }
    }
}
