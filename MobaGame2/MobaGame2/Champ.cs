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
    class Champ :GameEntity
    {
        public string Name;
        public Attributes attributes;

        public Champ()
        {
            attributes = new Attributes();
        }



        public void Draw(SpriteBatch spriteBatch,Color color)
        {
            //Console.WriteLine(this.Name + this.rect + " " + this.position);


            //so slow ;-;
            rotation %= (float)Math.PI * 2 ;
            rotation += (float)Math.PI / 2;

            spriteBatch.Draw(this.texture,
                new Rectangle((int)this.position.X+width/2,(int)this.position.Y+height/2,width,height),
                null,
                color,
                this.rotation,
                new Vector2(this.width/2,this.height/2),
                SpriteEffects.None,0);
            spriteBatch.Draw(this.texture,this.rect,Color.Black);
            attributes.Draw(spriteBatch, this.position);
        }
    }
}
