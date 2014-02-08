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

        public static bool RightMouseButton()
        { return lastmousesState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released; } 

        public static bool LeftMouseButton()
        { return lastmousesState.LeftButton== ButtonState.Pressed && mouseState.LeftButton== ButtonState.Released; }
    }
}
