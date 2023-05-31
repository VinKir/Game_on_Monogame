using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectX.Entities;
using static ProjectX.Game1;

namespace ProjectX.Components
{
    public enum Team
    {
        Player = 0,
        Enemy = 1,
    }

    public class CarBlock : IComponent
    {
        int hp;
        public int maxHp = 100;
        public Block block;
        public Team team;

        GameObject gameObject;
        public GameObject boundEntity { get => gameObject; set => gameObject = value; }

        public int HP
        {
            get { return hp; }
            set
            {
                hp = value;
                int colorValue = (int)((float)hp / (maxHp != 0 ? maxHp : 1) * 255);
                gameObject.sprite.color = new Color(255, colorValue, colorValue);
                if (hp < 0)
                {
                    hp = 0;
                    gameObject.isRemoved = true;
                    gameObject.sprite.color = new Color(0, 0, 0);
                }
            }
        }

        public CarBlock(Block _block, Team team)
        {
            hp = maxHp;
            block = _block;
            this.team = team;
        }
    }
}
