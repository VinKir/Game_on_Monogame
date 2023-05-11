using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectX.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectX.Game1;

namespace ProjectX.GameElements
{
    public class CarBlock
    {
        public int maxHp;
        public int hp;
        public bool isDead = false;
        public GameObject gameObject;
        public Block block;

        public int HP
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp < 0)
                {
                    hp = 0;
                    isDead = true;
                }
                gameObject.color = new Color((100 - hp / maxHp) * 255, 0, 0);
            }
        }

        public CarBlock(Game1 game, Block _block)
        {
            block = _block;
            gameObject = new GameObject(game.Content.Load<Texture2D>(BlockPaths[(int)_block]));
        }
    }
}
