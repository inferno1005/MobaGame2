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
    [Serializable]
    class Nexus:GameEntity
    {
        public Nexus()
        {
            this.height = 256;
            this.width = 256;
            this.attribute.maxhealth = 1000;
            this.attribute.health = 1000;
            this.attribute.visionrange = 2000;
            this.textureindex = 6;
            this.attribute.healthRegen = 0;
        }

        public void Draw(SpriteBatch spriteBatch,Color color)
        {
            if (this.attribute.visible)
            {
                spriteBatch.Draw(UI.textures[this.textureindex], this.rect, color);
                attribute.Draw(spriteBatch, this.position);
            }
        }
    }
}
