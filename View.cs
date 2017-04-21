using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquareTime {
    public abstract class View {
        public abstract void Update(GameTime dt);
        public abstract void Draw(SpriteBatch batch,GraphicsDevice d);
    }
}
