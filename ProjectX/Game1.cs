using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectX.Controls;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;

namespace ProjectX
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        enum GameState
        {
            Menu,
            Garage,
            Gameplay,
        }

        GameState state;

        Color backgroundColor = Color.CornflowerBlue;
        List<Component> mainMenuGameComponents;
        List<Component> garageMenuGameComponents;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var loadLevelButton = new Button(Content.Load<Texture2D>("Controls/Button"), Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Play",
            };
            loadLevelButton.Position = new Vector2(
                        (Window.ClientBounds.Width / 2) - (loadLevelButton.Rectangle.Width / 2),
                        (Window.ClientBounds.Height / 2) - (loadLevelButton.Rectangle.Height / 2) - 140);
            loadLevelButton.Click += QuitButton_Click;


            var loadGarageButton = new Button(Content.Load<Texture2D>("Controls/Button"), Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Garage",
            };
            loadGarageButton.Position = new Vector2(
                        (Window.ClientBounds.Width / 2) - (loadGarageButton.Rectangle.Width / 2),
                        (Window.ClientBounds.Height / 2) - (loadGarageButton.Rectangle.Height / 2));
            loadGarageButton.Click += garageSceneOpenButton_Click;

            var quitButton = new Button(Content.Load<Texture2D>("Controls/Button"), Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Quit",
            };
            quitButton.Position = new Vector2(
                        (Window.ClientBounds.Width / 2) - (quitButton.Rectangle.Width / 2),
                        (Window.ClientBounds.Height / 2) - (quitButton.Rectangle.Height / 2) + 140);
            quitButton.Click += QuitButton_Click;

            mainMenuGameComponents = new List<Component>() { quitButton, loadGarageButton, loadLevelButton };

            var backToMenuButton = new Button(Content.Load<Texture2D>("Controls/Button"), Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Back",
                Position = new Vector2(10, 10),
            };
            backToMenuButton.Click += backToMenuButton_Click;
            garageMenuGameComponents = new List<Component>() { backToMenuButton };
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void backToMenuButton_Click(object sender, EventArgs e)
        {
            state = GameState.Menu;
        }

        private void garageSceneOpenButton_Click(object sender, EventArgs e)
        {
            state = GameState.Garage;
        }

        private void loadLevelButton_Click(object sender, EventArgs e)
        {
            state = GameState.Gameplay;
        }

        protected override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case GameState.Menu:
                    UpdateMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    UpdateGameplay(gameTime);
                    break;
                case GameState.Garage:
                    UpdateGarage(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            switch (state)
            {
                case GameState.Menu:
                    DrawMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    DrawGameplay(gameTime);
                    break;
                case GameState.Garage:
                    DrawGarage(gameTime);
                    break;
            }

            base.Draw(gameTime);
        }

        void UpdateMenu(GameTime gameTime)
        {
            // Обрабатывает действия игрока в экране меню
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var component in mainMenuGameComponents)
                component.Update(gameTime);
        }

        void UpdateGarage(GameTime gameTime)
        {
            // Обрабатывает действия игрока в гараже
            foreach (var component in garageMenuGameComponents)
                component.Update(gameTime);
        }

        void UpdateGameplay(GameTime gameTime)
        {
            // Обновляет состояние игровых объектов, действия игрока.
            throw new NotImplementedException();
        }

        void DrawMenu(GameTime gameTime)
        {
            // Отрисовка меню, кнопок и т.д.
            _spriteBatch.Begin();
            foreach (var component in mainMenuGameComponents)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        void DrawGarage(GameTime gameTime)
        {
            // Отрисовка результатов, кнопок и т.д.
            _spriteBatch.Begin();
            foreach (var component in garageMenuGameComponents)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        void DrawGameplay(GameTime gameTime)
        {
            // Отрисовка игровых объектов, счета и т.д.
            throw new NotImplementedException();
        }
    }
}