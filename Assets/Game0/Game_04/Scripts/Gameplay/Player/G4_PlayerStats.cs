using Gameplay.Bin;
using UnityEngine;

namespace Gameplay.Player
{
    public class G4_PlayerStats : MonoBehaviour
    {
        private int _currentBin;


        public void SetupBin(int amount)
        {
            _currentBin = amount;
            G4_BinCtrl.Instance.SetupBin(amount);
        }

        public bool HasBin()
        {
            if (_currentBin > 0)
            {
                _currentBin -= 1;
                G4_BinCtrl.Instance.UseBin();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
