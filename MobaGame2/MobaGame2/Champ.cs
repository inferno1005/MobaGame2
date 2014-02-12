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


            //rotation %= (float)Math.PI * 2 ;
            spriteBatch.Draw(this.texture, this.rect, null,color,rotation,this.center,SpriteEffects.None,0);
            //spriteBatch.Draw(this.texture,this.rect,color);
            attributes.Draw(spriteBatch, this.position);
        }
    }
}
