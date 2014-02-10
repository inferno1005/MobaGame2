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
    static class MathHelper
    {
        static public bool Bounds(Rectangle a, Rectangle b)
        {
            if (a.X >= b.X && a.X + a.Width <= b.X + b.Width)
                if (a.Y >= b.Y && a.Y + a.Height <= b.Y + b.Height)
                    return true;
            return false;
        }
        static public bool ClickedOn(Vector2 loc,Rectangle rect)
        {
            if (loc.X > rect.X && loc.X < rect.X + rect.Width)
            {
                if (loc.Y > rect.Y && loc.Y < rect.Y + rect.Height)
                {
                    return true;
                }
            }
            return false;
        }




    }
}
