using System;

namespace Gameplay.Map
{
    public class G4_PathNode //: MonoBehaviour
    {
        private G4_Grid _g4Grid;
        
        public int X;
        public int Y;
        
        public bool IsWalkable;
        public G4_BaseNode currentNode;

        public void SetPathNode(G4_Grid g4Grid, int x, int y)
        {
            this._g4Grid = g4Grid;
            this.X = x;
            this.Y = y;
            IsWalkable = true;
        }
        
        public void SetIsWalkable(bool isWalkable) {
            this.IsWalkable = isWalkable;
        }

        public void SetCurrentNode(G4_BaseNode node)
        {
            currentNode = node;
        }
        public override string ToString() {
            return X + "," + Y;
        }
    }
}
