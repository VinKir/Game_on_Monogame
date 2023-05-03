using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Sprites
{
    public class Sprite : ICloneable
    {
        protected Texture2D texture;
        protected float rotation;

        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 Direction;
        public Sprite Parent;
        public Color color;

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            color = Color.White;
        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, color, rotation, Origin, 1, SpriteEffects.None, 0);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
