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
    class Camera
    {
        public Vector2 position;
        public Matrix transform;
        public float zoom;
        public float rotation;
        public int height;
        public int width;
        public int speed;

        public Camera(Vector2 pos, int h, int w, int s)
        {
            this.height = h;
            this.width = w;
            this.position = pos;
            this.speed = s;
            this.zoom = 1;
            this.rotation = 0;
        }

        public Matrix calc_transformation(int height, int width)
        {
            transform =
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(width * 0.5f, height * 0.5f, 0));

            return transform;
        }

        public void ScreenBorderMove(Vector2 mouse)
        {
            //move right
            if (mouse.X > (width - 4))
                position.X += speed;
            //move left 
            if (mouse.X + position.X < position.X + 4)
                position.X -= speed;
            //move up
            if (mouse.Y < 4)
                position.Y -= speed;
            //move up
            if (mouse.Y > height - 4)
                position.Y += speed;
        }
    
    }
}
