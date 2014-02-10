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
    class GameEntity
    {
        public Vector2 position;        //current position
        public Vector2 center           //center of the icon
        { get { return new Vector2(position.X + (width / (float)2), position.Y + (height / (float)2)); } }

        public Vector2 direction;       //direction from position to destination
        public Vector2 destination;     //current destination
        public float speed;             //how fast
        public double distance;         //distance from poisition to destination
        public double range;            //how far the object needs to be to agro

        public int height;              //height of this object
        public int width;               //width of this object

        //drawing
        public Texture2D texture;       //default texture  for this object
        public string texturename;      //texture name for this object
        public Color color=Color.White; //default color for this object, usually white

        //options
        public bool visible;            //whether this object should be drawn
        public bool clickable;          //whether this object can be clicked on

        //focus
        public GameEntity focus;        //the focus of this object

        //returns a rectangle the shape of this object
        public Rectangle rect
        { get { return new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height); } }


        //Finds the direction the object needs to move to goto the target location
        public Vector2 CalcDirection(Vector2 target)
        {
            destination = target;

            Vector2 direction = target - this.center;

            distance = Vector2.Distance(this.center, target);

            if (Vector2.Zero == direction)
            {
                return Vector2.Zero;
            }
            else
            {
                return Vector2.Normalize(direction);
            }
        }

        //finds the distance from this object to its set destination
        public double Distance()
        {
                return Vector2.Distance(destination, this.position);
        }

        public void Update(Rectangle map)
        {
            #region moving and map bounds

            //if focus or within range
            if (null != focus && range < distance)
            {
                direction = CalcDirection(focus.center);
                MoveWithinBounds(map);
            }

            //if moving to an object that we want to hit
            else if (null != focus && distance > 0 && range < distance)
            {
                MoveWithinBounds(map);
            }

            //if moving to a point on the map
            else if (distance > 0 && null == focus)
            {
                MoveWithinBounds(map);
            }

            #endregion

        }

        //ensures this object moves only within the map
        public void MoveWithinBounds(Rectangle map)
        {

            //check bounds
            if (MathHelper.Bounds(this.rect, map))
            {
                //Console.WriteLine("trying to move!");
                distance -= speed;
                position += (speed * direction);
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


        //sets the focus for this object
        public void FocusObject(GameEntity f)
        {
            focus = f;
            if (null != focus)
            {
                direction = CalcDirection(focus.center);
            }
        }

    }
}
