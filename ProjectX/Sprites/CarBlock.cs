using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Sprites
{
    class CarBlock : Sprite
    {
        public int maxHp;
        int hp;
        public int HP
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp < 0)
                    hp = 0;
                color = new Color((100 - hp / maxHp) * 255, 0, 0);
            }
        }

        public CarBlock(Texture2D texture) : base(texture)
        {
        }
    }
}
