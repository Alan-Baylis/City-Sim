using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace SquareTime {
    /// <summary>
    /// This is the main type for your game.
    public class SquareGame : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Stack<View> ViewStack { get; private set; }
        public static Viewport viewport;
        public static GraphicsDevice GraphicsD;
        public SquareGame() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
            this.IsMouseVisible = true;
            ViewStack = new Stack<View>();
        }
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            base.Initialize();
        }
        protected override void LoadContent() {
            GraphicsD = GraphicsDevice;
            viewport = GraphicsDevice.Viewport;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TextureBank.LoadTextures(Content);
            TextureBank.LoadFonts(Content);
            ViewStack.Push(new CityView());
            // TODO: use this.Content to load your game content here
        }
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            InputManager.Update();
            ViewStack.Peek().Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);
            ViewStack.Peek().Draw(spriteBatch,GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}
