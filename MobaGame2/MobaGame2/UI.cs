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
        //health and mana bars
        static Vector2 healthpos;
        static Vector2 manapos;
        static Vector2 healthStringPos;
        static Vector2 manaStringPos;
        static int barwidth;

        //menu
        public static bool escMenuOpen = false;
        static Vector2 menupos;
        static Vector2 menusize;



        static public void SetPos(int width,int height)
        {
            healthpos=new Vector2(300,height-50);
            manapos=new Vector2(300,height-25);


            healthStringPos = new Vector2(500, 0) + healthpos;
            manaStringPos= new Vector2(500, 0) + manapos;
            barwidth = width / 2;

            menupos = new Vector2(40,  40);
            menusize = new Vector2(width - 80, height - 80);
        }

        static public void Draw(SpriteBatch spritebatch,SpriteFont font,Player player,Vector2 mousepos)
        {
            int healthbarpercent = (int)(barwidth*((player.champ.attribute.Health / player.champ.attribute.maxhealth)));
            int manabarpercent = (int)(barwidth*((player.champ.attribute.mana/ player.champ.attribute.maxmana)));



            //draw healh bars  
            spritebatch.Draw(player.champ.attribute.texture, new Rectangle((int)healthpos.X, (int)healthpos.Y ,barwidth , 20), Color.Black);
            spritebatch.Draw(player.champ.attribute.texture,
                new Rectangle(
                    (int)healthpos.X,       //x
                    (int)healthpos.Y ,      //y
                    healthbarpercent,       //length
                    20),                    //height
                    Color.Red);             //color

            //draw mana bars  
            spritebatch.Draw(player.champ.attribute.texture, new Rectangle((int)manapos.X, (int)manapos.Y ,barwidth , 20), Color.Black);
            spritebatch.Draw(player.champ.attribute.texture,
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
            spritebatch.DrawString(font,((int)(player.champ.attribute.Health)).ToString(),new Vector2(healthbarpercent+healthpos.X-55,healthpos.Y-3),Color.White);
            spritebatch.DrawString(font,((int)player.champ.attribute.mana).ToString(),new Vector2(manabarpercent+manapos.X-55,manapos.Y-3),Color.White);

            //draw options menu
            if (escMenuOpen)
            {
                spritebatch.Draw(player.champ.attribute.texture, new Rectangle((int)menupos.X, (int)menupos.Y, (int)menusize.X, (int)menusize.Y), Color.Black);
                spritebatch.DrawString(font, "Exit", new Vector2(menupos.X + 30, menupos.Y + 30), Color.White);
            }


        }

        public static int MenuChoice(Vector2 mouse)
        {
            if( MathHelper.ClickedOn(mouse,new Rectangle( (int)menupos.X + 30, (int)menupos.Y + 30,30,30)))
            {
                return 1;
            }
            return 0;
        }

    }
}
