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
    class Champ :GameEntity
    {
        public string Name;
        public Attributes attributes;
        public List<Ability> abilities;

        public Champ()
        {
            attributes = new Attributes();
            abilities = new List<Ability>();
        }



        public void Draw(SpriteBatch spriteBatch,Color color)
        {
            /*
            //so slow ;-;
            this.rotation %= (float)Math.PI * 2 ;
            this.rotation += (float)Math.PI / 2;

            spriteBatch.Draw(this.texture,
                new Rectangle((int)this.center.X,(int)this.center.Y,(int)this.width,(int)this.height),
                null,
                this.color,
                this.rotation,
                new Vector2(this.width,this.height),
                SpriteEffects.None,0);
             */


            spriteBatch.Draw(this.texture,this.rect,color);
            foreach (var ability in abilities)
            {
                if(ability.texture!=null && ability.rect!=null && ability.color!=null)
                    spriteBatch.Draw(ability.texture, ability.rect, ability.color);
            }
            attributes.Draw(spriteBatch, this.position);
        }
        public void Updater(Rectangle rect,GameTime gametime)
        {
            this.Update(rect);
            attributes.Update();

            //attack focused object
            if (focus != null)
            {
                if (focus.distance < this.attribute.range)
                {
                    if (!this.abilities[0].cast)
                    {
                        //Console.WriteLine("SPAWNING AN ABILITY!");
                        this.abilities[0].cast = true;
                        Ability temp = new Ability();
                        temp.texture = this.abilities[0].texture;
                        temp.focus = this.focus;
                        temp.position = this.position;
                        abilities.Add(temp);
                    }
                }
            }
            //foreach (var ability in abilities)
            for(int i=0;i<abilities.Count;i++)
            {
                abilities[i].Update(gametime);
                if (abilities[i].ghost)
                {
                    abilities.RemoveAt(i);
                }
            }
        }
    }
}
