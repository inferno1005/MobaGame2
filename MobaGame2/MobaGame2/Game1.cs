
/*
 *  Todo 
 *   research gametime so the game is consistent
 *   add targeting game ents 
 */



using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;




namespace MobaGame2
{

    static class Input
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
        {
            return lastkeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);
        }
        public static bool RightMouseButton()
        {
            return lastmousesState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released;
        }

        public static bool LeftMouseButton()
        {
            return lastmousesState.LeftButton== ButtonState.Pressed && mouseState.LeftButton== ButtonState.Released;
        }
    }

    public class GameEntity
    {
        public Vector2 position;        //current position
        public Vector2 center           //center of the icon
        { get { return new Vector2(position.X + width / (float)2, position.Y + height / (float)2); } }

        public Vector2 direction;       //direction from position to destination
        public Vector2 destination;     //current destination
        public float speed;             //how fast
        public double distance;         //distance from poisition to destination

        public int height; 
        public int width;

        //drawing
        public Texture2D texture;
        public string texturename;
        public Color color;

        //options
        public bool visible;
        public bool clickable;

        //returns a rectangle the shape of this object
        public Rectangle rect
        { get { return new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height); } }

        //Finds the direction the object needs to move to goto the target location
        public Vector2 Move(Vector2 target)
        {
            destination = target;

            //Vector2 direction =mouseloc- new Vector2(this.position.X+(width/(float)2),this.position.Y+(height/(float)2));
            Vector2 direction = target - this.center;

            distance=Math.Sqrt( Math.Pow((this.center.X-target.X),2)+ Math.Pow((this.center.Y-target.Y),2));

            if (direction == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(direction);
        }

        //check to see if a is fully within b
        public bool Bounds(Rectangle a,Rectangle b)
        {
            if (a.X >= b.X && a.X + a.Width <= b.X + b.Width)
                if (a.Y >= b.Y && a.Y + a.Height <= b.Y + b.Height)
                    return true;
            return false;
        }




        public void Update(Rectangle map)
        {
            #region moving and map bounds
            //if we have distance to move
            if (distance > 0)
                //check bounds
                if(Bounds(this.rect,map))
                {
                        distance-=speed;
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
            #endregion 





        }



    }



    public class Camera
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
            {
                position.X += speed;
                Console.WriteLine("moving to right");
            }
            //move left 
            if (mouse.X + position.X < position.X + 4)
            {
                position.X -= speed;
                Console.WriteLine("moving to left");
            }
            //move up
            if (mouse.Y < 4)
            {
                position.Y -= speed;
            }
            //move up
            if (mouse.Y > height - 4)
            {
                position.Y += speed;
            }


        }

        public Vector2 mouseToWorld(Vector2 mouse)
        {

            return new Vector2(0, 0);
        }
    }
    public class Map : GameEntity
    {
        public Map()
        {
            this.position = new Vector2(30, 30);
            this.clickable = true;
            this.height = 800;
            this.width = 1300;
            texturename = "seamless_ground";
        }
        public bool ClickWitinMap(Vector2 loc)
        {
            if (loc.X > position.X && loc.X < position.X + width)
                if (loc.Y > position.Y && loc.Y < position.Y + height)
                    return true;
            return false;
        }


    }
    public class Attributes
    {
        public double health;
        public double mana;
        public double healthRegen;
        public double manaRegen;
        public double range;
        public double coolDown;
        public double armor;
        public double magicResist;
        public double attackDamage;
        public double attackSpeed;
        public int gold;
        public int goldpersec;
    }
    public class Champ : GameEntity
    {
        public string Name;
        public Attribute attribute;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.rect, Color.White);
        }

    }
    public class Minion : Champ
    {
        public Minion()
        {
            this.Name = "Minion";
            this.texturename = "Minion";
            this.speed = 1;
            this.height = 30;
            this.width = 30;
            this.position = new Vector2(80, 80);
        }
    }
    public class FiddleSticks : Champ
    {
        public FiddleSticks()
        {
            this.Name = "Fiddle Sticks";
            this.texturename = "FiddlesticksSquare";
            this.speed = 5;
            this.height = 48;
            this.width = 48;
            this.position = new Vector2(40, 40);
        }
    }
    public class Player
    {
        public string name;
        public int kills;
        public int deaths;
        public int assists;
        public Champ champ;

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            //player name
            spritebatch.DrawString(font, this.name, this.champ.position - new Vector2(0, 30), Color.White);
            //draw champ
            champ.Draw(spritebatch);
        }
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //needed
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //screen
        int SCREENHEIGHT = 720;
        int SCREENWIDTH = 1280;


        //game objects
        Camera camera;
        Player player1;
        Minion minion;

        Map map;

        //controls


        SpriteFont font1;
        Texture2D mouseTexture;

        //buffer
        String buffer;

        bool rightbuttonpressed;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            #region graphics setup
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;
            graphics.PreferredBackBufferWidth = SCREENWIDTH;
            graphics.PreferMultiSampling = false;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            this.Window.Title = "Moba";
            #endregion



            // TODO: Add your initialization logic here
            player1 = new Player();
            player1.name = "inferno1005";
            player1.champ = new FiddleSticks();


            minion = new Minion();

            camera = new Camera(new Vector2(0, 0), SCREENHEIGHT, SCREENWIDTH, 8);

            map = new Map();



            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            #region textures
            player1.champ.texture = Content.Load<Texture2D>(player1.champ.texturename);
            minion.texture = Content.Load<Texture2D>(minion.texturename);
            map.texture = Content.Load<Texture2D>(map.texturename);
            mouseTexture = Content.Load<Texture2D>("pointer");
            #endregion

            #region fonts
            font1 = Content.Load<SpriteFont>("DefaultFont");
            #endregion

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();


            #region controls
            if (Input.RightMouseButton())
            {
                if (map.ClickWitinMap(Input.MousePosition - camera.position))
                {
                    player1.champ.direction = player1.champ.Move(Input.MousePosition + camera.position);
                    buffer = "Right Button Pressed!";
                    rightbuttonpressed = true;
                }
            }
            if (Input.RightMouseButton())
            {
                buffer = "";
                rightbuttonpressed = false;
            }
            #endregion


            #region camera
            /*
            if(Input.KeyPressed(Keys.Down))
            {
                camera.zoom+=.05f;
            }
            if(Input.KeyPressed(Keys.Up))
            {
                camera.zoom-=.05f;
            }
             */

            camera.ScreenBorderMove(Input.MousePosition);


            #endregion

            //
            player1.champ.Update(map.rect);


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //draw based off camera location
            spriteBatch.Begin(
                //SpriteSortMode.BackToFront,
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                //camera.calc_transformation(SCREENHEIGHT,SCREENWIDTH)
                camera.calc_transformation(1, 1)
                );
            //draw map
            spriteBatch.Draw(map.texture, map.rect, Color.White);

            //spriteBatch.End();



            player1.Draw(spriteBatch, font1);

            //draw minion 
            minion.Draw(spriteBatch);

            spriteBatch.End();


            spriteBatch.Begin();

            //draw pointer to be drawn last so its over top everything
            spriteBatch.Draw(mouseTexture, Input.MousePosition - new Vector2(5, 5), Color.White);



            spriteBatch.End();


            #region debug
            /*
            //spriteBatch.Begin();
            spriteBatch.DrawString(font1, (camera.position.X - (Input.MousePosition).X).ToString() + " " + (camera.position.Y - (Input.MousePosition).Y).ToString() + buffer, new Vector2(0, 0), Color.White);
            //spriteBatch.DrawString(font1, player1.champ.position.X.ToString() + " " + player1.champ.position.Y.ToString(), player1.champ.position-new Vector2(0,80), Color.White);
            spriteBatch.DrawString(font1,
                (player1.champ.position.X ).ToString() + " " + (  player1.champ.position.Y).ToString(),
                (player1.champ.position- new Vector2(0, 80)), Color.White);
            spriteBatch.End();
             */
            #endregion

            base.Draw(gameTime);
        }
    }
}
