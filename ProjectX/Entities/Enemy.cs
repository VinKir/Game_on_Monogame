using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectX.Components;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;
using static ProjectX.Game1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ProjectX.Entities
{
    public class Enemy : GameObject
    {
        Game1 game;

        float strangeMovingTimer = 10000000f;
        float strangeMovingTimerTreshold = 1f;
        Vector2 moveTarget;
        float moveDistance = 270f;
        float moveTreshold = 50f;
        float speed = 10f;

        float fireTimer;
        float fireTimerTreshold = 0.5f;
        float fireDistance = 800;
        Player player;


        public Enemy(Texture2D texture, Game1 game, Player player) : base(texture)
        {
            fireTimerTreshold = (float)new Random().Next(20, 90) / 100;
            speed = (float)new Random().Next(6, 15);
            fireDistance = (float)new Random().Next(500, 1200);
            moveDistance = (float)new Random().Next(180, 450);
            strangeMovingTimerTreshold = (float)new Random().Next(80, 150) / 100;
            this.AddComponent(new CarBlock(Block.Enemy, Team.Enemy));
            this.player = player;
            this.game = game;
        }

        /*public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }*/

        public override void Update(GameTime gameTime)
        {
            MoveMethod(gameTime);
            FireMethod(gameTime);
        }

        void FireMethod(GameTime gameTime)
        {
            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireTimer > fireTimerTreshold &&
                Vector2.Distance(transform.Position, player.mainBlock.transform.Position)
                <= fireDistance)
            {
                fireTimer = 0;
                game.gameScene.CreateBullet(this,
                    player.mainBlock.transform.Position - transform.Position);
            }
        }

        void MoveMethod(GameTime gameTime)
        {
            strangeMovingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (strangeMovingTimer > strangeMovingTimerTreshold)
            {
                if (Vector2.Distance(transform.Position, player.mainBlock.transform.Position)
                > moveDistance)
                {
                    var diff = transform.Position - player.mainBlock.transform.Position;
                    diff.Normalize();
                    diff = new Vector2(diff.X * moveDistance, diff.Y * moveDistance);
                    moveTarget = player.mainBlock.transform.Position + diff;
                }
                strangeMovingTimer = 0;
            }

            if (Math.Abs(Vector2.Distance(transform.Position, moveTarget)) > moveTreshold)
            {
                transform.Velocity.X -=
                    Math.Abs(transform.Position.X - moveTarget.X) > 5 ?
                    Math.Sign(transform.Position.X - moveTarget.X) : 0;
                transform.Velocity.Y -=
                    Math.Abs(transform.Position.Y - moveTarget.Y) > 5 ?
                    Math.Sign(transform.Position.Y - moveTarget.Y) : 0;
                transform.Position += transform.Velocity * speed;
                transform.Velocity = Vector2.Zero;
            }
        }
    }
}
