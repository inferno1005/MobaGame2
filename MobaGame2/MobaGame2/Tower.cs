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
    [Serializable]
    class Tower:NewChamp
    {
        public Tower(Map m,List<Ability> ga): base(m,ga)
        {
            this.Name = "Tower";
            //this.texturename = "texture\\tower";
            this.attribute.range = 300;
            this.textureindex = 7;


            this.height = 132;
            this.width = 132;
            //this.position = new Vector2(100, 100);
            this.attribute.speed = 0;
            this.attribute.visionrange = 500;
            this.attribute.attackDamage=100;
            this.attribute.maxhealth = 500;
            this.attribute.health= 500;


            this.abilities.Add(new Ability());
            //this.abilities[0].name = "Basic Attack";
            this.abilities[0].attribute.range = this.attribute.range;
            this.abilities[0].physicalDamage = this.attribute.attackDamage;
            this.abilities[0].magicDamage = 0;
            this.abilities[0].coolDown = 1;
            //this.abilities[0].texturename = "texture\\fireball";
 
        }

        public void Updater(Rectangle rect, GameTime gametime)
        {

            if (this.focus != null && !this.focus.attribute.alive)
            {
                FocusObject(null);
            }

            this.Update(rect);
            attribute.Update();
            BasicAttack();

            if (this.attribute.health < 0)
            {
                Console.WriteLine("tower is dead!");
                this.attribute.alive = false;
                this.attribute.clickable = false;
                this.attribute.visible= false;
            }
            foreach (var ability in abilities)
            {
                ability.Update(gametime);
            }
        }

        public void BasicAttack()
        {
            if (this.focus != null &&        //if focused on object
                this.focus.Distance(this.position)< this.attribute.range && //if in range
                !this.abilities[0].cast) //if timer is good
            {
                this.abilities[0].cast = true;
                this.abilities[0].position= this.position;
                this.abilities[0].focus= this.focus;
                this.abilities[0].attribute.visible = true;
                Ability temp = new Ability(this.abilities[0]); 
                temp.position = this.position;
                temp.focus = this.focus;


                globalabilities.Add(temp);
            }
        }
        public void Agro(GameEntity target)
        {
            if (this.attribute.range > (distance = Vector2.Distance(this.position, target.position)))
            {
                if(this.attribute.team!=target.attribute.team)
                    FocusObject(target);
            }
        }
    }
}
