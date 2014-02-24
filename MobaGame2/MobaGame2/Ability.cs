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
        public Texture2D icon;
        public Vector2 iconSize=new Vector2(90,90);
        public string texturename;
        public string iconname;

        public Ability()
        {
            this.width = 10;
            this.height= 10;
            this.attribute.speed = 10;
        }
        public Ability(Ability copy)
        {

            this.width = 10;
            this.height= 10;
            this.attribute.speed = 10;

            name=copy.name;
            manaCost=copy.manaCost;
            healthCost=copy.healthCost;
            physicalDamage=copy.physicalDamage;
            magicDamage=copy.magicDamage;
            coolDown = copy.coolDown;
            timer = copy.timer;
            cast = true;
            ghost = false;
            texture=copy.texture;
            texturename=copy.texturename;
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
                if (focus != null && !ghost)
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

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (this.attribute.visible)
            {
                spriteBatch.Draw(this.texture, this.rect, color);
            }
        }
    }
}
