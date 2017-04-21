using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SquareTime {
    abstract class Button {
        public Vector2 screenPos { get; set; }
        public RenderTarget2D ButtonSprite { get; set; }
        public Texture2D baseTexture { get; protected set; }
        string embededText;
        public bool selected;
        UICanvas canvas;
        protected Button() {
            this.screenPos = Vector2.Zero;
            this.baseTexture = TextureBank.buttonTexture;
        }
        public void Draw(SpriteBatch batch) {
            Color col = Color.White;
            if (selected) {
                col = Color.Yellow;
            }
            if (ButtonSprite != null) {
                batch.Draw(ButtonSprite, screenPos,col);
            }
            else {
                batch.Draw(baseTexture, screenPos, col);
            }
            if (embededText != null) {
                batch.DrawString(TextureBank.font, embededText, screenPos, col);
            }
        }
    }
    class SelectableButton : Button{
        public Action<Button> onClickAction;
        public SelectableButton(Action<Button> onClickAction):base() {
            this.onClickAction = onClickAction;
        }
        public SelectableButton(Vector2 pos,Texture2D baseSprite,Action<Button> onClickAction) {
            this.screenPos = pos;
            this.baseTexture = baseSprite;
            this.onClickAction = onClickAction;
        }
    }
    class ToggleableButton : Button {
        public Action<Vector2> onActiveClick;
        public Action<Vector2> onActive;
        public bool active;
        public ToggleableButton(Action<Vector2> onActiveClick):base() {
            this.onActiveClick = onActiveClick;
        }
        public ToggleableButton(Vector2 pos, Texture2D baseSprite, Action<Vector2> onActiveClick) {
            this.screenPos = pos;
            this.active = false;
            this.baseTexture = baseSprite;
            this.onActiveClick = onActiveClick;
        }
        public ToggleableButton(Vector2 pos, Texture2D baseSprite, Action<Vector2> onActiveClick,Action<Vector2> onActive):this(pos,baseSprite,onActiveClick) {
            this.onActive = onActive;
        }
    }
    class DragBoxButton : Button {
        public Vector2 startPoint;
        public bool active;
        public bool Dragging { get; private set; }
        public Action<Vector2,Vector2> onReleaseClick;
        public Action<Vector2, Vector2> onDragHover;
        public DragBoxButton(Action<Tile> tileAction, CityGrid grid, bool linear,Texture2D texture):this(tileAction, grid, linear) {
            this.screenPos = Vector2.Zero;
            this.baseTexture = texture;
        }
        public DragBoxButton(Action<Tile> tileAction,CityGrid grid,bool linear) : base() {
            if (linear) {
                this.onReleaseClick = (start, end) => {
                    Vector2 worldStart = grid.worldToMap(start);
                    Vector2 worldEnd = grid.worldToMap(end);
                    if (Math.Abs(worldStart.X - worldEnd.X) >= Math.Abs(worldStart.Y - worldEnd.Y)) {
                        for (int x = (int)Math.Min(worldStart.X, worldEnd.X); x <= (int)Math.Max(worldStart.X, worldEnd.X); x++) {
                            tileAction(grid.getTile(x, (int)worldStart.Y));
                        }
                    }
                    else {
                        for (int y = (int)Math.Min(worldStart.Y, worldEnd.Y); y <= (int)Math.Max(worldStart.Y, worldEnd.Y); y++) {
                            tileAction(grid.getTile((int)worldStart.X, y));
                        }
                    }
                    Dragging = false;
                };
            }
            else {
                this.onReleaseClick = (start, end) => {
                    Vector2 worldStart = grid.worldToMap(start);
                    Vector2 worldEnd = grid.worldToMap(end);
                    for (int x = (int)Math.Min(worldStart.X, worldEnd.X); x <= (int)Math.Max(worldStart.X, worldEnd.X); x++) {
                        for (int y = (int)Math.Min(worldStart.Y, worldEnd.Y); y <= (int)Math.Max(worldStart.Y, worldEnd.Y); y++) {
                            tileAction(grid.getTile(x, y));
                        }
                    }
                    Dragging = false;
                };
            }
        }
        public DragBoxButton(Action<Tile> tileAction,CityGrid grid,bool linear,Action<Tile> hoverAction) : this(tileAction, grid, linear) {
            if (linear) {
                this.onDragHover = (start, end) => {
                    Vector2 worldStart = grid.worldToMap(start);
                    Vector2 worldEnd = grid.worldToMap(end);
                    if (Math.Abs(worldStart.X - worldEnd.X) >= Math.Abs(worldStart.Y - worldEnd.Y)) {
                        for (int x = (int)Math.Min(worldStart.X, worldEnd.X); x <= (int)Math.Max(worldStart.X, worldEnd.X); x++) {
                            hoverAction(grid.getTile(x, (int)worldStart.Y));
                        }
                    }
                    else {
                        for (int y = (int)Math.Min(worldStart.Y, worldEnd.Y); y <= (int)Math.Max(worldStart.Y, worldEnd.Y); y++) {
                            hoverAction(grid.getTile((int)worldStart.X, y));
                        }
                    }
                };
            }
            else {
                this.onReleaseClick = (start, end) => {
                    Vector2 worldStart = grid.worldToMap(start);
                    Vector2 worldEnd = grid.worldToMap(end);
                    for (int x = (int)Math.Min(worldStart.X, worldEnd.X); x <= (int)Math.Max(worldStart.X, worldEnd.X); x++) {
                        for (int y = (int)Math.Min(worldStart.Y, worldEnd.Y); y <= (int)Math.Max(worldStart.Y, worldEnd.Y); y++) {
                            hoverAction(grid.getTile(x, y));
                        }
                    }
                };
            }
        }
        public DragBoxButton(Action<Vector2, Vector2> onReleaseClick):base() {
            this.onReleaseClick = onReleaseClick;
        }
        public DragBoxButton(Vector2 pos, Texture2D baseSprite, Action<Vector2,Vector2> onReleaseClick) {
            this.screenPos = pos;
            this.Dragging = false;
            this.baseTexture = baseSprite;
            this.onReleaseClick = onReleaseClick;
        } 
        public void StartDrag(Vector2 pos) {
            this.startPoint = pos;
            this.Dragging = true;
        }
        
    }
}
