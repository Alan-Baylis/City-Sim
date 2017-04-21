using System;

namespace SquareTime {
    public class PathfindingNode: IHeapItem<PathfindingNode> {
        public int X, Y;
        public string Name { get; protected set; }
        public float gScore;
        public float hScore;
        public float fScore
        {
            get
            {
                return gScore + hScore;
            }           
        }
        public PathfindingNode(int X,int Y,string name,Func<PathfindingNode,float> findHScore,Func<PathfindingNode,float> findGScoreInc) {
            this.X = X;
            this.Y = Y;
            this.Name = name;
            this.findHScore = findHScore;
            this.findGScoreInclusive = findGScoreInc;
        }
        public PathfindingNode parent;
        private int heapIndex;
        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }

            set
            {
                heapIndex = value;
            }
        }
        public int CompareTo(PathfindingNode other) {
            int compare = fScore.CompareTo(other.fScore);
            if (compare == 0) {
                compare = hScore.CompareTo(other.hScore);
            }
            return -compare;
        }
        public Func<PathfindingNode, float> findHScore;
        public Func<PathfindingNode, float> findGScoreInclusive;
    }
}