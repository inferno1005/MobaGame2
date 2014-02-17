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
    class FiddleSticks : Champ
    {
        public FiddleSticks()
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

            this.abilities.Add(new Ability(this.attribute.attackDamage));
            this.abilities[0].name = "Basic Attack";
            this.abilities[0].attribute.range = this.attribute.range;
            //this.abilities[0].physicalDamage= this.attribute.attackDamage;
            this.abilities[0].magicDamage = 0;
            this.abilities[0].coolDown= 3;
            this.abilities[0].texturename = "texture\\fireball";

            this.attribute.maxmana= 1000;
            this.attribute.mana= 1000;
        }
    
    }
}
