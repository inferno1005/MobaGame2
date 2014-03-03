﻿using System;
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
    class Input
    {

        private static KeyboardState keyboardState, lastkeyboardState;
        private static MouseState mouseState, lastmousesState;

        public static Vector2 MousePosition
        { get { return new Vector2(mouseState.X, mouseState.Y); } }

        public static void Update()
        {
            lastkeyboardState = keyboardState;
            lastmousesState = mouseState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }

        public static bool KeyPressed(Keys key)
        { return lastkeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key); }

        public static bool KeyHeld(Keys key)
        { return keyboardState.IsKeyDown(key); }

        public static bool RightMouseButton()
        { return lastmousesState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released; }

        public static bool LeftMouseButton()
        { return lastmousesState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released; }

        public static bool HandleTitleScreenInput(LidgrenNetwork networking, bool startgame)
        {
            //if create session
            if (LeftMouseButton())
            {
                if (MathHelper.ClickedOn(MousePosition, new Rectangle(1280 - 530, 200, 400, 20)))
                {
                    networking.HostGame();
                }
                //if finding session
                if (MathHelper.ClickedOn(MousePosition, new Rectangle(1280 - 530, 250, 100, 20)))
                {
                    //networking.ConnectToHost();
                    //networking.FindGame();
                    networking.ConnectToClint();
                }
            }
            if (KeyPressed(Keys.Escape))
            {
                //need to return true to tell game to close
                return true;
            }
            return false;
        }

        public static void HandleLobbyInput(LidgrenNetwork networking,int playerindex,GameState gamestate)
        {
            //if pressed
            if (LeftMouseButton())
            {
                //if clicking start game
                if (MathHelper.ClickedOn(MousePosition, new Rectangle(1280 - 230, 200, 100, 20)))
                {
                    Console.WriteLine("clicked on start game");
                    if (networking.isServer)
                    {
                        networking.SendObject("start game");
                        networking.GameIsRunning = true;
                    }
                }

                //if clicking join team 1
                if (MathHelper.ClickedOn(MousePosition, new Rectangle(1280 - 530, 350, 100, 20)))
                {
                    Console.WriteLine("joined team 1");
                    gamestate.players[playerindex].champ.attribute.team = false;
                }
                //if clicking join team 2
                if (MathHelper.ClickedOn(MousePosition, new Rectangle(1280 - 230, 350, 100, 20)))
                {
                    Console.WriteLine("joined team 2");
                    gamestate.players[playerindex].champ.attribute.team = true;
                }



            }




            //if esc or back button
            if (KeyPressed(Keys.Escape))
            {
                networking.EndSession();
            }
        }

        public static Champ SelectChamp(int x, int y, List<Champ> champs)
        {
            if (LeftMouseButton())
            {
                //click on the grid and select champ
                //return champs[i];
            }
            return null;

        }

        public static void HandleAvailableSessionsInput(LidgrenNetwork networking)
        {
            //if esc or back button
            if (KeyPressed(Keys.Escape))
            {
                networking.searching = false;
                networking.EndSession();
            }



            if (networking.searching)
                for (int i = 0, y = 100; i < networking.availsessions.Count; i++, y += 100)
                {
                    if (LeftMouseButton())
                        if (MathHelper.ClickedOn(MousePosition, new Rectangle(1280 - 530, y + 200, 100, 20)))
                        {
                            Console.WriteLine("joining server!");
                            networking.ConnectToHost(networking.availsessions[i].ip);
                            networking.searching = false;
                            networking.inLobby = true;
                            //networking.SendObject("inferno1005");
                        }
                }
        }

        public static GameEntity FindUnderMouse(Camera camera, GameState gamestate)
        {
            foreach (var player in gamestate.players)
            {
                if (MathHelper.ClickedOn(Input.MousePosition + camera.position, player.champ.rect))
                {
                    if (gamestate.players[0].champ.attribute.team != player.champ.attribute.team) ;
                    {
                        Console.WriteLine("FOCUSED CHAMP");
                        //focus this player
                        return player.champ;
                    }
                }
            }



            foreach (var minion in gamestate.minions)
            {
                if (MathHelper.ClickedOn(Input.MousePosition + camera.position, minion.rect))
                {
                    if (gamestate.players[0].champ.attribute.team != minion.attribute.team) ;
                    {
                        Console.WriteLine("FOCUSED minion");
                        return minion;
                    }
                }

            }
            foreach (var tower in gamestate.towers)
            {
                if (MathHelper.ClickedOn(Input.MousePosition + camera.position, tower.rect))
                {
                    if (gamestate.players[0].champ.attribute.team != tower.attribute.team) ;
                    {
                        Console.WriteLine("FOCUSED tower");
                        return tower;
                    }
                }
            }
            foreach (var nexus in gamestate.nexuses)
            {
                if (MathHelper.ClickedOn(Input.MousePosition + camera.position, nexus.rect))
                {
                    if (gamestate.players[0].champ.attribute.team != nexus.attribute.team)
                        return nexus;
                }
            }
            return null;
        }

        public static int MenuChoice(Vector2 mouse,Vector2 menupos)
        {
            if( MathHelper.ClickedOn(mouse,new Rectangle( (int)menupos.X + 30, (int)menupos.Y + 30,30,30)))
            {
                return 1;
            }
            else if (MathHelper.ClickedOn(mouse, new Rectangle((int)menupos.X + 30, (int)menupos.Y + 60, 30, 30)))
            {
                return 2;
            }
            return 0;
        }

        /*
        public static string GetKeyboardString()
        {
            string temp="";


            if(KeyPressed(Keys.Enter))
                return temp;
        }
         */
    }
}
