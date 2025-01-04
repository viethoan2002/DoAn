using System;
using UnityEngine;

namespace Gameplay
{
    public class EffectReturnPool : MonoBehaviour
    {
        
        private void OnParticleSystemStopped()
        {
            G2_ObjectPool.Instance.Return(gameObject,true);
        }
    }
}
