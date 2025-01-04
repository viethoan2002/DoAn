using Gameplay.Map;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Player
{
    public class G4_PlayerCtrl : MonoBehaviour
    {
        public static  G4_PlayerCtrl Instance;
        [FormerlySerializedAs("playerMovement")] public G4_PlayerMovement g4PlayerMovement;
        [FormerlySerializedAs("playerStats")] public G4_PlayerStats g4PlayerStats;

        private void Awake()
        {
            Instance = this;
            g4PlayerMovement = GetComponent<G4_PlayerMovement>();
        }

        public void ResetPlayer()
        {
            g4PlayerMovement.ResetMovement();
        }

        public void UpdateBin(int amount)
        {
            g4PlayerStats.SetupBin(amount);
        }
        
        public void Test()
        {
            int x, y;
            G4_Grid.Instance.GetXY(transform.position, out x, out y);
            Debug.Log(x + ", " + y);
        }
    }
}
