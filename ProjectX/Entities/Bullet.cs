using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectX.Components;
using System;

namespace ProjectX.Entities
{
    public class Bullet : GameObject, ICloneable
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

            transform.Position += transform.Direction * transform.LinearVelocity;
        }

        public new object Clone()
        {
            var deepClone = new Bullet(sprite.texture);
            deepClone.transform = transform.Clone() as Transform;
            deepClone.sprite = sprite.Clone() as Sprite;
            deepClone.isStatic = isStatic;
            deepClone.isRemoved = isRemoved;
            return deepClone;
        }
    }
}
