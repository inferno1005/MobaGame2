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

    public class GameEntity
    {
        public Vector2 position;        //current position
        public Vector2 direction;       //direction from position to destination
        public Vector2 destination;     //current destination
        public float speed;             //how fast
        public double distance;         //distance from poisition to destination
        public int height; 
        public int width;
        public Texture2D texture;
        public string texturename;
        public bool visible;
        public bool clickable;
        public Rectangle rect
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height); }
        }


        public Vector2 Move(Vector2 mouseloc)
        {
            destination = mouseloc;

            Vector2 direction =mouseloc- this.position;

            distance=Math.Sqrt( Math.Pow((this.position.X-mouseloc.X),2)+ Math.Pow((this.position.Y-mouseloc.Y),2));

            if (direction == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(direction);
        }
        public void Update()
        {
            if (distance > 0)
            {
                distance--;
                position += speed * direction;
            }
        }



    }
    public class Camera
    {
        public Vector2 position;
        public int height;
        public int width;
    }
    public class Map : GameEntity
    {
        public Map()
        {
            this.clickable = true;
            this.height = 300;
            this.width= 300;
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


    }
    public class FiddleSticks : Champ
    {
        public FiddleSticks()
        {
            this.Name = "Fiddle Sticks";
            this.texturename = "FiddlesticksSquare";
            this.speed = 1;
            this.height = 64;
            this.width= 64;
        }
    }
    public class Player
    {
        public string name;
        public int kills;
        public int deaths;
        public int assists;
        public Champ champ;
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        Player player1;
        Map map;

        //controls
        MouseState ms;
        KeyboardState kb;
        SpriteFont font1;

        //buffer
        String buffer;

        bool rightbuttonpressed;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player1 = new Player();
            player1.champ = new FiddleSticks();
            camera = new Camera();
            map = new Map();


            //font


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            player1.champ.texture = Content.Load<Texture2D>(player1.champ.texturename);

            font1 = Content.Load<SpriteFont>("DefaultFont");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            ms = Mouse.GetState();
            kb=Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            if (ms.RightButton == ButtonState.Pressed)
            {
                player1.champ.direction=player1.champ.Move(new Vector2(ms.X, ms.Y));
                buffer = "Right Button Pressed!";
                rightbuttonpressed = true;
            }
            if (ms.RightButton == ButtonState.Released)
            {
                buffer = "";
                rightbuttonpressed = false;
            }


            player1.champ.Update();


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);



            // TODO: Add your drawing code here

            //draw map
            spriteBatch.Begin();
            spriteBatch.Draw(player1.champ.texture, map.rect, Color.White);
            spriteBatch.End();




            //draw champ
            spriteBatch.Begin();
            spriteBatch.Draw(player1.champ.texture, player1.champ.rect, Color.White);
            spriteBatch.End();




            #region debug
            spriteBatch.Begin();
            spriteBatch.DrawString(font1,(camera.position.X-ms.X).ToString() + " " +(camera.position.Y-ms.Y).ToString() + buffer,new Vector2(0,0),Color.White);
            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
