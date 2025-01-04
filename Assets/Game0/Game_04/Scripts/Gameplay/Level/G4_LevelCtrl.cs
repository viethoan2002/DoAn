using System.Collections.Generic;
using Gameplay.Map;
using UnityEngine;

namespace Gameplay.Level
{
    public class G4_LevelCtrl : MonoBehaviour
    {
        public Transform posPlayer;
        public List<G4_FinishNode> finishNodes = new List<G4_FinishNode>();
        public int binInLevel;

        public bool CheckLevelDone()
        {
            foreach (var node in finishNodes)
            {
                if(!node.HasBlock())
                    return false;
            }
            
            return true;
        }
    }
}
