
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
//using Microsoft.Xna.Framework.Net;
using Lidgren.Network;




namespace MobaGame2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region vars
        //needed
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        LidgrenNetwork networking;

        //screen
        int SCREENHEIGHT = 720;
        int SCREENWIDTH = 1280;


        //game objects
        Camera camera;
        GameState gstate;


        //scene stuff
        RenderTarget2D mainscene;
        RenderTarget2D fog;

        //Shader
        Effect fogeffect;


        //Camera minimap;
        Map map;


        //Texture2D mouseTexture;
        Texture2D lightmask;

        //buffer
        String buffer;

        Color drawcolor;


        bool GameStarted = false;
        #endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
            networking = new LidgrenNetwork();
        }

        protected override void Initialize()
        {

            #region graphics setup
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;
            graphics.PreferredBackBufferWidth = SCREENWIDTH;
            graphics.PreferMultiSampling = false;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            this.Window.Title = "Moba";
            #endregion

            //fog
            fogeffect = Content.Load<Effect>("lighting");
            var pp = GraphicsDevice.PresentationParameters;
            mainscene = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            fog = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);


            //ui
            UI.SetPos(SCREENWIDTH, SCREENHEIGHT);
            UI.ChampIcons = new List<Texture2D>();


            //game state
            #region game entity lists
            //gstate = new GameState(map);
            #endregion

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

            map.texture = Content.Load<Texture2D>(map.texturename);

            UI.LoadContent(Content);

            //lightmask = Content.Load<Texture2D>("texture\\lightmask");
            lightmask = Content.Load<Texture2D>("texture\\whitemask");
            #endregion
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            //networking.EndSession();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            if (networking.isServer && !networking.GameIsRunning)
            {
                Input.HandleLobbyInput(networking);
                if (networking.isServer)
                    networking.ListenMessage();
            }
            else if (networking.searching)
            {
                // Handle the available sessions input here...
                //Input.HandleLobbyInput(networking);
                Input.HandleAvailableSessionsInput(networking);
                networking.ListenMessage();
            }
            else if (!networking.GameIsRunning)
            {
                if (Input.HandleTitleScreenInput(networking, GameStarted))
                {
                    Exit();
                }
            }
            else if (networking.GameIsRunning)
            {

                if (gstate == null)
                {
                    gstate = new GameState(map);
                    gstate.LoadContent(Content);
                }

                if (gstate != null)
                    GameUpdate(gameTime);
            }
            //player.lastState = currentState;
            base.Update(gameTime);
        }

        protected void GameUpdate(GameTime gameTime)
        {
            #region controls
            //right button selects and targets, if not on anything but map, moves there
            #region mouse
            //if menu is not open
            if (!UI.escMenuOpen)
            {
                //right click
                if (Input.RightMouseButton() && gstate.players[0].champ.attribute.alive)
                {

                    //prefer plays over other objects, because they can kill you!
                    gstate.players[0].champ.FocusObject(Input.FindUnderMouse(camera, gstate));

                    if (gstate.players[0].champ.focus == null)
                        if (MathHelper.ClickedOn(Input.MousePosition + camera.position, map.rect))
                        {
                            gstate.players[0].champ.direction = gstate.players[0].champ.CalcDirection(Input.MousePosition + camera.position);
                            //players[0].champ.FocusObject(null);

                        }
                }
            }

            else //working with the esc menu
            {
                if (Input.LeftMouseButton())
                {
                    switch (Input.MenuChoice(Input.MousePosition, UI.menupos))
                    {
                        case 1:     //exit
                            networking.GameIsRunning = false;
                            networking.EndSession();
                            UI.escMenuOpen = false;
                            gstate = null;
                            return; //return so that we dont try to update a null game
                            break;
                        case 2:
                            //networking.GameIsRunning = false;
                            //networking.EndSession();
                            UI.escMenuOpen = false;
                            gstate = null;
                            gstate = new GameState(map);
                            gstate.LoadContent(Content);
                            break;


                    }

                }



            }
            #endregion

            #region keyboard

            //center camera to champ and follow
            if (Input.KeyHeld(Keys.Space))
            {
                camera.Center(gstate.players[0].champ.center);
            }

            //debug respawn and insta kill
            if (Input.KeyPressed(Keys.K))
            {
                if (gstate.players[0].champ.attribute.alive == true)
                    gstate.players[0].champ.attribute.alive = false;
                else
                {
                    gstate.players[0].champ.attribute.alive = true;
                    gstate.players[0].champ.attribute.Health = gstate.players[0].champ.attribute.maxhealth;
                }

            }

            if (Input.KeyPressed(Keys.Q))
            {
                gstate.players[0].champ.activeability = 1;

                gstate.players[0].champ.FocusObject(Input.FindUnderMouse(camera, gstate));
                gstate.players[0].champ.ability();
            }
            if (Input.KeyPressed(Keys.W))
            {
                gstate.players[0].champ.activeability = 2;
                gstate.players[0].champ.FocusObject(Input.FindUnderMouse(camera, gstate));
                gstate.players[0].champ.ability();
            }
            if (Input.KeyPressed(Keys.E))
            {
                gstate.players[0].champ.activeability = 3;
                gstate.players[0].champ.FocusObject(Input.FindUnderMouse(camera, gstate));
                gstate.players[0].champ.ability();
            }
            if (Input.KeyPressed(Keys.R))
            {
                gstate.players[0].champ.activeability = 4;
                gstate.players[0].champ.ability();
            }
            if (Input.KeyPressed(Keys.D))
            {
                gstate.players[0].champ.activeability = 5;
                gstate.players[0].champ.Spell();
            }
            if (Input.KeyPressed(Keys.F))
            {
                gstate.players[0].champ.activeability = 6;
                gstate.players[0].champ.Spell();
            }

            //display menu
            if (Input.KeyPressed(Keys.Escape))
            {
                if (UI.escMenuOpen)
                    UI.escMenuOpen = false;
                else
                    UI.escMenuOpen = true;

            }
            #endregion

            #endregion

            #region camera
            camera.ScreenBorderMove(Input.MousePosition);
            #endregion


            gstate.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //if in a lobby
            if (networking.isServer && !networking.GameIsRunning)
            {
                UI.DrawLobby(spriteBatch, networking);
            }
            //if looking for a session
            else if (networking.searching)
            {
                UI.DrawAvailableSessions(spriteBatch, networking);
            }
            else if (!networking.GameIsRunning)
            {
                UI.DrawTitleScreen(spriteBatch, networking);
            }
            else if (networking.GameIsRunning)
            {
                if (gstate != null)
                    DrawMain(gameTime);
            }



            base.Draw(gameTime);
        }

        protected void DrawMain(GameTime gameTime)
        {
            DrawGame(gameTime);

            DrawFog(gameTime);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            fogeffect.Parameters["lightMask"].SetValue(fog);
            fogeffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(mainscene, new Vector2(0, 0), Color.White);
            spriteBatch.End();


            #region interface
            spriteBatch.Begin();

            UI.Draw(spriteBatch, gstate.players[0], Input.MousePosition, gameTime);

            //draw pointer to be drawn last so its over top everything
            spriteBatch.Draw(UI.mouseTexture, Input.MousePosition - new Vector2(5, 5), Color.White);

            spriteBatch.End();
            #endregion
            base.Draw(gameTime);
        }

        protected void DrawGame(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRenderTarget(mainscene);
            GraphicsDevice.Clear(Color.Gray);

            //if player is alive draw normaly, otherwise dray grayed out
            if (gstate.players[0].champ.attribute.alive)
                drawcolor = Color.White;
            else
                drawcolor = Color.Gray;

            #region game camera
            //draw based off camera location
            spriteBatch.Begin(
                //SpriteSortMode.BackToFront,
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearWrap,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                //camera.calc_transformation(SCREENHEIGHT,SCREENWIDTH)
                camera.calc_transformation(1, 1)
                );

            //draw map
            spriteBatch.Draw(map.texture,
                map.rect,
                new Rectangle((int)map.position.X, (int)map.position.Y, map.texturewidth, map.textureheight),
               drawcolor);

            spriteBatch.End();

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

            gstate.Draw(spriteBatch, UI.font, drawcolor);


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

            GraphicsDevice.SetRenderTarget(null);
            //base.Draw(gameTime);
        }

        protected void DrawFog(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(fog);
            GraphicsDevice.Clear(new Color(15, 15, 15));

            #region game camera
            //draw based off camera location
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive,
                null,
                null,
                null,
                null,
                camera.calc_transformation(1, 1)
                );

            gstate.DrawVision(spriteBatch, Color.White, lightmask);

            spriteBatch.End();
            #endregion

            GraphicsDevice.SetRenderTarget(null);
        }


    }
}
