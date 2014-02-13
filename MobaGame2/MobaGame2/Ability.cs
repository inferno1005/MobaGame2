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
    class Ability : GameEntity
    {
        public string name;
        public double manaCost;
        public double healthCost;
        public double physicalDamage;
        public double magicDamage;
        public float coolDown=2f;
        public float timer=0f;
        public bool cast=false;
        public bool ghost=false;

        public Ability()
        {

        }
        public void Update(GameTime gametime)
        {
            //cool down
            if (cast)
            {
                timer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (timer >= coolDown)
                {
                    timer = 0;
                    cast = false;
                }
            }

            //move
            if (focus != null)
            {
                direction = CalcDirection(focus.center);
                distance -= this.attribute.speed;
                position += ((float)(this.attribute.speed) * direction);
            }

            //we have hit the target , we should apply damage and remove this casted ability 
            if (distance < 1)
            {
                ghost = true;
            }
        }
    }
}
