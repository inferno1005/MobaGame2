using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MobaGame2
{
    class Attributes
    {
        public double health;
        public double Health
        {
            get
            {
                if (health > 0)
                    return health;
                else
                    return 0;
            }
            set
            {
                if (value >= maxhealth)
                {
                    health = maxhealth;
                }
                else
                {
                    health = value;
                }

            }
        }

        public double maxhealth;
        public double mana;
        public double maxmana;
        public double healthRegen;
        public double speed;
        public double manaRegen;
        public double range;
        public double visionrange;
        public double coolDown;
        public double armor;
        public double magicResist;
        public double attackDamage;
        public double attackSpeed;
        public int gold;
        public int goldpersec;
        public Texture2D texture;
        public string texturename;
        public bool visible;
        public bool clickable;

        public Attributes()
        {
            texturename = "texture\\1x1";
        }


        public void Draw(SpriteBatch spritebatch,Vector2 pos)
        {
            if (pos != null && spritebatch != null && texture!=null)
            {
                int healthbarpercent = (int)(50* ((Health / maxhealth)));
                //draw healh bars  going to need to fix these, find good ratios for them to display
                spritebatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y-20, 50, 5), Color.Black);
                spritebatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y-20, healthbarpercent, 5), Color.Red);

                int manabarpercent= (int)(50* ((mana/ maxmana)));
                //draw healh bars  going to need to fix these, find good ratios for them to display
                spritebatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y-10, 50, 5), Color.Black);
                spritebatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y-10, manabarpercent, 5), Color.Blue);

            }
        }
        public void Update()
        {
            if (health < maxhealth)
            {
                health += healthRegen;
            }
            else
            {
                health = maxhealth;
            }



            if (mana < maxmana)
            {
                mana += manaRegen;
            }
            else
            {
                mana = maxmana;
            }
        }
    }
}
