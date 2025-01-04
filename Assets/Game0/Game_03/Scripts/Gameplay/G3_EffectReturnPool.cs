using UnityEngine;

namespace Gameplay
{
    public class G3_EffectReturnPool : MonoBehaviour
    {
        private void OnParticleSystemStopped()
        {
            G3_ObjectPool.Instance.Return(gameObject,true);
        }
    }
}
