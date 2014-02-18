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
    class Minion : Champ
    {
        GameEntity enemybase;
        public Minion(GameEntity ge)
        {
            this.Name = "Minion";
            this.texturename = "texture\\Minion";
            this.height = 32;
            this.width = 32;
            this.position = new Vector2(10, 10);
            this.attribute.range = 200;
            this.attribute.speed = 1;
            this.attribute.visionrange = 200;

            this.abilities.Add(new Ability(100));
            this.abilities[0].name = "Basic Attack";
            this.abilities[0].attribute.range = this.attribute.range;
            //this.abilities[0].physicalDamage = this.attributes.attackDamage;
            //this.abilities[0].physicalDamage = 1000f;
            this.abilities[0].magicDamage = 0;
            this.abilities[0].coolDown = 1;
            this.abilities[0].texturename = "texture\\fireball";
            enemybase = ge;





        }
        public void Agro(GameEntity target)
        {
            if (this.attribute.range > (distance = Vector2.Distance(this.position, target.position)))
            {
                FocusObject(target);
            }
        }
        public void Updater(Rectangle rect, GameTime gametime)
        {

            this.Update(rect);
            attribute.Update();

            //attack focused object
            if (focus != null)
            {
                if (focus.distance < this.attribute.range)
                {
                    if (!this.abilities[0].cast)
                    {
                        //Console.WriteLine("SPAWNING AN ABILITY!");
                        this.abilities[0].cast = true;
                        Ability temp = new Ability(this.abilities[0].physicalDamage);
                        temp.texture = this.abilities[0].texture;
                        temp.focus = this.focus;
                        temp.position = this.position;
                        abilities.Add(temp);
                    }
                }
                else
                {
                    this.focus = enemybase;
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
