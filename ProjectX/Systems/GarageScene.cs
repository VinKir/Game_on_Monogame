using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static ProjectX.Game1;
using ProjectX.Entities;

namespace ProjectX.Systems
{
    public class GarageScene
    {
        List<GameObject> garageMenuGameObjects;
        Game1 game;
        GameWindow Window;
        SpriteBatch _spriteBatch;

        public Block[,] carBlocks;
        public Button[,] carBlocksButtons;

        public static int Money = 160;

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


            // Developer debug help
            carBlocks[0, 0] = Block.LeftWheel;
            carBlocks[0, 2] = Block.LeftWheel;
            carBlocks[1, 0] = Block.Simple;
            carBlocks[1, 1] = Block.Simple;
            carBlocks[1, 2] = Block.Simple;
            carBlocks[1, 0] = Block.Simple;
            carBlocks[1, 1] = Block.Main;
            carBlocks[1, 2] = Block.Cannon;
            carBlocks[2, 0] = Block.Simple;
            carBlocks[2, 1] = Block.Simple;
            carBlocks[2, 2] = Block.Simple;
            carBlocks[3, 0] = Block.RightWheel;
            carBlocks[3, 2] = Block.RightWheel;
            Money -= 160;

        }

        public void LoadContent()
        {
            var backToMenuButton = new Button(game.Content.Load<Texture2D>("Controls/Button"), game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Back",
            };
            backToMenuButton.transform.Position = new Vector2(10, 10);
            backToMenuButton.Click += backToMenuButton_Click;

            garageMenuGameObjects = new List<GameObject>() { backToMenuButton };
            CreateButtons();
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
                    var changeMachineBlockButton = new Button(game.Content.Load<Texture2D>(texturePath),
                        game.Content.Load<SpriteFont>("Fonts/Font"));
                    changeMachineBlockButton.transform.Position =
                        new Vector2(buildingZoneX + 50 * x, buildingZoneY + 50 * y);
                    var safeX = x;
                    var safeY = y;
                    var safeCurrBlock = currBlock;
                    changeMachineBlockButton.Click += (s, e) =>
                    {
                        if (Money >=
                            BlockPrice[(int)currBlock] - BlockPrice[(int)carBlocks[safeX, safeY]])
                        {
                            Money += BlockPrice[(int)carBlocks[safeX, safeY]];
                            carBlocks[safeX, safeY] = currBlock;
                            changeMachineBlockButton.sprite.texture = game.Content.Load<Texture2D>(BlockPaths[(int)carBlocks[safeX, safeY]]);
                            Money -= BlockPrice[(int)currBlock];
                        }
                    };
                    garageMenuGameObjects.Add(changeMachineBlockButton);
                }

            #endregion

            #region ChooseBlockButtons

            var currPosY = 200;
            for (int i = 0; i < BlockPaths.Length; i++)
            {
                var id = i;
                var blockPath = BlockPaths[i];
                var addCurrBlockButton = new Button(game.Content.Load<Texture2D>(blockPath),
                    game.Content.Load<SpriteFont>("Fonts/Font"));
                addCurrBlockButton.transform.Position = new Vector2(10, currPosY);
                addCurrBlockButton.Click += (s, e) => { currBlock = (Block)id; };//+= chooseBlock_Click;

                garageMenuGameObjects.Add(addCurrBlockButton);
                currPosY += 100;
            }

            #endregion
        }

        /* private void chooseBlock_Click(object sender, EventArgs e, int id)
        {
            currBlock = (Block)id;
        }*/

        public void UpdateGarage(GameTime gameTime)
        {
            // Обрабатывает действия игрока в гараже
            foreach (var component in garageMenuGameObjects)
                component.Update(gameTime);
        }

        public void DrawGarage(GameTime gameTime)
        {
            // Отрисовка результатов, кнопок и т.д.
            _spriteBatch.Begin();
            foreach (var component in garageMenuGameObjects)
                component.Draw(gameTime, _spriteBatch);

            // текст "Money"
            _spriteBatch.DrawString(game.Content.Load<SpriteFont>("Fonts/Font"),
                "Money ", new Vector2(1530, 50), Color.Yellow,
                0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
            // отрисовываем, сколько денег есть у игрока
            _spriteBatch.DrawString(game.Content.Load<SpriteFont>("Fonts/Font"),
                Money.ToString(), new Vector2(1700, 50), Color.Yellow,
                0, new Vector2(0, 0), 3, SpriteEffects.None, 0);

            _spriteBatch.End();
        }

        private void backToMenuButton_Click(object sender, EventArgs e)
        {
            game.state = GameState.Menu;
        }
    }
}
