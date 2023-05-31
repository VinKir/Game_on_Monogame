using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectX.Systems;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;

namespace ProjectX
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public enum GameState
        {
            Menu,
            Garage,
            Gameplay,
        }
        public enum Block
        {
            None, Main, Simple, LeftWheel, RightWheel, GunBase, Cannon, Enemy
        }
        public static string[] BlockPaths = new string[] {
            "Controls/AddBlockButton",
            "GameSprites/MainBlock",
            "GameSprites/SimpleBlock",
            "GameSprites/LeftWheelBlock",
            "GameSprites/RightWheelBlock",
            "GameSprites/GunBaseBlock",
            "GameSprites/CannonBlock",
            "GameSprites/EnemyBlock",  };

        Color backgroundColor = Color.CornflowerBlue;
        public GameState state;
        public MainScene mainScene;
        public GarageScene garageScene;
        public GameScene gameScene;

        public static int ScreenHeight;
        public static int ScreenWidth;

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

            ScreenHeight = _graphics.PreferredBackBufferHeight;
            ScreenWidth = _graphics.PreferredBackBufferWidth;

            //_graphics.IsFullScreen = true;
            // to do -------------------------------------------------------------------
            // разкоментировать, когда игра будет готова, чтоб игра была на весь экран
            // пока это включено не выходит нормально отлаживать ошибки

            _graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            mainScene = new MainScene(this, _spriteBatch);
            garageScene = new GarageScene(this, _spriteBatch);
            gameScene = new GameScene(this, _spriteBatch);

            mainScene.LoadContent();
            garageScene.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case GameState.Menu:
                    mainScene.UpdateMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    gameScene.UpdateGameplay(gameTime);
                    break;
                case GameState.Garage:
                    garageScene.UpdateGarage(gameTime);
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
                    mainScene.DrawMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    gameScene.DrawGameplay(gameTime);
                    break;
                case GameState.Garage:
                    garageScene.DrawGarage(gameTime);
                    break;
            }

            base.Draw(gameTime);
        }
    }
}