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
    class Tower:NewChamp
    {
        public Tower(Map m,List<Ability> ga): base(m,ga)
        {
            this.Name = "Tower";
            this.texturename = "texture\\tower";
            this.attribute.range = 500;


            this.height = 132;
            this.width = 132;
            this.position = new Vector2(10, 10);
            this.attribute.speed = 1;
            this.attribute.visionrange = 900;
            this.attribute.attackDamage=10;
            this.attribute.maxhealth = 500;
            this.attribute.health= 500;


            this.abilities.Add(new Ability());
            this.abilities[0].name = "Basic Attack";
            this.abilities[0].attribute.range = this.attribute.range;
            this.abilities[0].physicalDamage = this.attribute.attackDamage;
            this.abilities[0].magicDamage = 0;
            this.abilities[0].coolDown = 3;
            this.abilities[0].texturename = "texture\\fireball";
 
        }

        public void Agro(GameEntity target)
        {
            if (this.attribute.range > (distance = Vector2.Distance(this.position, target.position)))
            {
                FocusObject(target);
            }
        }
    }
}
