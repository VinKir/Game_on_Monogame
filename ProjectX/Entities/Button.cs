using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectX.Components;
using System;

namespace ProjectX.Entities
{
    public class Button : GameObject
    {
        #region Fields

        MouseState currentMouse;
        MouseState previousMouse;
        SpriteFont font;
        bool isHovering;

        #endregion

        #region Properties

        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Color PenColor { get; set; }
        public string Text { get; set; }

        #endregion

        #region Methods

        public Button(Texture2D texture, SpriteFont font)
        {
            sprite = new Sprite(texture);
            this.font = font;
            PenColor = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if (isHovering)
                color = Color.Gray;

            spriteBatch.Draw(sprite.texture, rectangle, color);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = rectangle.X + rectangle.Width / 2 - font.MeasureString(Text).X / 2;
                var y = rectangle.Y + rectangle.Height / 2 - font.MeasureString(Text).Y / 2;
                spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            var mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            isHovering = false;
            if (mouseRectangle.Intersects(rectangle))
            {
                isHovering = true;
                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        #endregion
    }
}
