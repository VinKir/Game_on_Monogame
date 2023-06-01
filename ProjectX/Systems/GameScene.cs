using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static ProjectX.Game1;
using ProjectX.Entities;
using ProjectX.Components;

namespace ProjectX.Systems
{
    public class GameScene
    {
        List<GameObject> gameObjects;
        List<Bullet> bullets;
        List<Button> gameButtons;
        Game1 game;
        GameWindow Window;
        SpriteBatch spriteBatch;

        float enemySpawnTimer;
        float maxEnemySpawnTreshold = 7f;
        float enemySpawnTreshold;
        int enemyMaxCount = 5;

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
            enemySpawnTreshold = maxEnemySpawnTreshold;
            gameObjects = new List<GameObject>();
            bullets = new List<Bullet>();
            bullet = new Bullet(game.Content.Load<Texture2D>("GameSprites/Bullet"), 0);
            player = new Player(game);
            gameObjects.Add(player);

            SpawnEnemy();

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

            SpawnEnemies(gameTime);
        }

        void SpawnEnemies(GameTime gameTime)
        {
            enemySpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (enemySpawnTimer > enemySpawnTreshold)
            {
                SpawnEnemy();
                enemySpawnTimer = 0;
                enemySpawnTreshold = Math.Max(enemySpawnTreshold-0.1f, 2);
            }
        }

        void SpawnEnemy()
        {
            Enemy enemy = new Enemy(game.Content.Load<Texture2D>("GameSprites/EnemyBlock"),
                game, player);
            var side = new Random().Next(0, 5);
            if (side == 0) enemy.transform.Position =
                    new Vector2(-20, new Random().Next(0, Window.ClientBounds.Height));
            else if (side == 1) enemy.transform.Position =
                    new Vector2(Window.ClientBounds.Width + 20,
                    new Random().Next(0, Window.ClientBounds.Height));
            else if (side == 3) enemy.transform.Position =
                    new Vector2(new Random().Next(0, Window.ClientBounds.Width), -20);
            else if (side == 4) enemy.transform.Position =
                    new Vector2(new Random().Next(0, Window.ClientBounds.Width),
                    Window.ClientBounds.Height + 20);
            gameObjects.Add(enemy);
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
