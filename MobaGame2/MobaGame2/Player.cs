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
        public NewChamp champ;

        public void Draw(SpriteBatch spritebatch, SpriteFont font,Color color)
        {
            //player name
            spritebatch.DrawString(font, this.name, this.champ.position - new Vector2(0, 50), color);
            //draw champ
            champ.Draw(spritebatch,color);
        }

        public void Update(Rectangle rect,GameTime gametime,List<Ability> abi)
        {
            if (champ.attribute.alive)
            {
                champ.Updater(gametime);
            }
        }

    
    }
}
