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
        public Minion()
        {
            this.Name = "Minion";
            this.texturename = "Minion";
            this.speed = 1;
            this.height = 32;
            this.width = 32;
            this.position = new Vector2(80, 80);
            this.range = 200;
            this.visionrange = 200;
        }
        public void Agro(GameEntity target)
        {
            if (range > (distance=Vector2.Distance(this.position, target.position)))
            {
                FocusObject(target);
            }
        }
    
    }
}
