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
        public Texture castedtexture;
        public Texture uitexture;

        //public string texturename;
        //public string iconname;

        public Ability()
        {
            castedtexture = new Texture();
            uitexture = new Texture();

            this.castedtexture.width = 10;
            this.castedtexture.height= 10;

            this.uitexture.width = 90;
            this.uitexture.height= 90;

            this.attribute.speed = 10;
        }
        public Ability(Ability copy)
        {
            castedtexture = new Texture();

            this.castedtexture.width = copy.castedtexture.width;
            this.castedtexture.height= copy.castedtexture.height;
            this.attribute.speed = 10;

            this.name=copy.name;
            this.manaCost=copy.manaCost;
            this.healthCost=copy.healthCost;
            this.physicalDamage=copy.physicalDamage;
            this.magicDamage=copy.magicDamage;
            this.coolDown = copy.coolDown;
            this.timer = copy.timer;
            this.cast = true;
            this.ghost = false;
            this.texture=copy.texture;
            this.texture.name=copy.texture.name;
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
                position.Move(focus.position.center, this.attribute.speed);
            }

            //we have hit the target , we should apply damage and remove this casted ability 
            if (position.Distance()< 3)
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
                spriteBatch.Draw(this.castedtexture.texture, this.castedtexture.rect, this.castedtexture.color);
            }
        }
    }
}
