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
        public double range;

        public int height;
        public int width;

        //drawing
        public Texture2D texture;
        public string texturename;
        public Color color;

        //options
        public bool visible;
        public bool clickable;

        //focus
        public GameEntity focus;

        //returns a rectangle the shape of this object
        public Rectangle rect
        { get { return new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height); } }

        //Finds the direction the object needs to move to goto the target location
        public Vector2 CalcDirection(Vector2 target)
        {
            destination = target;

            //Vector2 direction =mouseloc- new Vector2(this.position.X+(width/(float)2),this.position.Y+(height/(float)2));
            Vector2 direction = target - this.center;

            distance = Math.Sqrt(Math.Pow((this.center.X - target.X), 2) + Math.Pow((this.center.Y - target.Y), 2));

            if (direction == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(direction);
        }

        public double Distance()
        {
            return Vector2.Distance(focus.position, this.position);
        }

        //check to see if a is fully within b
        public bool Bounds(Rectangle a, Rectangle b)
        {
            if (a.X >= b.X && a.X + a.Width <= b.X + b.Width)
                if (a.Y >= b.Y && a.Y + a.Height <= b.Y + b.Height)
                    return true;
            return false;
        }

        public void Update(Rectangle map)
        {
            #region moving and map bounds
            //if no focus or within range
            if (focus != null && range < (distance = this.Distance()))
            {
                direction = CalcDirection(focus.center);
            }


            //if moving to an object that we want to hit
            if (focus != null)
            {
                if (distance > 0 && range < distance)
                {
                    //check bounds
                    if (Bounds(this.rect, map))
                    {
                        distance -= speed;
                        position += speed * direction;
                    }
                    //if not push him back in the map
                    else
                    {
                        if (position.Y + height >= map.Y + map.Height)
                            position.Y = map.Height + map.Y - height;
                        if (position.Y <= map.Y)
                            position.Y = map.Y;
                        if (position.X + width >= map.X + map.Width)
                            position.X = map.Width + map.X - width;
                        if (position.X <= map.X)
                            position.X = map.X;
                    }
                }
            }
            //if moving to a point on the map
            else
            {

                if (distance > 0)
                {
                    //check bounds
                    if (Bounds(this.rect, map))
                    {
                        distance -= speed;
                        position += speed * direction;
                    }
                    //if not push him back in the map
                    else
                    {
                        if (position.Y + height >= map.Y + map.Height)
                            position.Y = map.Height + map.Y - height;
                        if (position.Y <= map.Y)
                            position.Y = map.Y;
                        if (position.X + width >= map.X + map.Width)
                            position.X = map.Width + map.X - width;
                        if (position.X <= map.X)
                            position.X = map.X;
                    }
                }
            }

            #endregion

        }


        public bool ClickedOn(Vector2 loc)
        {
            if (loc.X > position.X && loc.X < position.X + width)
                if (loc.Y > position.Y && loc.Y < position.Y + height)
                    return true;
            return false;
        }


        public void FocusObject(GameEntity f)
        {
            focus = f;
            if (focus != null)
                direction = CalcDirection(f.center);
        }

    }
}
