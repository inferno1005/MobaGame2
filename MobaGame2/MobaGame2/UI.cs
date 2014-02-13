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
    static class UI
    {
        static Vector2 healthpos;
        static Vector2 manapos;

        static Vector2 healthStringPos;
        static Vector2 manaStringPos;

        static int barwidth;

        static public void SetPos(int width,int height)
        {
            healthpos=new Vector2(300,height-50);
            manapos=new Vector2(300,height-25);


            healthStringPos = new Vector2(500, 0) + healthpos;
            manaStringPos= new Vector2(500, 0) + manapos;
            barwidth = width / 2;
        }

        static public void Draw(SpriteBatch spritebatch,SpriteFont font,Player player)
        {
            int healthbarpercent = (int)(barwidth*((player.champ.attributes.health / player.champ.attributes.maxhealth)));
            int manabarpercent = (int)(barwidth*((player.champ.attributes.mana/ player.champ.attributes.maxmana)));



            //draw healh bars  
            spritebatch.Draw(player.champ.attributes.texture, new Rectangle((int)healthpos.X, (int)healthpos.Y ,barwidth , 20), Color.Black);
            spritebatch.Draw(player.champ.attributes.texture,
                new Rectangle(
                    (int)healthpos.X,       //x
                    (int)healthpos.Y ,      //y
                    healthbarpercent,       //length
                    20),                    //height
                    Color.Red);             //color

            //draw mana bars  
            spritebatch.Draw(player.champ.attributes.texture, new Rectangle((int)manapos.X, (int)manapos.Y ,barwidth , 20), Color.Black);
            spritebatch.Draw(player.champ.attributes.texture,
                new Rectangle(
                    (int)manapos.X,       //x
                    (int)manapos.Y ,      //y
                    manabarpercent,       //length
                    20),                  //height
                    Color.Blue);          //color


            //drawstring health
            //spritebatch.DrawString(font,player.champ.attributes.health.ToString(),healthStringPos ,Color.White);
            //spritebatch.DrawString(font,player.champ.attributes.mana.ToString(), manaStringPos,Color.White);

            //draw near the percent of each bar to look nice
            spritebatch.DrawString(font,((int)(player.champ.attributes.health)).ToString(),new Vector2(healthbarpercent+healthpos.X-55,healthpos.Y-3),Color.White);
            spritebatch.DrawString(font,((int)player.champ.attributes.mana).ToString(),new Vector2(manabarpercent+manapos.X-55,manapos.Y-3),Color.White);


        }

    }
}
