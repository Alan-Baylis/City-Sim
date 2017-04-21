using System;
using Microsoft.Xna.Framework.Graphics;
namespace SquareTime {
    abstract class Building {
        public string Name { get; protected set; }
        public int Cost { get; protected set; }
        protected int varient;
        public TileType[] AllowedTypes { get; protected set; }
        public virtual int Level
        {
            get
            {
                return -1;
            }
        }
        public virtual int Varient
        {
            get
            {
                return varient;
            }
            private set
            {
                varient = value;
            }
        }
        /* VARIENCE RULES - Not all variants need to be drawn
              2
            0XXX1
              3

        0 = XXX 1= X0X 2 = XXX 3 = XXX 4 = XXX 5= X0X 6 = XXX 7 = X0X 8 = X0X 9 = XXX 10= XXX 11= X0X 12= X0X 13 = XXX 14= X0X 15= X0X
            X0X    X0X     00X     X00     X0X    X0X     000     00X     X00     X00     00X     000     X00      000     00X     000
            XXX    XXX     XXX     XXX     X0X    X0X     XXX     XXX     XXX     X0X     X0X     XXX     X0X      X0X     X0X     X0X
        */
        protected int width, height;
        public virtual PathfindingNode Node { get
            {
                return null;
            }
        }
        public Tile RootTile { get; protected set; }
        public void calculateVarient() {
            if (varient == -1) return;
            if(RootTile == null) {
                return;
            }
            if (!neighbourContainsMe(0) && !neighbourContainsMe(1) && !neighbourContainsMe(2) && !neighbourContainsMe(3)) varient = 0;
            if (!neighbourContainsMe(0) && !neighbourContainsMe(1) &&  neighbourContainsMe(2) && !neighbourContainsMe(3)) varient = 1;
            if (neighbourContainsMe(0) && !neighbourContainsMe(1) && !neighbourContainsMe(2) && !neighbourContainsMe(3)) varient = 2;
            if (!neighbourContainsMe(0) && neighbourContainsMe(1) && !neighbourContainsMe(2) && !neighbourContainsMe(3)) varient = 3;
            if (!neighbourContainsMe(0) && !neighbourContainsMe(1) && !neighbourContainsMe(2) && neighbourContainsMe(3)) varient = 4;
            if (!neighbourContainsMe(0) && !neighbourContainsMe(1) && neighbourContainsMe(2) && neighbourContainsMe(3)) varient = 5;
            if (neighbourContainsMe(0) && neighbourContainsMe(1) && !neighbourContainsMe(2) && !neighbourContainsMe(3)) varient = 6;
            if (neighbourContainsMe(0) && !neighbourContainsMe(1) && neighbourContainsMe(2) && !neighbourContainsMe(3)) varient = 7;
            if (!neighbourContainsMe(0) && neighbourContainsMe(1) && neighbourContainsMe(2) && !neighbourContainsMe(3)) varient = 8;
            if (!neighbourContainsMe(0) && neighbourContainsMe(1) && !neighbourContainsMe(2) && neighbourContainsMe(3)) varient = 9;
            if (neighbourContainsMe(0) && !neighbourContainsMe(1) && !neighbourContainsMe(2) && neighbourContainsMe(3)) varient = 10;
            if (neighbourContainsMe(0) && neighbourContainsMe(1) && neighbourContainsMe(2) && !neighbourContainsMe(3)) varient = 11;
            if (!neighbourContainsMe(0) && neighbourContainsMe(1) && neighbourContainsMe(2) && neighbourContainsMe(3)) varient = 12;
            if (neighbourContainsMe(0) && neighbourContainsMe(1) && !neighbourContainsMe(2) && neighbourContainsMe(3)) varient = 13;
            if (neighbourContainsMe(0) && !neighbourContainsMe(1) && neighbourContainsMe(2) && neighbourContainsMe(3)) varient = 14;
            if (neighbourContainsMe(0) && neighbourContainsMe(1) && neighbourContainsMe(2) && neighbourContainsMe(3)) varient = 15;
        }
        public bool neighbourContainsMe(int pos) {
            if (RootTile.DirectNeighbours.ContainsKey(pos)) {
                if (RootTile.DirectNeighbours[pos].AttachedBuilding != null && RootTile.DirectNeighbours[pos].AttachedBuilding != RootTile.AttachedBuilding) {
                    if (RootTile.DirectNeighbours[pos].AttachedBuilding.Name == this.Name) {
                        return true;
                    }
                }
            }
            return false;
        }
        protected static Tile iterateTile(int x, int y,Building b) {
            Tile t = b.RootTile;
            while(x > 0) {
                x--;
                t = t.DirectNeighbours[1];
            }
            while(y > 0) {
                y--;
                t = t.DirectNeighbours[3];
            }
            return t;
        }
    }
}