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
    class Texture
    {
        //drawing
        public float rotation = 0;        //rotation to draw this texture in
        public Texture2D texture;         //default texture  for this object
        public string name;        //texture name for this object
        public Color color = Color.White; //default color for this object, usually white
        public int height;
        public int width;


        public Rectangle rect
        { get { return new Rectangle(0 ,0 ,width,height); } }

        public void CalcRot(Vector2 direction)
        {
            rotation= (float)Math.Atan2(direction.Y, direction.X);
        }
    }
}
