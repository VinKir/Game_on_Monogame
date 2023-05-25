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

        public List<List<CarBlockComponent>> car; // car[y][x]
        public CarBlockComponent mainBlock;
        public int mainBlockIndexX;
        public int mainBlockIndexY;
        public float carSpeed = 5;
        public float carRotationSpeed = 0.03f;
        public float carScaleMultiplier = 0.4f;

        public Bullet Bullet;

        // здесь хранятся например пули, которые тоже нужно в сцене пробегаться по Update и Draw
        public List<GameObject> gameObjects;

        public Player(Game1 game)
        {
            this.game = game;
            gameObjects = new List<GameObject>();
            InitializeMachine();
            mainBlock.gameObject.transform.Position =
                new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
        }

        void InitializeMachine()
        {
            car = new List<List<CarBlockComponent>>();
            for (int y = 0; y < game.garageScene.carBlocks.GetLength(0); y++)
            {
                car.Add(new List<CarBlockComponent>());
                for (int x = 0; x < game.garageScene.carBlocks.GetLength(1); x++)
                {
                    var block = game.garageScene.carBlocks[x, y];
                    if (block != Block.None)
                    {
                        car[y].Add(new CarBlockComponent(game, block));
                        car[y][x].gameObject.isStatic = false;
                        car[y][x].gameObject.transform.scale = carScaleMultiplier;
                        //car[y][x].rigidbody.mass = 10;
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
                        block.gameObject.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            ScanCar();
            Move();
            Fire();
        }

        void Fire()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                foreach (var listOfBlocks in car)
                    foreach (var block in listOfBlocks)
                        if (block?.block == Block.Cannon)
                        {
                            var bullet = Bullet.Clone() as Bullet;
                            bullet.transform.Rotation = block.gameObject.transform.Rotation - (float)Math.PI / 2;
                            bullet.transform.Direction = block.gameObject.transform.Direction;
                            bullet.transform.Position = block.gameObject.transform.Position;
                            bullet.transform.LinearVelocity = 10;
                            bullet.LifeSpan = 3f;
                            bullet.transform.Parent = block.gameObject.transform;
                            gameObjects.Add(bullet);
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
                    mainBlock.gameObject.transform.Rotation -= carRotationSpeed;
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    mainBlock.gameObject.transform.Rotation += carRotationSpeed;
            }
            mainBlock.gameObject.transform.Velocity.X -=
                MoveDirForwardNoBackward * (float)Math.Sin(mainBlock.gameObject.transform.Rotation * -1) * carSpeed;
            mainBlock.gameObject.transform.Velocity.Y -=
                MoveDirForwardNoBackward * (float)Math.Cos(mainBlock.gameObject.transform.Rotation * -1) * carSpeed;

            mainBlock.gameObject.transform.Position += mainBlock.gameObject.transform.Velocity;
            mainBlock.gameObject.transform.Velocity = Vector2.Zero;
            MoveOtherCarBlocks();
        }

        void MoveOtherCarBlocks()
        {
            for (int y = 0; y < car.Count; y++)
            {
                for (int x = 0; x < car[y].Count; x++)
                {
                    if (car[y][x] == null || car[y][x].block == Block.Main) continue;
                    var mainBlockX = mainBlock.gameObject.transform.Position.X;
                    var mainBlockY = mainBlock.gameObject.transform.Position.Y;
                    var localPosX = mainBlockX +
                        (x - mainBlockIndexX) * mainBlock.gameObject.rectangle.Width
                        * (float)Math.Cos(mainBlock.gameObject.transform.Rotation) -
                        (y - mainBlockIndexY) * mainBlock.gameObject.rectangle.Height
                        * (float)Math.Sin(mainBlock.gameObject.transform.Rotation);
                    var localPosY = mainBlockY +
                        (y - mainBlockIndexY) * mainBlock.gameObject.rectangle.Height
                        * (float)Math.Cos(mainBlock.gameObject.transform.Rotation) +
                        (x - mainBlockIndexX) * mainBlock.gameObject.rectangle.Width
                        * (float)Math.Sin(mainBlock.gameObject.transform.Rotation);
                    car[y][x].gameObject.transform.Position = new Vector2(localPosX, localPosY);
                    if (car[y][x].block == Block.Cannon)
                    {
                        var mousePos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
                        var direction = mousePos - car[y][x].gameObject.transform.Position;
                        var rotation = (float)Math.Atan2(direction.Y, direction.X);
                        car[y][x].gameObject.transform.Rotation = rotation + (float)Math.PI / 2;
                    }
                    else
                        car[y][x].gameObject.transform.Rotation = mainBlock.gameObject.transform.Rotation;
                }
            }
        }

        void DetectCollisionsWithOtherGameObjects()
        {

        }

        void ScanCar()
        {
            // блоки, которые не соединены с главным - отваливаются
        }
    }
}
