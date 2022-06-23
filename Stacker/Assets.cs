using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoTime
{
    public static class Assets {
        #region SpriteFonts
        public static SpriteFont NovaSquare24;
        public static SpriteFont NovaSquare48;
        #endregion SpriteFonts
        #region Textures
        public static Texture2D Pixel;
        #endregion Textures
        public static void LoadContent(ContentManager content) {
            #region SpriteFonts
            NovaSquare24 = content.Load<SpriteFont>("Fonts/NovaSquare24");
            NovaSquare48 = content.Load<SpriteFont>("Fonts/NovaSquare48");
            #endregion SpriteFonts
            #region Textures
            Pixel = content.Load<Texture2D>("Textures/pixel");
            #endregion Textures
        }
    }
}
