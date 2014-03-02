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

        public Bush()
        {
            this.texturename = "texture\\bush";
            this.height = 100;
            this.width = 300;
        }

        public void Draw(SpriteBatch spriteBatch,Color color)
        {
            if (this.attribute.visible)
            {
                /*
                //so slow ;-;
                this.rotation %= (float)Math.PI * 2 ;
                this.rotation += (float)Math.PI / 2;

                spriteBatch.Draw(this.texture,
                    new Rectangle((int)this.center.X,(int)this.center.Y,(int)this.width,(int)this.height),
                    null,
                    this.color,
                    this.rotation,
                    new Vector2(this.width,this.height),
                    SpriteEffects.None,0);
                 */


                spriteBatch.Draw(this.texture, this.rect, color);
                attribute.Draw(spriteBatch, this.position);
            }
        }

    }
}
