using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectX.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.GameElements
{
    public class Bullet : GameObject
    {
        float timer;

        public Bullet(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > LifeSpan)
                isRemoved = true;

            Position += Direction * LinearVelocity;
        }
    }
}
