using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareTime {
    class Road:Building{
        private PathfindingNode roadNode;
        public override PathfindingNode Node
        {
            get
            {
                return roadNode;
            }
        }
        protected float speedLimit;
        public void initNode(int x,int y){
                roadNode = new PathfindingNode(x, y, this.GetType().ToString(), findHScore, findGScoreInc);
        }
        public static Road createProtoype(string name, int cost,float speedLimit,TileType[] allowedType) {
            //all road tiles are 1x1 (for now) 
            Road r = new Road();
            r.Name = name;
            r.Cost = cost;
            r.width = 1;
            r.height = 1;
            r.speedLimit = speedLimit;
            r.AllowedTypes = allowedType;
            return r;
        }
        public static Road placeRoad(Road prototype, Tile rootTile) {
            Road r = new Road();
            r.Name = prototype.Name;
            r.Cost = prototype.Cost;
            r.height = prototype.height;
            r.width = prototype.width;
            r.speedLimit = prototype.speedLimit;
            r.AllowedTypes = prototype.AllowedTypes;
            r.RootTile = rootTile;
            r.initNode(rootTile.X, rootTile.Y);
            if (!r.RootTile.AttachBuilding(r)) {
                return null;
            }
            return r;
        }
        private float findGScoreInc(PathfindingNode to) {
            return (1 / speedLimit);
        }

        private float findHScore(PathfindingNode to) {//use exact distance
            float dx = to.X - this.RootTile.X;
            float dy = to.Y - this.RootTile.Y;
            return (float) Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
        }

        public void onPlaced(Tile tile) {

        }
    }
}
