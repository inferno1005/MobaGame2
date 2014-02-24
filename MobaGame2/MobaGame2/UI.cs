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
using Microsoft.Xna.Framework.Net;



namespace MobaGame2
{
    static class UI
    {
        //health and mana bars
        static Vector2 healthpos;
        static Vector2 manapos;
        static Vector2 healthStringPos;
        static Vector2 manaStringPos;
        static Vector2 abilityPos;
        static int barwidth;

        //menu
        public static bool escMenuOpen = false;
        static Vector2 menupos;
        static Vector2 menusize;

        //textures
        public static Texture2D mouseTexture;
        public static Texture2D background;
        public static List<Texture2D> ChampIcons;

        //size of ui
        private static int width;
        private static int height;


        static public void SetPos(int w,int h)
        {

            width=w;
            height=h;

            healthpos=new Vector2(300,height-50-100);
            manapos=new Vector2(300,height-25-100);


            abilityPos=new Vector2(300,height-100);


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

            #region bars
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



            //draw near the percent of each bar to look nice
            if (healthbarpercent > 100)
                spritebatch.DrawString(font, ((int)(player.champ.attribute.Health)).ToString(), new Vector2(healthbarpercent + healthpos.X -55, healthpos.Y - 3), Color.White);
            else
                spritebatch.DrawString(font, ((int)(player.champ.attribute.Health)).ToString(), new Vector2(healthpos.X, healthpos.Y - 3), Color.White);

            if(manabarpercent>100)
                spritebatch.DrawString(font, ((int)player.champ.attribute.mana).ToString(), new Vector2(manabarpercent + manapos.X - 55, manapos.Y - 3), Color.White);
            else
                spritebatch.DrawString(font, ((int)player.champ.attribute.mana).ToString(), new Vector2( manapos.X, manapos.Y - 3), Color.White);
            #endregion

            Color color;

            abilityPos=new Vector2(300,height-100);
            #region abilities
            //for(int i=0;i<player.champ.abilities.Count;i++)
            for(int i=0;i<player.champ.abilities.Count;i++)
            {
                if (player.champ.abilities[i].cast || !player.champ.attribute.alive)
                    color = Color.Gray;
                else
                {
                    color = Color.White;
                }


                spritebatch.Draw(player.champ.abilities[i].icon,
                new Rectangle(
                    (int)abilityPos.X + (int)(i * player.champ.abilities[i].iconSize.X),
                    (int)abilityPos.Y,
                     (int)player.champ.abilities[i].iconSize.X,
                     (int)player.champ.abilities[i].iconSize.Y),
                    color);
            abilityPos.X += 2;
            }

            #endregion
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


        public static void DrawTitleScreen(SpriteBatch spriteBatch,SpriteFont font,Networking networking)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0,0,width,height), Color.White);
            if (SignedInGamer.SignedInGamers.Count == 0)
            {
                networking.SignIn();
            }
            else
            {
                spriteBatch.DrawString(font, "Create New Lobby", new Vector2(width-530, 200), Color.White);
                spriteBatch.DrawString(font, "Find a game", new Vector2(width-530, 250), Color.White);
            }
            spriteBatch.Draw(mouseTexture, Input.MousePosition - new Vector2(5, 5), Color.White);

            spriteBatch.End();
        }


        public static void DrawLobby(SpriteBatch spriteBatch, SpriteFont font,Networking networking)
        {
            Console.WriteLine("In draw lobby");
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0,0,width,height), Color.White);

            spriteBatch.DrawString(font, "Lobby", new Vector2(width - 530, 200), Color.White);

            int y = 0;

            foreach (NetworkGamer gamer in networking.networkSession.AllGamers)
            {
                string text = gamer.Gamertag;

                Player player = gamer.Tag as Player;

                //picture stuff
                DisplayChampSelect(spriteBatch, font,40, 40);


                if (gamer.IsReady)
                    text += " - ready";

                spriteBatch.DrawString(font, text, new Vector2(width - 530, y+300), Color.White);


                y += 100;
            }

            spriteBatch.DrawString(font, "Ready", new Vector2(width - 430,  200), Color.White);

            spriteBatch.Draw(mouseTexture, Input.MousePosition - new Vector2(5, 5), Color.White);
            spriteBatch.End();
        }


        public static void DrawAvailableSessions(SpriteBatch spriteBatch, SpriteFont font,Networking networking)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0,0,width,height), Color.White);
            spriteBatch.DrawString(font, "Available Sessions", new Vector2(width - 530, 200), Color.White);

            //int selectedSessionIndex = 0;
            for (int i = 0,y=100; i < networking.availableSessions.Count; i++,y+=100)
            {
                spriteBatch.DrawString(font, networking.availableSessions[i].HostGamertag, new Vector2(width-530, y+300), Color.White);
            }
            spriteBatch.Draw(mouseTexture, Input.MousePosition - new Vector2(5, 5), Color.White);
            spriteBatch.End();
        }

        private static void DisplayChampSelect(SpriteBatch spriteBatch,SpriteFont font, int x,int y)
        {
            int lastwidth=0;
            int lastheight=0;



            //spriteBatch.Begin();
            for (int i = 0; i < ChampIcons.Count; i++)
            {
                spriteBatch.Draw(ChampIcons[i], new Rectangle(x + lastwidth, y + lastheight, 50, 50), Color.White);
                spriteBatch.DrawString(font,i.ToString(), new Vector2(x + lastwidth, y + lastheight), Color.White);
                if ((i+1) % 10 == 0) 
                {
                    lastwidth = 0;
                    lastheight += 50;
                }
                else
                {
                    lastwidth += 50;
                }
            }
            //spriteBatch.End();
        }

    }
}
