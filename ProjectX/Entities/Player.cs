using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectX.Components;
using System;
using System.Collections.Generic;
using static ProjectX.Game1;

namespace ProjectX.Entities
{
    public class Player : GameObject
    {
        Game1 game;
        float fireTimer;
        float fireTreshold = 0.3f;

        public List<List<GameObject>> car; // car[y][x]
        public GameObject mainBlock;
        public int mainBlockIndexX;
        public int mainBlockIndexY;
        public float carSpeed = 5;
        public float carRotationSpeed = 0.03f;
        public float carScaleMultiplier = 0.4f;


        public Player(Game1 game)
        {
            this.game = game;
            InitializeMachine();
            mainBlock.transform.Position =
                new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
        }

        void InitializeMachine()
        {
            car = new List<List<GameObject>>();
            for (int y = 0; y < game.garageScene.carBlocks.GetLength(0); y++)
            {
                car.Add(new List<GameObject>());
                for (int x = 0; x < game.garageScene.carBlocks.GetLength(1); x++)
                {
                    var block = game.garageScene.carBlocks[x, y];
                    if (block != Block.None)
                    {
                        car[y].Add(new GameObject(game.Content.Load<Texture2D>(BlockPaths[(int)block])));
                        car[y][x].isStatic = false;
                        car[y][x].transform.scale = carScaleMultiplier;
                        car[y][x].AddComponent(new CarBlock(block, Team.Player));
                        car[y][x].AddComponent(new Rigidbody());
                        car[y][x].GetComponent<Rigidbody>().mass = 10;
                        if (block == Block.Main)
                        {
                            mainBlock = car[y][x];
                            mainBlockIndexX = x;
                            mainBlockIndexY = y;
                        }
                    }
                    else
                        car[y].Add(null);
                }
            }
            MoveOtherCarBlocks();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var listOfBlocks in car)
                foreach (var block in listOfBlocks)
                    if (block != null)
                        block.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            ScanCar();
            Move();
            Fire(gameTime);
        }

        void Fire(GameTime gameTime)
        {
            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireTimer > fireTreshold && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                foreach (var listOfBlocks in car)
                    foreach (var block in listOfBlocks)
                        if (block?.GetComponent<CarBlock>().block == Block.Cannon)
                            game.gameScene.CreateBullet(
                                block, block.transform.Direction);
                fireTimer = 0;
            }
        }

        void Move()
        {
            var MoveDirForwardNoBackward = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                MoveDirForwardNoBackward = 1;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                MoveDirForwardNoBackward = -1;

            if (MoveDirForwardNoBackward != 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    mainBlock.transform.Rotation -= carRotationSpeed;
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    mainBlock.transform.Rotation += carRotationSpeed;
            }
            mainBlock.transform.Velocity.X -=
                MoveDirForwardNoBackward * (float)Math.Sin(mainBlock.transform.Rotation * -1) * carSpeed;
            mainBlock.transform.Velocity.Y -=
                MoveDirForwardNoBackward * (float)Math.Cos(mainBlock.transform.Rotation * -1) * carSpeed;

            mainBlock.transform.Position += mainBlock.transform.Velocity;
            mainBlock.transform.Velocity = Vector2.Zero;
            MoveOtherCarBlocks();
        }

        void MoveOtherCarBlocks()
        {
            for (int y = 0; y < car.Count; y++)
            {
                for (int x = 0; x < car[y].Count; x++)
                {
                    if (car[y][x] == null || car[y][x].GetComponent<CarBlock>().block == Block.Main) continue;
                    var mainBlockX = mainBlock.transform.Position.X;
                    var mainBlockY = mainBlock.transform.Position.Y;
                    var localPosX = mainBlockX +
                        (x - mainBlockIndexX) * mainBlock.rectangle.Width
                        * (float)Math.Cos(mainBlock.transform.Rotation) -
                        (y - mainBlockIndexY) * mainBlock.rectangle.Height
                        * (float)Math.Sin(mainBlock.transform.Rotation);
                    var localPosY = mainBlockY +
                        (y - mainBlockIndexY) * mainBlock.rectangle.Height
                        * (float)Math.Cos(mainBlock.transform.Rotation) +
                        (x - mainBlockIndexX) * mainBlock.rectangle.Width
                        * (float)Math.Sin(mainBlock.transform.Rotation);
                    car[y][x].transform.Position = new Vector2(localPosX, localPosY);
                    if (car[y][x].GetComponent<CarBlock>().block == Block.Cannon)
                    {
                        var mousePos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
                        car[y][x].transform.Direction = mousePos - car[y][x].transform.Position;
                        car[y][x].transform.Rotation += (float)Math.PI / 2;
                    }
                    else
                        car[y][x].transform.Rotation = mainBlock.transform.Rotation;
                }
            }
        }

        void DetectCollisionsWithOtherGameObjects()
        {

        }

        void ScanCar()
        {
            // блоки, которые не соединены с главным - отваливаются
            for (int y = 0; y < car.Count; y++)
                for (int x = 0; x < car[y].Count; x++)
                    if (car[y][x] != null && car[y][x].isRemoved)
                    {
                        if (car[y][x].GetComponent<CarBlock>().block == Block.Main)
                            mainBlock = null;
                        car[y][x] = null;
                    }
        }
    }
}
