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
    class FiddleSticks : NewChamp
    {
        public FiddleSticks(Map m,List<Ability> ga) : base(m,ga)
        {
            this.Name = "Fiddle Sticks";
            this.texturename = "texture\\Fiddle-Pumpkin";
            //this.texturename = "FiddleSticksSquare";
            this.height = 32;
            this.width = 64;
            this.attribute.range = 250;
            this.attribute.visionrange = 450;
            this.attribute.speed = 5;
            this.position = new Vector2(290, 290);

            this.attribute.maxhealth = 1000;
            this.attribute.health = 750;
            this.attribute.healthRegen = .05f;
            this.attribute.manaRegen= .15f;
            this.attribute.attackDamage=30;

            for (int i = 0; i < 7; i++)
            {
                this.abilities.Add(new Ability());
                this.abilities[i].name = "Basic Attack";
                this.abilities[i].attribute.range = this.attribute.range;
                this.abilities[i].physicalDamage = this.attribute.attackDamage;
                this.abilities[i].magicDamage = 0;
                this.abilities[i].coolDown = 3;
                this.abilities[i].attribute.visible = false;


                this.abilities[0].texturename = "texture\\fireball";
                this.abilities[0].iconname = "texture\\fireball";
            }

            //this.abilities[0].attribute.speed= 10;

            this.attribute.maxmana= 1000;
            this.attribute.mana= 1000;
        }
    
    }
}
