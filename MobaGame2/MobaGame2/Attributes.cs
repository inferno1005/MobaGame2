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
        public double maxhealth;
        public double mana;
        public double maxmana;
        public double healthRegen;
        public double manaRegen;
        public double range;
        public double coolDown;
        public double armor;
        public double magicResist;
        public double attackDamage;
        public double attackSpeed;
        public int gold;
        public int goldpersec;
        public Texture2D texture;
        public string texturename;

        public Attributes()
        {
            texturename = "1x1";
        }


        public void Draw(SpriteBatch spritebatch,Vector2 pos)
        {
            if (pos != null && spritebatch != null && texture!=null)
            {
                //draw healh bars  going to need to fix these, find good ratios for them to display correctly
                spritebatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, 50, 10), Color.Black);
                spritebatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, (int)(100*(health/maxhealth)), 10), Color.Red);
            }
        }
    }
}
