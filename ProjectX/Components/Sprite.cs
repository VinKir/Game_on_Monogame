using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectX.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Components
{
    public class Sprite : IComponent, ICloneable
    {
        public Texture2D texture;
        public float layer = 0;

        public Color color;

        GameObject gameObject;
        public GameObject boundEntity { get => gameObject; set => gameObject = value; }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            color = Color.White;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
