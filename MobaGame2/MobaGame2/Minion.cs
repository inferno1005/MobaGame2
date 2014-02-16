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
    class Minion : Champ
    {
        public Minion()
        {
            this.Name = "Minion";
            this.texturename = "texture\\Minion";
            this.height = 32;
            this.width = 32;
            this.position = new Vector2(10, 10);
            this.attribute.range = 200;
            this.attribute.speed = 1;
            this.attribute.visionrange = 200;

            this.abilities.Add(new Ability());
            this.abilities[0].name = "Basic Attack";
            this.abilities[0].attribute.range = this.attribute.range;
            //this.abilities[0].physicalDamage = this.attributes.attackDamage;
            this.abilities[0].physicalDamage = 1000;
            this.abilities[0].magicDamage = 0;
            this.abilities[0].coolDown = 1;





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
