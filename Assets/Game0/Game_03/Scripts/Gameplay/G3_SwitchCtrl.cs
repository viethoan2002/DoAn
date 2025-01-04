using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public class G3_SwitchCtrl : MonoBehaviour
    {
        [SerializeField] private List<G3_BarrierCtrl> barrierCtrls=new List<G3_BarrierCtrl>();
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite onSprite,offSprite;
        [SerializeField] private bool switchOn;

        public bool isTrigger;
        
        public void Switch()
        {
            switchOn = !switchOn;
            if(!isTrigger)
                spriteRenderer.sprite = switchOn ? onSprite : offSprite;
            foreach (G3_BarrierCtrl barrierCtrl in barrierCtrls)
                barrierCtrl.SetBarrierTick(switchOn);
        }

        private void Update()
        {
            if (isTrigger)
            {
                Collider2D[] cols= Physics2D.OverlapBoxAll(transform.position, new Vector2(0.3f, 0.3f), 0);
                switchOn = true;
                foreach (var col in cols)
                {
                    if (col.transform.GetComponent<BoxCtrl>() != null || col.transform.GetComponent<G3_HandNode>() != null)
                    {
                        switchOn = false;
                        break;
                    }
                }

                foreach (G3_BarrierCtrl barrierCtrl in barrierCtrls)
                    barrierCtrl.SetBarrierTick(switchOn);
            }
        }
    }
}
