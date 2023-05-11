using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static ProjectX.Game1;

namespace ProjectX.GameElements
{
    public class Player : Component
    {
        Game1 game;

        public List<List<CarBlock>> car; // car[y][x]
        public CarBlock mainBlock;
        public int mainBlockIndexX;
        public int mainBlockIndexY;
        public float carSpeed = 10;
        public float carRotationSpeed = 0.1f;

        public Player(Game1 game)
        {
            this.game = game;
            InitializeMachine();
            mainBlock.gameObject.Position =
                new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
        }

        void InitializeMachine()
        {
            car = new List<List<CarBlock>>();
            for (int y = 0; y < game.garageScene.carBlocks.GetLength(0); y++)
            {
                car.Add(new List<CarBlock>());
                for (int x = 0; x < game.garageScene.carBlocks.GetLength(1); x++)
                {
                    var block = game.garageScene.carBlocks[x, y];
                    if (block != Block.None)
                    {
                        car[y].Add(new CarBlock(game, block));
                        car[y][x].gameObject.isStatic = false;
                        car[y][x].gameObject.mass = 10;
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
                        block.gameObject.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            ScanCar();
            Move();
        }

        void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                mainBlock.gameObject.Velocity.X -= carSpeed;
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
                mainBlock.gameObject.Velocity.X += carSpeed;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                mainBlock.gameObject.Velocity.Y -= carSpeed;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                mainBlock.gameObject.Velocity.Y += carSpeed;

            mainBlock.gameObject.Position += mainBlock.gameObject.Velocity;
            mainBlock.gameObject.Velocity = Vector2.Zero;
            MoveOtherCarBlocks();
        }

        void MoveOtherCarBlocks()
        {
            for (int y = 0; y < car.Count; y++)
            {
                for (int x = 0; x < car[y].Count; x++)
                {
                    if (car[y][x] == null) continue;

                    var posX = mainBlock.gameObject.Position.X +
                        (x - mainBlockIndexX) * mainBlock.gameObject.rectangle.Width;
                    var posY = mainBlock.gameObject.Position.Y +
                        (y - mainBlockIndexY) * mainBlock.gameObject.rectangle.Height;
                    car[y][x].gameObject.Position = 
                        new Vector2(posX, posY);
                    car[y][x].gameObject.Direction = mainBlock.gameObject.Direction;
                    // to do движение машины
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
