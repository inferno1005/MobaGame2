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
            this.texturename = "FiddlesticksSquare";
            this.speed = 5;
            this.height = 48;
            this.width = 48;
            this.range = 250;
            this.position = new Vector2(290, 290);

            this.attributes.maxhealth = 1000;
            this.attributes.health = 50;
        }
    
    }
}
