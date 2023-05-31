using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectX.Components;
using System;

namespace ProjectX.Entities
{
    public class Bullet : GameObject, ICloneable
    {
        float timer;
        public Team team;
        public int damage = 10;

        public Bullet(Texture2D texture, Team team) : base(texture)
        {
            this.team = team;
        }

        public Bullet(Texture2D texture, Team team, int damage) : base(texture)
        {
            this.team = team;
            this.damage = damage;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > LifeSpan)
                isRemoved = true;

            
            transform.Position += transform.Direction * transform.LinearVelocity;
        }

        public void DoDamage(CarBlock carBlock)
        {
            carBlock.HP -= damage;
            isRemoved = true;
        }

        public new object Clone()
        {
            var deepClone = new Bullet(sprite.texture, team);
            deepClone.transform = transform.Clone() as Transform;
            deepClone.sprite = sprite.Clone() as Sprite;
            deepClone.isStatic = isStatic;
            deepClone.isRemoved = isRemoved;
            return deepClone;
        }
    }
}
