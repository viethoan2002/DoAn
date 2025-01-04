using System;
using Raycast;
using UnityEngine;

namespace Game0.Game_01.Game_1.Scripts.Gameplay
{
    public class G1ButtonPlay : MonoBehaviour,IClickHandle
    {
        private void OnMouseDown()
        {
            G1GameManager.Instance.g1DinoCtrl.MoveToStation();
            gameObject.SetActive(false);
        }

        public void OnClickObject()
        {
            G1GameManager.Instance.g1DinoCtrl.MoveToStation();
            gameObject.SetActive(false);
        }

        public void OnDragObject()
        {
        }

        public void EndObject()
        {
        }
    }
}
