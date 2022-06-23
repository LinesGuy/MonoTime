using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoTime
{
    public class GameRoot : Microsoft.Xna.Framework.Game {
        public static GameRoot Instance;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Timeline _timeline;

        public GameRoot() {
            Instance = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            _timeline = new Timeline("timeline.txt");
            _graphics.PreferredBackBufferWidth = 1440;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Assets.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime) {
            Input.Update();
            if (Input.Keyboard.IsKeyDown(Keys.R))
                _timeline = new Timeline("timeline.txt");
            _timeline.Update(gameTime.TotalGameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            _spriteBatch.Begin();
            _timeline.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
