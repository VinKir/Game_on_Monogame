using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Sprites
{
    public class GameObject : Sprite
    {
        public bool isStatic;
        public float mass;
        public Vector2 Velocity;

        public Rectangle rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height); }
        }

        public GameObject(Texture2D texture) : base(texture)
        {
        }

        #region Collision

        public bool IsTouchingLeft(GameObject gameObject)
        {
            return this.rectangle.Right + this.Velocity.X > gameObject.rectangle.Left &&
                this.rectangle.Left < gameObject.rectangle.Left &&
                this.rectangle.Bottom > gameObject.rectangle.Top &&
                this.rectangle.Top < gameObject.rectangle.Bottom;
        }

        public bool IsTouchingRight(GameObject gameObject)
        {
            return this.rectangle.Left + this.Velocity.X < gameObject.rectangle.Right &&
                this.rectangle.Right > gameObject.rectangle.Right &&
                this.rectangle.Bottom > gameObject.rectangle.Top &&
                this.rectangle.Top < gameObject.rectangle.Bottom;
        }

        public bool IsTouchingTop(GameObject gameObject)
        {
            return this.rectangle.Bottom + this.Velocity.Y > gameObject.rectangle.Top &&
                this.rectangle.Top < gameObject.rectangle.Top &&
                this.rectangle.Right > gameObject.rectangle.Left &&
                this.rectangle.Left < gameObject.rectangle.Right;
        }

        public bool IsTouchingBottom(GameObject gameObject)
        {
            return this.rectangle.Top + this.Velocity.Y < gameObject.rectangle.Bottom &&
                this.rectangle.Bottom > gameObject.rectangle.Bottom &&
                this.rectangle.Right > gameObject.rectangle.Left &&
                this.rectangle.Left < gameObject.rectangle.Right;
        }

        #endregion
    }
}
