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
    class Bush :GameEntity
    {
        private int textureheight;
        private int texturewidth;

        public Bush()
        {
            this.height = 100;
            this.width = 300;
            this.texturewidth = 1536;
            this.textureheight= 100;
            this.textureindex = 5;
        }

        public void Draw(SpriteBatch spriteBatch,Color color)
        {
            if (this.attribute.visible)
            {
                spriteBatch.Draw(UI.textures[textureindex], this.rect,  color);
                attribute.Draw(spriteBatch, this.position);
            }
        }

    }
}
