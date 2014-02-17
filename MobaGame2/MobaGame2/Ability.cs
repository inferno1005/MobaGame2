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
        public Texture2D texture;
        public string texturename;

        public Ability(double damage)
        {
            this.width = 20;
            this.height= 20;
            this.attribute.speed = 2;
            this.physicalDamage = damage;

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
                //Console.WriteLine("Should be moving!");
                direction = CalcDirection(focus.center);
                distance -= this.attribute.speed;
                position += ((float)(this.attribute.speed) * direction);
                //Console.WriteLine(position);
            }

            //we have hit the target , we should apply damage and remove this casted ability 
            if (distance < 3)
            {
                if (focus != null)
                {
                    Console.WriteLine("Should be applying damage!");
                    Console.WriteLine(focus.attribute.Health);
                    focus.attribute.Health -= this.physicalDamage;
                    Console.WriteLine(this.physicalDamage);
                    Console.WriteLine(focus.attribute.Health);
                    ghost = true;
                }
            }
        }
    }
}
