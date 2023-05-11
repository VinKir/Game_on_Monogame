using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static ProjectX.Game1;

namespace ProjectX.Controls
{
    public class MainScene
    {
        List<Component> mainMenuGameComponents;
        Game1 game;
        GameWindow Window;
        SpriteBatch _spriteBatch;

        public MainScene(Game1 game, SpriteBatch spriteBatch)
        {
            this.game = game;
            _spriteBatch = spriteBatch;
            Window = game.Window;
        }

        public void LoadContent()
        {
            var loadLevelButton = new Button(game.Content.Load<Texture2D>("Controls/Button"), game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Play",
            };
            loadLevelButton.Position = new Vector2(
                        (Window.ClientBounds.Width / 2) - (loadLevelButton.Rectangle.Width / 2),
                        (Window.ClientBounds.Height / 2) - (loadLevelButton.Rectangle.Height / 2) - 140);
            loadLevelButton.Click += loadLevelButton_Click;


            var loadGarageButton = new Button(game.Content.Load<Texture2D>("Controls/Button"), game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Garage",
            };
            loadGarageButton.Position = new Vector2(
                        (Window.ClientBounds.Width / 2) - (loadGarageButton.Rectangle.Width / 2),
                        (Window.ClientBounds.Height / 2) - (loadGarageButton.Rectangle.Height / 2));
            loadGarageButton.Click += garageSceneOpenButton_Click;

            var quitButton = new Button(game.Content.Load<Texture2D>("Controls/Button"), game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Quit",
            };
            quitButton.Position = new Vector2(
                        (Window.ClientBounds.Width / 2) - (quitButton.Rectangle.Width / 2),
                        (Window.ClientBounds.Height / 2) - (quitButton.Rectangle.Height / 2) + 140);
            quitButton.Click += QuitButton_Click;

            mainMenuGameComponents = new List<Component>() { quitButton, loadGarageButton, loadLevelButton };
        }

        public void UpdateMenu(GameTime gameTime)
        {
            // Обрабатывает действия игрока в экране меню
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();

            foreach (var component in mainMenuGameComponents)
                component.Update(gameTime);
        }

        public void DrawMenu(GameTime gameTime)
        {
            // Отрисовка меню, кнопок и т.д.
            _spriteBatch.Begin();
            foreach (var component in mainMenuGameComponents)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }

        private void garageSceneOpenButton_Click(object sender, EventArgs e)
        {
            game.state = Game1.GameState.Garage;
        }

        private void loadLevelButton_Click(object sender, EventArgs e)
        {
            game.state = GameState.Gameplay;
            game.gameScene.LoadContent();
        }
    }
}
