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
    class Map :GameEntity
    {
        public Map()
        {
            this.position = new Vector2(30, 30);
            this.clickable = true;
            this.height = 800;
            this.width = 1300;
            texturename = "seamless_ground";
        }
    }
}
