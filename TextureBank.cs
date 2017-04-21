using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace SquareTime {
    static class TextureBank {
        public static Texture2D tileTexture;
        public static Texture2D buttonTexture;
        public static Texture2D bullzoneTexture;
        public static Dictionary<string, Texture2D> textureBank;
        public static SpriteFont font;
        public static void LoadTextures(ContentManager content) {
            textureBank = new Dictionary<string, Texture2D>();
            tileTexture = content.Load<Texture2D>("square");
            bullzoneTexture = content.Load<Texture2D>("button_buldoze");
            textureBank.Add("road", content.Load<Texture2D>("road_4"));
            for (int i = 0; i <= 15; i++) {
                textureBank.Add("road_" + i, content.Load<Texture2D>("road_" + i));
            }
            textureBank.Add("res", content.Load<Texture2D>("res-level-0"));
            textureBank.Add("res-level-0", content.Load<Texture2D>("res-level-0"));
            buttonTexture = content.Load<Texture2D>("button");
        }
        public static void LoadFonts(ContentManager content) {
            font = content.Load<SpriteFont>("gameFont");
        }
    }
}
