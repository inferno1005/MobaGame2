
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
        //Camera minimap;

        List<Player> players;
        List<Minion> minions;
        List<GameEntity> entities;

        Map map;

        //controls


        SpriteFont font1;
        Texture2D mouseTexture;

        //buffer
        String buffer;


        Color drawcolor;




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

            //ui
            UI.SetPos(SCREENWIDTH, SCREENHEIGHT);


            // TODO: Add your initialization logic here
            players= new List<Player>();
            players.Add(new Player());
            players[0].name = "inferno1005";
            players[0].champ = new FiddleSticks();

            minions= new List<Minion>();
            minions.Add(new Minion());

            camera = new Camera(new Vector2(0, 0), SCREENHEIGHT, SCREENWIDTH, 8);
            //minimap = new Camera(new Vector2(0, 0), 50, 50, 8);

            //minimap.zoom = .30f;

            map = new Map();



            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region textures
            foreach (var Player in players)
            {
                Player.champ.texture = Content.Load<Texture2D>(Player.champ.texturename);
                Player.champ.attributes.texture = Content.Load<Texture2D>(Player.champ.attributes.texturename);
            }

            foreach (var Minion in minions)
                Minion.texture = Content.Load<Texture2D>(Minion.texturename);

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
            bool foundobject;


            #region controls

            //right button selects and targets, if not on anything but map, moves there
            #region mouse
            if (Input.RightMouseButton())
            {
                foundobject = false;

                //prefer plays over other objects, because they can kill you!
                foreach (var player in players)
                {
                    if (!foundobject)
                    {
                        if (MathHelper.ClickedOn(Input.MousePosition - camera.position,player.champ.rect))
                        {

                            foundobject = true;
                            //focus this player
                        }
                    }
                }


                if(!foundobject)
                    foreach (var minion in minions)
                    {
                        if (!foundobject)
                            if (MathHelper.ClickedOn(Input.MousePosition - camera.position, minion.rect))
                            {
                                Console.WriteLine("Clicked on minion");
                                foundobject = true;
                                players[0].champ.FocusObject(minion);
                            }

                    }


                if (!foundobject)
                    if (MathHelper.ClickedOn(Input.MousePosition - camera.position,map.rect))
                    {
                        Console.WriteLine("Clicked on map!");
                        players[0].champ.direction = players[0].champ.CalcDirection(Input.MousePosition + camera.position);
                        players[0].champ.FocusObject(null);

                    }
            }
            #endregion

            #region keyboard

            //center camera to champ and follow
            if(Input.KeyHeld(Keys.Space))
            {
                camera.Center(players[0].champ.center);
            }
            #endregion


            #endregion

            #region camera
            camera.ScreenBorderMove(Input.MousePosition);
            #endregion

            #region updates

            foreach (var player in players)
            {
                player.Update(map.rect);
            }

            foreach(var minion in minions)
            {
                foreach (var player in players)
                {
                    minion.Agro(player.champ);
                }
                minion.Update(map.rect);
            }
            #endregion

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.Gray);

            //if player is alive draw normaly, otherwise dray grayed out
            if (players[0].alive)
                drawcolor = Color.White;
            else 
                drawcolor = Color.Gray;

            #region game camera
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
            spriteBatch.Draw(map.texture, map.rect, drawcolor);

            //spriteBatch.End();



            foreach(var player in players)
                player.Draw(spriteBatch, font1,drawcolor);

            //draw minion 
            foreach(var minion in minions)
                minion.Draw(spriteBatch,drawcolor);

            spriteBatch.End();
            #endregion

            //
            #region minimap camera
            /*
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
                minimap.calc_transformation(SCREENHEIGHT, SCREENWIDTH)
                );
            //draw map
            spriteBatch.Draw(map.texture, map.rect, Color.White);

            //spriteBatch.End();



            foreach(var player in players)
                player.Draw(spriteBatch, font1);

            //draw minion 
            foreach(var minion in minions)
                minion.Draw(spriteBatch);

            spriteBatch.End();

            */
            #endregion
            //

            #region interface
            spriteBatch.Begin();

            UI.Draw(spriteBatch,font1,players[0]);

            //draw pointer to be drawn last so its over top everything
            spriteBatch.Draw(mouseTexture, Input.MousePosition - new Vector2(5, 5), Color.White);

            spriteBatch.End();
            #endregion


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
