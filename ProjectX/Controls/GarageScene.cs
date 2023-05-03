using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectX.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using static ProjectX.Game1;
using static ProjectX.Sprites.CarBlock;

namespace ProjectX.Controls
{
    public class GarageScene
    {
        List<Component> garageMenuGameComponents;
        Game1 game;
        GameWindow Window;
        SpriteBatch _spriteBatch;

        public Block[,] carBlocks;
        public Button[,] carBlocksButtons;

        Block currBlock;

        public GarageScene(Game1 game, SpriteBatch spriteBatch)
        {
            this.game = game;
            _spriteBatch = spriteBatch;
            Window = game.Window;
            carBlocks = new Block[11, 11];
            InitializeMachineArray();
            carBlocksButtons = new Button[11, 11];
        }

        void InitializeMachineArray()
        {
            for (int i = 0; i < carBlocks.GetLength(0); i++)
                for (int j = 0; j < carBlocks.GetLength(1); j++)
                    carBlocks[i, j] = Block.None;
        }

        public void LoadContent()
        {
            var backToMenuButton = new Button(game.Content.Load<Texture2D>("Controls/Button"), game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Back",
                Position = new Vector2(10, 10),
            };
            backToMenuButton.Click += backToMenuButton_Click;

            garageMenuGameComponents = new List<Component>() { backToMenuButton };
            CreateButtons();
            var print = new Button(game.Content.Load<Texture2D>("Controls/Button"), game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Position = new Vector2(1500, 800),
            };
            print.Click += (s, e) => { print.Text = currBlock.ToString(); };

            garageMenuGameComponents.Add(print);
        }

        void CreateButtons()
        {
            #region BuildingZoneButtons

            int buildingZoneX = 600;
            int buildingZoneY = 270;
            for (int x = 0; x < carBlocks.GetLength(0); x++)
                for (int y = 0; y < carBlocks.GetLength(1); y++)
                {
                    string texturePath = BlockPaths[(int)carBlocks[x, y]];
                    var changeMachineBlockButton = new Button(game.Content.Load<Texture2D>(texturePath), game.Content.Load<SpriteFont>("Fonts/Font"))
                    {
                        Position = new Vector2(buildingZoneX + 50 * x, buildingZoneY + 50 * y),
                    };
                    var safeX = x;
                    var safeY = y;
                    var safeCurrBlock = currBlock;
                    changeMachineBlockButton.Click += (s, e) => 
                    { 
                        carBlocks[safeX, safeY] = currBlock;
                        changeMachineBlockButton.texture = game.Content.Load<Texture2D>(BlockPaths[(int)carBlocks[safeX, safeY]]);
                     };
                    garageMenuGameComponents.Add(changeMachineBlockButton);
                }

            #endregion

            #region ChooseBlockButtons

            var currPosY = 200;
            for (int i = 0; i < BlockPaths.Length; i++)
            {
                var id = i;
                var blockPath = BlockPaths[i];
                var addCurrBlockButton = new Button(game.Content.Load<Texture2D>(blockPath), game.Content.Load<SpriteFont>("Fonts/Font"))
                {
                    Position = new Vector2(10, currPosY),
                };
                addCurrBlockButton.Click += (s, e) => { currBlock = (Block)id; };//+= chooseBlock_Click;

                garageMenuGameComponents.Add(addCurrBlockButton);
                currPosY += 100;
            }

            #endregion
        }

        private void chooseBlock_Click(object sender, EventArgs e, int id)
        {
            currBlock = (Block)id;
        }

        public void UpdateGarage(GameTime gameTime)
        {
            // Обрабатывает действия игрока в гараже
            foreach (var component in garageMenuGameComponents)
                component.Update(gameTime);
        }

        public void DrawGarage(GameTime gameTime)
        {
            // Отрисовка результатов, кнопок и т.д.
            _spriteBatch.Begin();
            foreach (var component in garageMenuGameComponents)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        private void backToMenuButton_Click(object sender, EventArgs e)
        {
            game.state = GameState.Menu;
        }
    }
}
