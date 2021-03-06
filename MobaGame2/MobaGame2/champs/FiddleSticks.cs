﻿using System;
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
    class FiddleSticks : NewChamp
    {
        public FiddleSticks(Map m,List<Ability> ga) : base(m,ga)
        {
            this.Name = "Fiddle Sticks";
            this.textureindex = 8;
            this.height = 32;
            this.width = 64;
            this.attribute.range = 250;
            this.attribute.visionrange = 550;
            this.attribute.speed = 5;
            this.position = new Vector2(290, 290);

            this.attribute.maxhealth = 1000;
            this.attribute.health = 750;
            this.attribute.healthRegen = .05f;
            this.attribute.manaRegen= .15f;
            this.attribute.attackDamage=30;

            this.attribute.maxmana= 1000;
            this.attribute.mana= 1000;

            #region abilities
            //setup basic stuff for each ability
            for (int i = 0; i < 7; i++)
            {
                this.abilities.Add(new Ability());
                this.abilities[i].attribute.range = this.attribute.range+2;
                this.abilities[i].physicalDamage = this.attribute.attackDamage;
                this.abilities[i].magicDamage = 0;
                this.abilities[i].coolDown = 3;
                this.abilities[i].attribute.visible = false;
                this.abilities[i].manaCost = 100;
            }



            this.abilities[1].attribute.range = this.attribute.range;
            this.abilities[1].physicalDamage = this.attribute.attackDamage;
            this.abilities[1].magicDamage = 0;
            this.abilities[1].coolDown = 3;
            this.abilities[1].attribute.visible = false;
            this.abilities[1].textureindex = 10;

            this.abilities[2].attribute.range = this.attribute.range;
            this.abilities[2].physicalDamage = this.attribute.attackDamage;
            this.abilities[2].magicDamage = 0;
            this.abilities[2].coolDown = 3;
            this.abilities[2].attribute.visible = false;
            this.abilities[2].textureindex = 11;

            this.abilities[3].attribute.range = this.attribute.range;
            this.abilities[3].physicalDamage = this.attribute.attackDamage;
            this.abilities[3].magicDamage = 0;
            this.abilities[3].coolDown = 3;
            this.abilities[3].attribute.visible = false;
            this.abilities[3].textureindex = 12;

            this.abilities[4].attribute.range = this.attribute.range;
            this.abilities[4].physicalDamage = this.attribute.attackDamage;
            this.abilities[4].magicDamage = 0;
            this.abilities[4].coolDown = 3;
            this.abilities[4].attribute.visible = false;
            this.abilities[4].textureindex = 13;

            this.abilities[5].attribute.range = this.attribute.range;
            this.abilities[5].physicalDamage = -100;
            this.abilities[5].magicDamage = 0;
            this.abilities[5].coolDown = 0;
            this.abilities[5].castvisibile= false;
            this.abilities[5].attribute.visible = false; 
            this.abilities[5].manaCost = 0;
            this.abilities[5].textureindex = 14;


            this.abilities[6].attribute.range = this.attribute.range;
            this.abilities[6].physicalDamage = 0;
            this.abilities[6].magicDamage = 0;
            this.abilities[6].coolDown = 0;
            this.abilities[6].castvisibile= false;
            this.abilities[6].attribute.visible = false; 
            this.abilities[6].manaCost = -100;
            this.abilities[6].textureindex = 15;
            #endregion

        }
    
    }
}
