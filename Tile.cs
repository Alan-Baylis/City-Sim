using System;
using System.Linq;
using System.Collections.Generic;
namespace SquareTime {
    enum TileType {
        EMPTY,
        OCEAN,
        WATER,
        GRASS,
        HILL,
        SAND,
        MOUNTAIN,
        HIGHMOUNTAIN
    }
    class Tile {
        public int X { get; private set; }
        public int Y { get; private set; }
        public double Height;
        public TileType Type {
            get
            {
                return grid.heightToType(Height);
            }
        }
        public Dictionary<int,Tile> DirectNeighbours;//might get rid of this for perfomance reasons????? need to think about it?
        public Building AttachedBuilding {
            get; private set;
        }
        private CityGrid grid;
        private PathfindingNode node;
        Action<Tile> directNeighbourCBAction;
        public List<Tile> Neighbours { get; private set; }
        public Tile(int x,int y,double height,CityGrid grid) {
            this.X = x;
            this.Y = y;
            this.grid = grid;
            this.Height = height;
            node = new PathfindingNode(X, Y, this.GetType().ToString(), findHScore, findGScoreInc);
        }
        public void initalizeNeighbours() {
            /*
            With direct neighbours, 0 refers to left,1 refers to right, 2 to top and 3 to bottom
            */
            Neighbours = new List<Tile>();
            DirectNeighbours = new Dictionary<int, Tile>();
            if (X - 1 >= 0) {
                Neighbours.Add(grid.getTile(X - 1, Y));
                grid.getTile(X - 1, Y).addNeighbourCallback(onNeighbourBuilt);
                DirectNeighbours.Add(0,grid.getTile(X - 1, Y));
            }
            if (X + 1 < grid.Size.X) {
                Neighbours.Add(grid.getTile(X + 1, Y));
                grid.getTile(X + 1, Y).addNeighbourCallback(onNeighbourBuilt);
                DirectNeighbours.Add(1,grid.getTile(X + 1, Y));
            }
            if (Y - 1 >= 0) {
                Neighbours.Add(grid.getTile(X, Y - 1));
                grid.getTile(X, Y-1).addNeighbourCallback(onNeighbourBuilt);
                DirectNeighbours.Add(2,grid.getTile(X, Y - 1));
            }
            if(Y+1 < grid.Size.Y) {
                Neighbours.Add(grid.getTile(X, Y + 1));
                grid.getTile(X,Y+1).addNeighbourCallback(onNeighbourBuilt);
                DirectNeighbours.Add(3,grid.getTile(X, Y + 1));
            }
            if (Y + 1 < grid.Size.Y && X+1 <grid.Size.X) Neighbours.Add(grid.getTile(X+1, Y + 1));
            if (Y + 1 < grid.Size.Y && X -1 >=0) Neighbours.Add(grid.getTile(X-1, Y + 1));
            if (Y - 1 >= 0 && X+1 < grid.Size.X  ) Neighbours.Add(grid.getTile(X+1, Y - 1));
            if (Y - 1 >= 0 && X-1 >= 0) Neighbours.Add(grid.getTile(X-1, Y - 1));
        }
        public int Bulldoze() {
            if (AttachedBuilding == null) {
                return 0;
            }
            int cost = AttachedBuilding.Cost;
            Building tb = AttachedBuilding;           
            AttachedBuilding = null;
            foreach(KeyValuePair<int,Tile> p in DirectNeighbours) {
                if (p.Value.AttachedBuilding != tb) continue;
                p.Value.Bulldoze();
            }
            directNeighbourCBAction(this);
            return cost;
        }
        public void onNeighbourBuilt (Tile neighbour) {
            if (AttachedBuilding != null) {
                  AttachedBuilding.calculateVarient();
            }
        }
        public void addNeighbourCallback(Action<Tile> tileCallback) {
            directNeighbourCBAction += tileCallback;
        }
        public bool AttachBuilding(Building building) {
            if (checkBuilding(building)) {
                this.AttachedBuilding = building;
                building.calculateVarient();
                directNeighbourCBAction(this);
                return true;
            }
            else return false;
        }
        public bool checkBuilding(Building building) {
            if (AttachedBuilding != null) {
                return false;
            }
            if (!building.AllowedTypes.Contains(Type)) return false;
            return true;

        }
        private float findGScoreInc(PathfindingNode to) {
            return 1.0f;
        }

        private float findHScore(PathfindingNode to) {//use exact distance
            float dx = to.X - this.X;
            float dy = to.Y - this.Y;
            return (float)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
        }
        public PathfindingNode getNode(string id) {
            if (id == this.GetType().ToString()) return node;
            if (id == this.AttachedBuilding.GetType().ToString()) return this.AttachedBuilding.Node;
            else return null;
        }
    }
}


