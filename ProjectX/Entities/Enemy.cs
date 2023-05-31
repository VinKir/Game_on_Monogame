using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectX.Components;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using static ProjectX.Game1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ProjectX.Entities
{
    public class Enemy : GameObject
    {
        Game1 game;
        float fireTimer;
        float fireTreshold = 0.5f;
        float fireDistance = 800;
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
            fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireTimer > fireTreshold &&
                Vector2.Distance(transform.Position, player.mainBlock.transform.Position)
                <= fireDistance)
            {
                fireTimer = 0;
                game.gameScene.CreateBullet(this,
                    player.mainBlock.transform.Position - transform.Position);
            }
        }
    }
}
