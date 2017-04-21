using System;
using Microsoft.Xna.Framework.Graphics;
namespace SquareTime {
    class Residential:Building {
        protected int maxCapacity;
        public int curretCapacity = 0;
        protected int level = 0;
        public override int Level
        {
            get
            {
                return level;
            }
        }
        protected int population = 0;
        private Residential() { }
        public static Residential createProtoype (string name,int cost, int height, int width, int capacity,TileType[] allowedTypes) {
            Residential r = new Residential();
            r.Name = name;
            r.Cost = cost;
            r.height = height;
            r.width = width;
            r.maxCapacity = capacity;
            r.AllowedTypes = allowedTypes;
            return r;
        }
        public static Residential placeResidence(Residential prototype,Tile rootTile) {
            Residential r = new Residential();
            r.varient = -1;
            r.Name = prototype.Name;
            r.Cost = prototype.Cost; 
            r.height = prototype.height;
            r.width = prototype.width;
            r.maxCapacity = prototype.maxCapacity;
            r.RootTile = rootTile;
            r.AllowedTypes = prototype.AllowedTypes;
            for (int x = 0; x < r.width; x++) {
                for (int y = 0; y < r.height; y++) {
                    Tile t = iterateTile(x, y, r);
                    if (!t.checkBuilding(r) || t.Type != rootTile.Type) {
                        return null;
                    }
                }
            }
            for(int x = 0; x < r.width; x++) {
                for (int y = 0; y < r.height; y++) {
                    Tile t = iterateTile(x, y, r);
                    t.AttachBuilding(r);
                }
            }
            return r;
        }
    }
}
