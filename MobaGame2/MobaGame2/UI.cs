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
        static int barwidth;

        //menu
        public static bool escMenuOpen = false;
        static Vector2 menupos;
        static Vector2 menusize;
        //mouse
        public static Texture2D mouseTexture;



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



            //draw near the percent of each bar to look nice
            if (healthbarpercent > 100)
                spritebatch.DrawString(font, ((int)(player.champ.attribute.Health)).ToString(), new Vector2(healthbarpercent + healthpos.X -55, healthpos.Y - 3), Color.White);
            else
                spritebatch.DrawString(font, ((int)(player.champ.attribute.Health)).ToString(), new Vector2(healthpos.X, healthpos.Y - 3), Color.White);

            if(manabarpercent>100)
                spritebatch.DrawString(font, ((int)player.champ.attribute.mana).ToString(), new Vector2(manabarpercent + manapos.X - 55, manapos.Y - 3), Color.White);
            else
                spritebatch.DrawString(font, ((int)player.champ.attribute.mana).ToString(), new Vector2( manapos.X, manapos.Y - 3), Color.White);

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


        public static void DrawTitleScreen(SpriteBatch spriteBatch,SpriteFont font)
        {
            spriteBatch.Begin();
            if (SignedInGamer.SignedInGamers.Count == 0)
            {
                spriteBatch.DrawString(font, "no one signed in\n in order to play hit the home button on the keyboard\n then create a new account\n scroll down and create a local account", new Vector2(10, 10), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, "Create New Lobby", new Vector2(10, 10), Color.White);
                spriteBatch.DrawString(font, "Find a game", new Vector2(10, 50), Color.White);
            }
            spriteBatch.Draw(mouseTexture, Input.MousePosition - new Vector2(5, 5), Color.White);

            spriteBatch.End();
        }
        public static void HandleTitleScreenInput(Vector2 mouse)
        {
            //if create session
            if(MathHelper.ClickedOn(mouse,new Rectangle(10,10,100,20)))
            {
                Networking.CreateSession();
            }
            //if finding session
            if(MathHelper.ClickedOn(mouse,new Rectangle(10,50,100,20)))
            {
                Networking.availableSessions =
                    NetworkSession.Find(NetworkSessionType.SystemLink, 1, null);

                Networking.selectedSessionIndex = 0;
            }
        }

        public static void DrawLobby(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font,"LOBBY",new Vector2(10,10),Color.White);

            int y = 100;

            foreach (NetworkGamer gamer in Networking.networkSession.AllGamers)
            {
                string text = gamer.Gamertag;

                Player player = gamer.Tag as Player;

                //picture stuff

                if (gamer.IsReady)
                    text += " - ready";

                spriteBatch.DrawString(font, text, new Vector2(10, y), Color.White);


                y += 100;
            }
            spriteBatch.End();



        }

        public static void HandleLobbyInput()
        {
            //if pressed
            {
                foreach (LocalNetworkGamer gamer in Networking.networkSession.LocalGamers)
                {
                    gamer.IsReady = true;
                }
            }

            //if esc or back button
            {
                Networking.networkSession = null;
                Networking.availableSessions = null;
            }

            //if everyone is ready start game!
            if(Networking.networkSession.IsHost)
            {
                if (Networking.networkSession.IsEveryoneReady)
                    Networking.networkSession.StartGame();
            }

            //pump the underlying seesion object
            Networking.networkSession.Update();
        }

    }
}
