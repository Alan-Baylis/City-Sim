using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace SquareTime {
    class CityView : View {
        private Camera cam;
        public CityGrid City { get; private set; }
        private UICanvas canvas;
        private HorizontalLayout hRow;
        private float scrollSpeed = 0.0001f;
        private Road prefab;
        private Vector2 from,to;
        private Dictionary<Tile, Color> hoverTiles;
        public CityView() {
            cam = new Camera();
            hoverTiles = new Dictionary<Tile, Color>();
            City = new CityGrid(513,513, TextureBank.tileTexture.Width);
            prefab = Road.createProtoype("road", 10, 30,new TileType[] { TileType.GRASS,TileType.HILL});
            Residential home = Residential.createProtoype("res",20,2,2,10,new TileType[]{ TileType.GRASS});
            canvas = new UICanvas(cam);
            hRow = new HorizontalLayout(canvas, new Vector2(10, 10), 10.0f);
            hRow.addPreppedButtonToLayout(new DragBoxButton((x) => { if (x != null) { Road.placeRoad(prefab, x); } }, City, true,(x) => { if (x != null) { if (x.checkBuilding(prefab)) hoverTiles[x] = Color.LightGreen; else hoverTiles[x] = Color.Red; } }),prefab.Name);
            hRow.addButtonToLayout(new DragBoxButton((x) => { if (x != null) {
                    x.Bulldoze();
                } }, City, false, TextureBank.bullzoneTexture));
            hRow.addPreppedButtonToLayout(new ToggleableButton((x) => { Residential.placeResidence(home, City.getTile(City.worldToMap(x))); }), home.Name); 
        }
        public override void Draw(SpriteBatch batch,GraphicsDevice d) {
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, cam.getTransform(d));
            City.Draw(batch, cam);
            foreach(KeyValuePair<Tile,Color> pair in hoverTiles) {
                batch.Draw(TextureBank.tileTexture, new Vector2(pair.Key.X * TextureBank.tileTexture.Bounds.Width, pair.Key.Y * TextureBank.tileTexture.Bounds.Height), pair.Value);
                hoverTiles = new Dictionary<Tile, Color>();
            }
            batch.End();
            batch.Begin();
            canvas.Draw(batch);
            batch.End();
        }
        public override void Update(GameTime dt) {
            if (InputManager.getKey(Keys.A)) cam.Position -=new Vector2(5.0f * (1/cam.Zoom),0);
            if (InputManager.getKey(Keys.D)) cam.Position -= new Vector2(-5.0f * (1 / cam.Zoom), 0);
            if (InputManager.getKey(Keys.W)) cam.Position -= new Vector2(0,5.0f * (1 / cam.Zoom));
            if (InputManager.getKey(Keys.S)) cam.Position -= new Vector2(0,-5.0f * (1 / cam.Zoom));
            if (InputManager.getKey(Keys.PageUp)) City.smoothNonDirect();
            canvas.Update();
            cam.Zoom += InputManager.getScrollState() * scrollSpeed;
            City.Update(dt.ElapsedGameTime.Milliseconds);
        }
    }
}
