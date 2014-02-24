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
    class Movement
    {
        public Vector2 position;        //current position
        public Vector2 center           //center of the icon
        { get { return new Vector2(position.X + (width / (float)2), position.Y + (height / (float)2)); } }

        public Vector2 direction;       //direction from position to destination
        public Vector2 destination;     //current destination
        public double distance;         //distance from poisition to destination

        public int height;              //height of this object
        public int width;               //width of this object
        public Rectangle rect
        { get { return new Rectangle((int)position.X ,(int)position.Y ,width,height); } }

        //Finds the direction the object needs to move to goto the target location
        public void CalcDirection(Vector2 target)
        {
            destination = target;

            Vector2 direction = target - this.center;

            distance = Vector2.Distance(this.center, target);

            if (Vector2.Zero == direction)
            {
                direction=Vector2.Zero;
            }
            else
            {
                direction=Vector2.Normalize(direction);
            }
        }

        //finds the distance from this object to its set destination
        public double Distance()
        {
                return Vector2.Distance(destination, this.position);
        }

        public void Move(Vector2 target,double speed)
        {
            CalcDirection(target);
            distance -= speed;
            position += ((float)(speed) * direction);

        }



        public void MoveWithinBounds(Rectangle map,double speed)
        {
            //check bounds
            if (MathHelper.Bounds(this.rect, map))
            {
                //Console.WriteLine("trying to move!");
                distance -= speed;
                position += ((float)(speed )* direction);
            }
            //if not push him back in the map
            else
            {
                if ((position.Y + height) >= (map.Y + map.Height))
                {
                    position.Y = map.Height + map.Y - height-1;
                }
                if (position.Y <= map.Y)
                {
                    position.Y = map.Y+1;
                }
                if ((position.X + width) >= (map.X + map.Width))
                {
                    position.X = map.Width + map.X - width-1;
                }
                if (position.X <= map.X)
                {
                    position.X = map.X+1;
                }
            }

        }

    }
}
