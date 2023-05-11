using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectX.Game1;
using ProjectX.GameElements;

namespace ProjectX.Controls
{
    public class GameScene
    {
        List<Component> gameComponents;
        Game1 game;
        GameWindow Window;
        SpriteBatch spriteBatch;

        Player player;

        public GameScene(Game1 game, SpriteBatch _spriteBatch)
        {
            this.game = game;
            this.spriteBatch = _spriteBatch;
            Window = game.Window;
        }

        public void LoadContent()
        {
            gameComponents = new List<Component>();

            var backToMenuButton = new Button(game.Content.Load<Texture2D>("Controls/Button"), game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Back",
                Position = new Vector2(10, 10),
            };
            backToMenuButton.Click += backToMenuButton_Click;
            gameComponents.Add(backToMenuButton);

            player = new Player(game);
            gameComponents.Add(player);
        }

        private void backToMenuButton_Click(object sender, EventArgs e)
        {
            game.state = GameState.Menu;
        }

        public void UpdateGameplay(GameTime gameTime)
        {
            foreach (var component in gameComponents)
                component.Update(gameTime);

        }
        public void DrawGameplay(GameTime gameTime)
        {
            // Отрисовка результатов, кнопок и т.д.
            spriteBatch.Begin();
            foreach (var component in gameComponents)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        void DrawCar()
        {
            // блоки, которые не соединены с главным - отваливаются
        }
    }
}
