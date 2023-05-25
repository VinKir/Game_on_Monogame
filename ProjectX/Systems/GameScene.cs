using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static ProjectX.Game1;
using ProjectX.Entities;

namespace ProjectX.Systems
{
    public class GameScene
    {
        List<GameObject> gameObjects;
        List<Button> gameButtons;
        Game1 game;
        GameWindow Window;
        SpriteBatch spriteBatch;

        Player player;

        public GameScene(Game1 game, SpriteBatch _spriteBatch)
        {
            this.game = game;
            spriteBatch = _spriteBatch;
            Window = game.Window;
        }

        public void LoadContent()
        {
            gameObjects = new List<GameObject>();
            player = new Player(game) { Bullet = new Bullet(game.Content.Load<Texture2D>("GameSprites/Bullet")) };
            gameObjects.Add(player);

            gameButtons = new List<Button>();

            var backToMenuButton = new Button(game.Content.Load<Texture2D>("Controls/Button"), game.Content.Load<SpriteFont>("Fonts/Font"))
            {
                Text = "Back",
            };
            backToMenuButton.transform.Position = new Vector2(10, 10);
            backToMenuButton.Click += backToMenuButton_Click;
            gameButtons.Add(backToMenuButton);
        }

        private void backToMenuButton_Click(object sender, EventArgs e)
        {
            game.state = GameState.Menu;
        }

        public void UpdateGameplay(GameTime gameTime)
        {
            foreach (var component in gameObjects)
            {
                component.Update(gameTime);
                if (component as Player is Player)
                {
                    var pl = component as Player;
                    foreach (var sprite in pl.gameObjects)
                        sprite.Update(gameTime);

                    // удаляем удаленные(ненужные) объекты из списка объектов игрока
                    for (int i = 0; i < pl.gameObjects.Count; i++)
                        if (pl.gameObjects[i].isRemoved)
                        {
                            pl.gameObjects.RemoveAt(i);
                            i--;
                        }
                }
            }
            foreach (var component in gameButtons)
                component.Update(gameTime);
        }

        public void DrawGameplay(GameTime gameTime)
        {
            // Отрисовка игровых объектов
            spriteBatch.Begin();
            foreach (var component in gameObjects)
            {
                component.Draw(gameTime, spriteBatch);
                if (component as Player is Player)
                    foreach (var sprite in ((Player)component).gameObjects)
                        sprite.Draw(gameTime, spriteBatch);
            }
            foreach (var component in gameButtons)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
