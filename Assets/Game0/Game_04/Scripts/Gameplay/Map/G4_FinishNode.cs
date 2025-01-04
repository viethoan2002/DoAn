using System;
using UnityEngine;

namespace Gameplay.Map
{
    public class G4_FinishNode : G4_BaseNode
    {
        private bool _hasBlock;
        [SerializeField] private bool isMainFinished;

        public bool HasBlock()
        {
            return _hasBlock;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isMainFinished)
            {
                if(other.CompareTag("Player"))
                    _hasBlock = true;
            }
            else
            {
                if (other.GetComponent<G4_ChickenNode>() != null)
                {
                    other.GetComponent<G4_ChickenNode>().SheepDance();
                }
                _hasBlock = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (isMainFinished)
            {
                if(other.CompareTag("Player"))
                    _hasBlock = false;
            }
            else
                _hasBlock = false;
        }
    }
}
