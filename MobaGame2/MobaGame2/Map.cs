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
    class Map :GameEntity
    {

        public int textureheight = 512;
        public int texturewidth= 512;


        public Map()
        {
            this.position.position = new Vector2(30, 30);
            this.attribute.clickable = true;

            this.texture.height = 800;
            this.texture.width = 5000;
            this.texture.name= "texture\\seamless_ground";
        }
    }
}
