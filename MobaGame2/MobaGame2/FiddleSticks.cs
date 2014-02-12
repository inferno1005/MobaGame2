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
    class FiddleSticks : Champ
    {
        public FiddleSticks()
        {
            this.Name = "Fiddle Sticks";
            this.texturename = "Fiddle-Pumpkin";
            this.speed = 5;
            this.height = 32;
            this.width = 64;
            this.range = 250;
            this.visionrange = 450;
            this.position = new Vector2(290, 290);

            this.attributes.maxhealth = 1000;
            this.attributes.health = 750;


            this.attributes.maxmana= 1000;
            this.attributes.mana= 1000;
        }
    
    }
}
