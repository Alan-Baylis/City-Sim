using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace SquareTime {
    class CityGrid {
        private Tile[,] grid;
        private float tileSize; //Todo change this 
        public Vector2 Size { get; private set; }
        float tideLevel = 0.0f;
        float timer = 0.0f;
        public CityGrid(int x, int y, float tileSize) {
            this.Size = new Vector2(x, y);
            this.grid = new Tile[x, y];
            this.tileSize = tileSize;
            generateMap(4);
        }
        struct Diamond {
            public Point top, left, right, bottom;
            public Diamond(Point top, Point left, Point right, Point bottom) {
                this.top = top;
                this.left = left;
                this.right = right;
                this.bottom = bottom;
            }
        }
        struct Square {
            public Point tleft, tright, bleft, bright;
            public Square(Point tleft, Point tright, Point bright, Point bleft) {
                this.tleft = tleft;
                this.tright = tright;
                this.bright = bright;
                this.bleft = bleft;
            }
        }
        public void generateMap(float smoothness, List<List<double>> heightMap, float maxHeight = 1.0f) {
            Random r = new Random();
            List<Square> sq = new List<Square>();
            sq.Add(new Square(new Point(0, 0), new Point((int)Size.X - 1, 0), new Point((int)Size.X - 1, (int)Size.X - 1), new Point(0, (int)Size.X - 1)));
            float iter = 1;
            while ((sq[0].bright.X - sq[0].bleft.X) > 1) {
                List<Diamond> dias = new List<Diamond>();
                foreach (Square s in sq) { //diamond step
                    //sort out the heightmap
                    Point mid = new Point((int)(((float)s.tleft.X + (float)s.tright.X) / 2f), (int)(((float)s.bright.Y + (float)s.tright.Y) / 2f));
                    double tLeft = heightMap[s.tleft.X][s.tleft.Y];
                    double tRight = heightMap[s.tright.X][s.tright.Y];
                    double bLeft = heightMap[s.bleft.X][s.bleft.Y];
                    double bRight = heightMap[s.bright.X][s.bright.Y];
                    double newVal = (tLeft + tRight + bLeft + bRight) / 4 + (randDouble(-maxHeight, maxHeight, r)) / (smoothness * iter);
                    heightMap[(int)mid.X][(int)mid.Y] = newVal;
                    dias.AddRange(getDiamond(s));
                }
                foreach (Diamond d in dias) { //square step
                    //FIXME
                    int divisor = 0;//COULD GO WRONG
                    double summation = 0;
                    if (inRange(d.top.X, heightMap.Count) && inRange(d.top.Y, heightMap.Count)) {
                        divisor++;
                        summation += heightMap[d.top.X][d.top.Y];
                    }
                    if (inRange(d.left.X, heightMap.Count) && inRange(d.left.Y, heightMap.Count)) {
                        divisor++;
                        summation += heightMap[d.left.X][d.left.Y];
                    }
                    if (inRange(d.right.X, heightMap.Count) && inRange(d.right.Y, heightMap.Count)) {
                        summation += heightMap[d.right.X][d.right.Y];
                        divisor++;
                    }
                    if (inRange(d.bottom.X, heightMap.Count) && inRange(d.bottom.Y, heightMap.Count)) {
                        divisor++;
                        summation += heightMap[d.bottom.X][d.bottom.Y];
                    }
                    double newVal = summation / divisor + (randDouble(-maxHeight, maxHeight, r)) / (smoothness * iter);
                    Point mid = new Point((int)((d.left.X + d.right.X) / 2f), (int)((d.top.Y + d.bottom.Y) / 2f));
                    heightMap[mid.X][mid.Y] = newVal;
                }
                //get squares
                iter++;
                sq = getSquares(sq, (int)iter).ToList();
            }
            double max = double.MinValue;
            double min = double.MaxValue;
            foreach (List<double> hlist in heightMap) {
                foreach (double height in hlist) {
                    if (height > max) max = height;
                    if (height < min) min = height;
                }
            }
            for (int x = 0; x < Size.X; x++) {
                for (int y = 0; y < Size.Y; y++) {
                    grid[x, y] = new Tile(x, y, (heightMap[x][y] - min) / (max - min), this);
                }
            }
            smoothNonDirect();
        }
        public void generateMap(float smoothness, float maxHeight = 1.0f) {
            List<List<double>> heightMap = new List<List<double>>();
            //Initalize heightmap
            for (int i = 0; i < Size.X; i++) {
                heightMap.Add(new List<double>());
                for (int j = 0; j < Size.X; j++) {
                    heightMap[i].Add(0);
                }
            }
            generateMap(smoothness, heightMap, maxHeight);
        }

        public void smoothNonDirect() {
            foreach (Tile tile in grid) {
                tile.initalizeNeighbours();
                TileType t = tile.Type;
                if (tile.Neighbours.Where((x) => x.Type == t).ToList().Count <= 3) {
                    if (tile.Neighbours.Count <= 3) {
                        continue;
                    }
                    tile.Height = tile.Neighbours.Where((x) => x.Type != t).Average(x => x.Height);
                }
            }
        }
        /* WORK IN PROGRESS
        public void extendCity(int dir) {//0 for left 1 for up 2 for right 3 for down
            List<List<double>> heightmap = new List<List<double>>();
            switch (dir) {
                case 0:
                    
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }

        }*/
        public float TypeToHeight(TileType type) {//aproxmation
            switch (type) {
                case TileType.GRASS:
                    return 0.6f;
                case TileType.SAND:
                    return 0.59f;
                case TileType.WATER:
                    return 0.5f;
                case TileType.OCEAN:
                    return 0.0f;
                case TileType.HILL:
                    return 0.75f;
                case TileType.MOUNTAIN:
                    return 0.8f;
                case TileType.HIGHMOUNTAIN:
                    return 0.95f;
                default:
                    return 0.0f;
            }
        }
        public TileType heightToType(double height) {
            if (height < 0.45f) return TileType.OCEAN;
            if (height >= 0.45f && height < 0.55f) return TileType.WATER;
            if (height >= 0.55f && height < 0.60f) return TileType.SAND;
            if (height >= 0.60f && height < 0.75f) return TileType.GRASS;
            if (height >= 0.75f && height < 0.85f) return TileType.HILL;
            if (height >= 0.85f && height < 0.95f) return TileType.MOUNTAIN;
            if (height >= 0.95f) return TileType.HIGHMOUNTAIN;
            return TileType.EMPTY;
        }
        public Tile getTile(int x, int y) {
            if (x < 0 || x >= Size.X || y < 0 || y >= Size.Y) {
                return null;
            }
            return grid[x, y];
        }
        public Tile getTile(Vector2 v) {
            return getTile((int)v.X, (int)v.Y);
        }
        private bool inRange(int pos, int max) { //come get me oracle
            if (pos >= max) return false;
            if (pos < 0) return false;
            return true;
        }
        private double randDouble(double min, double max, Random rand) {
            return rand.NextDouble() * (max - min) + min;
        }
        private Square[] getSquares(List<Square> oldSquares, int iter) {

            Square[] squares = new Square[(int)Math.Pow(4, iter - 1)];
            float length = (oldSquares[0].bright.X - oldSquares[0].bleft.X) / 2;
            float tLength = (float)Math.Sqrt(squares.Length);
            for (int s = 0; s < squares.Length; s++) {
                squares[s] = new Square(new Point((int)(length * (s % tLength)), (int)((int)(s / tLength) * length)),
                                        new Point((int)(length * (s % tLength) + length), (int)((int)(s / tLength) * length)),
                                        new Point((int)(length * (s % tLength) + length), (int)((int)(s / tLength) * length + length)),
                                        new Point((int)(length * (s % tLength)), (int)((int)(s / tLength) * length + length)));
            }
            return squares;
        }
        private Diamond[] getDiamond(Square sq) {
            Point mid = new Point((int)(((float)sq.tleft.X + (float)sq.tright.X) / 2f), (int)(((float)sq.bright.Y + (float)sq.tright.Y) / 2f));
            Diamond[] dias = new Diamond[4];
            dias[0] = new Diamond(sq.tleft, new Point(mid.X - (mid.X - sq.bleft.X) * 2, mid.Y), mid, sq.bleft);
            dias[1] = new Diamond(new Point(mid.X, mid.Y - 2 * (mid.Y - sq.tleft.Y)), sq.tleft, sq.tright, mid);
            dias[2] = new Diamond(sq.tright, mid, new Point(mid.X + (mid.X - sq.bleft.X) * 2, mid.Y), sq.bright);
            dias[3] = new Diamond(mid, sq.bleft, sq.bright, new Point(mid.X, mid.Y + 2 * (mid.Y - sq.tleft.Y)));
            return dias;
        }
        public void initalizeMap(TileType typeToInit) {
            for (int x = 0; x < Size.X; x++) {
                for (int y = 0; y < Size.Y; y++) {
                    grid[x, y] = new Tile(x, y, TypeToHeight(typeToInit), this);
                }
            }
        }

        public Vector2 mapToWorld(Vector2 map) {
            return new Vector2(map.X * tileSize, map.Y * tileSize);
        }
        public Vector2 worldToMap(Vector2 world) {
            return new Vector2((int)Math.Floor(world.X / tileSize), (int)Math.Floor(world.Y / tileSize));
        }
        public void Update(double timeStep) { //in mins

        }
        public void Draw(SpriteBatch batch, Camera cam) {
            //Calculate Drawing bounds
            Vector2 minPos = worldToMap(cam.screenToWorld(Vector2.Zero));
            Vector2 maxPos = worldToMap(cam.screenToWorld(new Vector2(SquareGame.viewport.Width, SquareGame.viewport.Height)));
            for (int x = (int)Math.Max(minPos.X - 1, 0); x < Math.Min(maxPos.X + 1, Size.X); x++) {
                for (int y = (int)Math.Max(minPos.Y - 1, 0); y < Math.Min(maxPos.Y + 1, Size.Y); y++) {
                    Color color;
                    if (grid[x, y].AttachedBuilding != null) {
                        if (grid[x, y].AttachedBuilding.RootTile == grid[x, y]) {
                            string key = (grid[x, y].AttachedBuilding.Name + ((grid[x, y].AttachedBuilding.Level > 0) ? "-level-" + grid[x, y].AttachedBuilding.Level : "") + ((grid[x, y].AttachedBuilding.Varient >= 0) ? "_" + grid[x, y].AttachedBuilding.Varient : "")).ToString();
                            batch.Draw(TextureBank.textureBank[ key], new Vector2(x * tileSize, y * tileSize), Color.White);
                        }
                    }
                    else {
                        switch (grid[x,y].Type) {
                            case TileType.EMPTY:
                                color = Color.Black;
                                break;
                            case TileType.GRASS:
                                color = Color.Green;
                                break;
                            case TileType.HILL:
                                color = Color.DarkGreen;
                                break;
                            case TileType.MOUNTAIN:
                                color = Color.Gray;
                                break;
                            case TileType.HIGHMOUNTAIN:
                                color = Color.White;
                                break;
                            case TileType.SAND:
                                color = Color.SandyBrown;
                                break;
                            case TileType.WATER:
                                color = Color.Blue;
                                break;
                            case TileType.OCEAN:
                                color = Color.DarkBlue;
                                break;
                            default:
                                color = Color.Black;
                                break;
                        }
                        batch.Draw(TextureBank.tileTexture, new Vector2(x * tileSize, y * tileSize), color);
                    }
                }
            }
        }
        public Tile[] findAStarPath(PathfindingNode start, PathfindingNode end) {
            if (end.Name != start.Name) return null;
            Heap<PathfindingNode> openSet = new Heap<PathfindingNode>((int)(Size.X * Size.Y));
            start.gScore = 0;
            start.hScore = start.findHScore(end);
            openSet.Add(start);
            List<PathfindingNode> closedSet = new List<PathfindingNode>();
            while (openSet.Count > 0) {
                PathfindingNode current = openSet.RemoveTop();
                if (current == end) {
                    List<Tile> finalList = new List<Tile>();
                    finalList.Add(getTile(current.X, current.Y));
                    while (current.parent != null) {
                        current = current.parent;
                        finalList.Add(getTile(current.X, current.Y));
                    }
                    finalList.Reverse();
                    return finalList.ToArray();
                }
                closedSet.Add(current);
                for (int x = current.X - 1; x <= current.X + 1; x++) {
                    for (int y = current.Y - 1; y <= current.Y + 1; y++) {
                        if ((x != current.X && y != current.Y) || x < 0 || y < 0 || x > Size.X || y > Size.Y) continue;
                        PathfindingNode n = grid[x, y].getNode(current.Name);
                        if (n == null) continue;
                        if (closedSet.Contains(n)) {
                            continue;
                        }
                        float tgScore = current.gScore + current.findGScoreInclusive(n);
                        if (!openSet.Contains(n) || tgScore < n.gScore) {
                            n.parent = current;
                            n.gScore = tgScore;
                            n.hScore = n.findHScore(end);
                            if (!openSet.Contains(n)) {
                                openSet.Add(n);
                            }
                            else {
                                openSet.UpdateItem(n);
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
