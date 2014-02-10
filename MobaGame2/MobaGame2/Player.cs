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
    class Player
    {

        public string name;
        public int kills;
        public int deaths;
        public int assists;
        public Champ champ;

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            //player name
            spritebatch.DrawString(font, this.name, this.champ.position - new Vector2(0, 50), Color.White);
            //draw champ
            champ.Draw(spritebatch);
        }
    
    }
}
