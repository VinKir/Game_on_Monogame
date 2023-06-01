using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static ProjectX.Game1;
using ProjectX.Entities;
using ProjectX.Components;
using System.Net.Sockets;

namespace ProjectX.Systems
{
    public class GameScene
    {
        List<GameObject> gameObjects = new List<GameObject>();
        List<Bullet> bullets = new List<Bullet>();
        List<Button> gameButtons;
        Game1 game;
        GameWindow Window;
        SpriteBatch spriteBatch;

        Player player;
        Bullet bullet;

        public GameScene(Game1 game, SpriteBatch _spriteBatch)
        {
            this.game = game;
            spriteBatch = _spriteBatch;
            Window = game.Window;
        }

        public void LoadContent()
        {
            bullet = new Bullet(game.Content.Load<Texture2D>("GameSprites/Bullet"), 0);
            player = new Player(game);
            gameObjects.Add(player);

            Enemy enemy1 = new Enemy(game.Content.Load<Texture2D>("GameSprites/EnemyBlock"),
                game, player);
            enemy1.transform.Position = new Vector2(1600, 500);
            gameObjects.Add(enemy1);

            #region Тренировочные мишени

            var punchingBag = new GameObject(game.Content.Load<Texture2D>(BlockPaths[(int)Block.Simple]));
            punchingBag.transform.Position = new Vector2(300, 800);
            punchingBag.AddComponent(new CarBlock(Block.Simple, Team.Obstacle));
            gameObjects.Add(punchingBag);
            punchingBag = new GameObject(game.Content.Load<Texture2D>(BlockPaths[(int)Block.Simple]));
            punchingBag.transform.Position = new Vector2(500, 800);
            punchingBag.AddComponent(new CarBlock(Block.Simple, Team.Obstacle));
            gameObjects.Add(punchingBag);
            punchingBag = new GameObject(game.Content.Load<Texture2D>(BlockPaths[(int)Block.Simple]));
            punchingBag.transform.Position = new Vector2(300, 700);
            punchingBag.AddComponent(new CarBlock(Block.Simple, Team.Obstacle));
            gameObjects.Add(punchingBag);
            punchingBag = new GameObject(game.Content.Load<Texture2D>(BlockPaths[(int)Block.Simple]));
            punchingBag.transform.Position = new Vector2(300, 600);
            punchingBag.AddComponent(new CarBlock(Block.Simple, Team.Obstacle));
            gameObjects.Add(punchingBag);

            #endregion

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
            #region удаляем удаленные(ненужные) объекты из списка объектов

            for (int i = 0; i < gameObjects.Count; i++)
                if (gameObjects[i].isRemoved)
                {
                    gameObjects.RemoveAt(i);
                    i--;
                }
            for (int i = 0; i < bullets.Count; i++)
                if (bullets[i].isRemoved)
                {
                    bullets.RemoveAt(i);
                    i--;
                }

            #endregion

            foreach (var go in gameObjects)
                go.Update(gameTime);

            foreach (var go in bullets)
            {
                go.Update(gameTime);
                foreach (var target in gameObjects)
                    if (target.GetComponent<CarBlock>() != null
                        && target.GetComponent<CarBlock>().team != go.team
                        && go.IsTouching(target))
                        go.DoDamage(target.GetComponent<CarBlock>());
                if (go.team == Team.Enemy)
                    foreach (var list in player.car)
                        foreach (var target in list)
                            if (target != null
                                && target.GetComponent<CarBlock>() != null
                                && target.GetComponent<CarBlock>().team != go.team
                                && go.IsTouching(target))
                                go.DoDamage(target.GetComponent<CarBlock>());
            }

            foreach (var component in gameButtons)
                component.Update(gameTime);
        }

        public void CreateBullet(GameObject parent, Vector2 direction)
        {
            var bullet = this.bullet.Clone() as Bullet;
            bullet.team = parent.GetComponent<CarBlock>().team;
            bullet.transform.Direction = direction;
            bullet.transform.Position = parent.transform.Position;
            bullet.transform.LinearVelocity = 10;
            bullet.LifeSpan = 2f;
            bullet.transform.Parent = parent.transform;
            bullets.Add(bullet);
        }

        public void DrawGameplay(GameTime gameTime)
        {
            // Отрисовка игровых объектов
            spriteBatch.Begin();
            foreach (var go in gameObjects)
                go.Draw(gameTime, spriteBatch);

            foreach (var go in bullets)
                go.Draw(gameTime, spriteBatch);

            foreach (var go in gameButtons)
                go.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
