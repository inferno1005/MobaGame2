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
        public Attributes attribute;
        public Movement position;
        public Texture texture;
     
        //focus
        public GameEntity focus;        //the focus of this object

        //returns a rectangle the shape of this object
        public Rectangle rect
        { get { return new Rectangle((int)position.position.X, (int)position.position.Y, (int)texture.width, (int)texture.height); } }


        public Rectangle visionrect
        { get { return new Rectangle((int)(position.center.X-this.attribute.visionrange), (int)(position.center.Y-this.attribute.visionrange), (int)(this.attribute.visionrange*2), (int)(this.attribute.visionrange*2)); } }


        public GameEntity()
        {
            attribute = new Attributes();
            position= new Movement();
            texture = new Texture();
        }



        public void Update(Rectangle map)
        {
            #region moving and map bounds

            //if focus or within range
            if (null != focus && this.attribute.range < position.distance)
            {
                position.CalcDirection(focus.position.center);
                position.MoveWithinBounds(map,attribute.speed);
            }

            //if moving to an object that we want to hit
            else if (null != focus && position.distance > 0 && this.attribute.range < position.distance)
            {
                position.MoveWithinBounds(map,attribute.speed);
            }

            //if moving to a point on the map
            else if (position.distance > 0 && null == focus)
            {
                position.MoveWithinBounds(map,attribute.speed);
            }

            #endregion

            attribute.Update();

        }

        //ensures this object moves only within the map

        //sets the focus for this object
        public void FocusObject(GameEntity f)
        {
            focus = f;
            if (null != focus)
            {
                 position.CalcDirection(focus.position.center);
            }
        }

    }
}
