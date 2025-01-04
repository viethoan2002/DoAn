using System;
using UnityEngine;

namespace Gameplay
{
    public class G3_BarrierCtrl : MonoBehaviour
    {
        [SerializeField] private GameObject barrierStick;
        public static Action<G3_BaseNode> CutHand;
        public void SetBarrierTick(bool en)
        {
            barrierStick.SetActive(en);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var node = other.GetComponent<G3_BaseNode>();
            if (node != null)
            {
                CutHand?.Invoke(node);
            }
            else if(other.GetComponent<BoxCtrl>()!= null)
            {
                var effect = G3_ObjectPool.Instance.Get(G3_ObjectPool.Instance.smoke);
                effect.transform.position = other.transform.position;
                Destroy(other.gameObject);
            }
        }
    }
}
