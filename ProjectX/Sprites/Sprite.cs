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

        public float layer = 0;
        public float scale = 1;
        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 Direction;
        public Sprite Parent;
        public Color color;

        public Rectangle rectangle
        {
            get { return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, (int)(texture.Width* scale), (int)(texture.Height*scale)); }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            color = Color.White;
        }

        public virtual void Update(GameTime gameTime)
        {
            Direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, color, rotation, Origin, scale, SpriteEffects.None, layer);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
