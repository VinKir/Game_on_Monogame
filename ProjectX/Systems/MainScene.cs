using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectX.Entities;
using System;
using System.Collections.Generic;
using static ProjectX.Game1;

namespace ProjectX.Systems
{
    public class MainScene
    {
        List<GameObject> mainMenuGameObjects;
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
            loadLevelButton.transform.Position = new Vector2(
                        Window.ClientBounds.Width / 2 - loadLevelButton.rectangle.Width / 2,
                        Window.ClientBounds.Height / 2 - loadLevelButton.rectangle.Height / 2 - 140);
            loadLevelButton.Click += loadLevelButton_Click;


            var loadGarageButton = new Button(game.Content.Load<Texture2D>("Controls/Button"), game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Garage",
            };
            loadGarageButton.transform.Position = new Vector2(
                        Window.ClientBounds.Width / 2 - loadGarageButton.rectangle.Width / 2,
                        Window.ClientBounds.Height / 2 - loadGarageButton.rectangle.Height / 2);
            loadGarageButton.Click += garageSceneOpenButton_Click;

            var quitButton = new Button(game.Content.Load<Texture2D>("Controls/Button"), game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Quit",
            };
            quitButton.transform.Position = new Vector2(
                        Window.ClientBounds.Width / 2 - quitButton.rectangle.Width / 2,
                        Window.ClientBounds.Height / 2 - quitButton.rectangle.Height / 2 + 140);
            quitButton.Click += QuitButton_Click;

            mainMenuGameObjects = new List<GameObject>() { quitButton, loadGarageButton, loadLevelButton };
        }

        public void UpdateMenu(GameTime gameTime)
        {
            // Обрабатывает действия игрока в экране меню
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();

            foreach (var component in mainMenuGameObjects)
                component.Update(gameTime);
        }

        public void DrawMenu(GameTime gameTime)
        {
            // Отрисовка меню, кнопок и т.д.
            _spriteBatch.Begin();
            foreach (var component in mainMenuGameObjects)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }

        private void garageSceneOpenButton_Click(object sender, EventArgs e)
        {
            game.state = GameState.Garage;
        }

        private void loadLevelButton_Click(object sender, EventArgs e)
        {
            game.state = GameState.Gameplay;
            game.gameScene.LoadContent();
        }
    }
}
