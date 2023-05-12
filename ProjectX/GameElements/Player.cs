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
        public float carSpeed = 5;
        public float carRotationSpeed = 0.03f;

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
            var MoveDirForwardNoBackward = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                MoveDirForwardNoBackward = 1;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                MoveDirForwardNoBackward = -1;

            if (MoveDirForwardNoBackward != 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    mainBlock.gameObject.Rotation -= carRotationSpeed;
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    mainBlock.gameObject.Rotation += carRotationSpeed;
            }
            mainBlock.gameObject.Velocity.X -=
                MoveDirForwardNoBackward * (float)Math.Sin(mainBlock.gameObject.Rotation * -1) * carSpeed;
            mainBlock.gameObject.Velocity.Y -=
                MoveDirForwardNoBackward * (float)Math.Cos(mainBlock.gameObject.Rotation * -1) * carSpeed;

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
                    if (car[y][x] == null || car[y][x].block == Block.Main) continue;
                    var mainBlockX = mainBlock.gameObject.Position.X;
                    var mainBlockY = mainBlock.gameObject.Position.Y;
                    var localPosX = mainBlockX +
                        (x - mainBlockIndexX) * mainBlock.gameObject.rectangle.Width
                        * (float)Math.Cos(mainBlock.gameObject.Rotation ) -
                        (y - mainBlockIndexY) * mainBlock.gameObject.rectangle.Height
                        * (float)Math.Sin(mainBlock.gameObject.Rotation);
                    var localPosY = mainBlockY +
                        (y - mainBlockIndexY) * mainBlock.gameObject.rectangle.Height
                        * (float)Math.Cos(mainBlock.gameObject.Rotation) +
                        (x - mainBlockIndexX) * mainBlock.gameObject.rectangle.Width
                        * (float)Math.Sin(mainBlock.gameObject.Rotation);
                    car[y][x].gameObject.Position = new Vector2(localPosX, localPosY);
                    car[y][x].gameObject.Rotation = mainBlock.gameObject.Rotation;
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
