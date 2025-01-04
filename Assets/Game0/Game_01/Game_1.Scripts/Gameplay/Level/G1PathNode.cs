using System;
using Game_01.Game_1.Scripts.Gameplay.Block;

namespace Game_01.Game_1.Scripts.Gameplay.Level
{
    public class G1PathNode //: MonoBehaviour
    {
        private G1Grid _g1Grid;
        public int X;
        public int Y;

        public int GCost;
        public int HCost;
        public int FCost;

        public bool IsWalkable;
        public bool IsStation;

        public G1PathNode CameFromNode;
        public G1MiniBlock G1MiniBlock;

        public static Action<G1PathNode> SelectNode;

        public void SetPathNode(G1Grid g1Grid, int x, int y)
        {
            this._g1Grid = g1Grid;
            this.X = x;
            this.Y = y;
            IsWalkable = true;
        }

        public void CalculateFCost() {
            FCost = GCost + HCost;
        }

        public void SetIsWalkable(bool isWalkable) {
            this.IsWalkable = isWalkable;
        }
        
        public override string ToString() {
            return X + "," + Y;
        }
    }
}
