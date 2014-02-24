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
        { return  keyboardState.IsKeyDown(key); }

        public static bool RightMouseButton()
        { return lastmousesState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released; } 

        public static bool LeftMouseButton()
        { return lastmousesState.LeftButton== ButtonState.Pressed && mouseState.LeftButton== ButtonState.Released; }

        public static void HandleTitleScreenInput(Networking networking)
        {
            //if create session
            if (LeftMouseButton())
            {
                if (MathHelper.ClickedOn(MousePosition, new Rectangle(10, 10, 100, 20)))
                {
                    networking.HostGame();
                }
                //if finding session
                if (MathHelper.ClickedOn(MousePosition, new Rectangle(10, 50, 100, 20)))
                {
                    networking.FindGame();
                }
            }
        }

        public static void HandleLobbyInput(Networking networking)
        {

            //if pressed

            if (LeftMouseButton())
                if (MathHelper.ClickedOn(MousePosition, new Rectangle(10, 50, 100, 20)))
                {
                    foreach (LocalNetworkGamer gamer in networking.networkSession.LocalGamers)
                    {
                        gamer.IsReady = !gamer.IsReady;
                    }
                }

            //if esc or back button
            if(KeyPressed(Keys.Escape))
            {
                networking.EndSession();
            }

            //if everyone is ready start game!
            if(networking.networkSession!=null && networking.networkSession.IsHost)
            {
                if (networking.networkSession.IsEveryoneReady)
                    networking.networkSession.StartGame();
            }

            //pump the underlying seesion object
            if(networking.networkSession!=null)
                networking.networkSession.Update();
        }

        public static void HandleAvailableSessionsInput(Networking networking)
        {
            for (int i = 0,y=100; i < networking.availableSessions.Count; i++,y+=100)
            {
                if (MathHelper.ClickedOn(MousePosition, new Rectangle(10, y, 100, 20)))
                {
                    networking.selectedSessionIndex = i;
                    networking.JoinGame();
                }
            }
        }
    }
}
