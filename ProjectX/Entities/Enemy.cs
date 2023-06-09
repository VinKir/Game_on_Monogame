﻿using Microsoft.Xna.Framework;
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
        public int bounty = 2;

        public float strangeMovingTimerTreshold = 1f;
        public float moveDistance = 270f;
        public float moveTreshold = 50f;
        public float speed = 10f;

        public float fireTimerTreshold = 0.5f;
        public float fireDistance = 800;

        float strangeMovingTimer = 10000000f;
        float fireTimer;

        Game1 game;
        Vector2 moveTarget;
        Player player;

        public Enemy(Texture2D texture, Game1 game, Player player) : base(texture)
        {
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
