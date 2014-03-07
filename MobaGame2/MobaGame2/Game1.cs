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

        //screen res
        int SCREENHEIGHT = 720;
        int SCREENWIDTH = 1280;


        //game
        Camera camera;
        GameState gstate;
        int playerindex = 0;  //the index of this local player in the gamestate



        //scene stuff
        RenderTarget2D mainscene;
        RenderTarget2D fog;

        //Shader
        Effect fogeffect;

        //the map to play on
        Map map;


        //Texture2D mouseTexture;
        Texture2D lightmask;

        //color to draw currently
        Color drawcolor;

        //if the game is started
        bool GameStarted = false;
        #endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
            networking = new LidgrenNetwork();

            //ensure when game window is not focused to not slow down the game,
            //need this for debugging networking on a single computer
            InactiveSleepTime = new TimeSpan(0);    

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

            //fog of war
            fogeffect = Content.Load<Effect>("lighting");
            var pp = GraphicsDevice.PresentationParameters;
            mainscene = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            fog = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);


            //ui
            UI.SetPos(SCREENWIDTH, SCREENHEIGHT);


            //game state
            #region game entity lists
            //gstate = new GameState(map);
            #endregion

            camera = new Camera(new Vector2(0, 0), SCREENHEIGHT, SCREENWIDTH, 12);

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

            //load all textures for the ui
            UI.LoadContent(Content);

            lightmask = Content.Load<Texture2D>("texture\\whitemask");
            #endregion
        }

        protected override void UnloadContent()
        {
            networking.EndSession();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            //lobby
            if ((networking.isServer && !networking.GameIsRunning) || networking.inLobby)
            {
                Input.HandleLobbyInput(networking,playerindex,gstate);

                //both client and server listen for updates
                object temp;
                temp=networking.ListenMessage();

                //if client
                if (!networking.isServer)
                {
                    //if we got a valid message
                    if (temp != null)
                    {
                        //playerindex 0 is the server, we dont have an index yet
                        //we need to listen for one
                        if (playerindex == 0)
                        {
                            if (temp is int)
                            {
                                playerindex = (int)temp;
                            }
                        }

                        //if we are geting a gamestate
                        if (temp is GameState)
                        {
                            //if we are ready to run the lobby
                            //if we have a playerindex for the client
                            //and our gamestate is not empty
                            if (playerindex != 0 && gstate != null)
                            {

                                //update the server with client choice
                                Player p;
                                p = gstate.players[playerindex];
                                p.id = playerindex;
                                networking.SendObject(p);

                                //update this gstate with other player info
                                gstate = (GameState)temp;

                                //reset this local player 
                                gstate.players[playerindex] = p;
                            }

                            else
                            {
                                gstate = (GameState)temp;
                            }

                        }


                    }
                }
                //if server
                else
                {
                    //start a game start if there isnt one
                    //need to start it before the game starts to setup players
                    if (gstate == null)
                    {
                        gstate = new GameState(map);
                        gstate.LoadContent(Content);

                    }
                    //if we have a new networked player, add to the gamstate of players
                    if (networking.PlayerCount() != gstate.players.Count)
                    {
                        gstate.AddNewPlayer();
                        networking.SendObject(gstate.players.Count-1); //send the index to the new player
                    }

                    //object temp;
                    temp = networking.ListenMessage();
                    if (temp != null)
                    {
                        //got player info from client
                        if (temp is Player)
                        {
                            Player p = (Player)temp;
                            gstate.players[p.id] = p;
                        }
                    }

                    //send to all clients the gamestate
                    networking.SendObject(gstate);
                }

            }

            //searching for session
            else if (networking.searching)
            {
                // Handle the available sessions input here...
                Input.HandleAvailableSessionsInput(networking);
                networking.ListenMessage(); //listen for responses of our seach
            }
            //title screen
            else if (!networking.GameIsRunning)
            {
                //can return whether or not to exit the game
                if (Input.HandleTitleScreenInput(networking, GameStarted))
                {
                    Exit(); 
                }
            }
            //in game
            else if (networking.GameIsRunning||gstate.GameIsRunning)
            {
                //ensure we have a gamestate
                if (gstate == null)
                {
                    gstate = new GameState(map);
                    gstate.LoadContent(Content);
                }

                if (gstate != null)
                {
                    //if server
                    if (networking.isServer)
                    {
                        gstate.GameIsRunning = true;


                        //listen for player update
                        object temp;
                        temp=networking.ListenMessage();
                        if (temp != null)
                        {
                            //if player info, set it
                            if (temp is Player)
                            {
                                Player p=(Player)temp;
                                gstate.players[p.id] = p;
                            }
                        }


                        //send the gamestate
                        networking.SendObject(gstate);
                    }

                    //if client
                    else
                    {
                        //send player data
                        Player p;
                        p = gstate.players[playerindex];
                        p.id = playerindex;
                        networking.SendObject(p);

                        //get the gamestate from the server
                        GameState temp;
                        temp=(GameState)networking.ListenMessage();
                        if (temp != null)
                        {
                                gstate = temp;
                        }
                        //put out player info back into the gamestate, fully trust clients
                        gstate.players[playerindex] = p;

                    }


                    GameUpdate(gameTime);
                }
            }
            base.Update(gameTime);
        }

        protected void GameUpdate(GameTime gameTime)
        {
            #region controls
            //right button selects and targets, if not on anything but map, moves there
            #region mouse
            //if escape menu is not open handle gameplay input
            if (!UI.escMenuOpen)
            {
                //right click
                if (Input.RightMouseButton() && gstate.players[playerindex].champ.attribute.alive)
                {
                    //find an object under the mouse
                    //prefer plays over other objects, because they can kill you!
                    gstate.players[playerindex].champ.FocusObject(Input.FindUnderMouse(camera, gstate,playerindex));

                    //if we didnt find anything under the mouse
                    if (gstate.players[playerindex].champ.focus == null)
                    {
                        //check and see if we clicked on the map
                        if (MathHelper.ClickedOn(Input.MousePosition + camera.position, map.rect))
                        {
                            //if we did, we want to move to that location on the map
                            gstate.players[playerindex].champ.direction = gstate.players[playerindex].champ.CalcDirection(Input.MousePosition + camera.position);
                        }
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
                        case 2:
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
                camera.Center(gstate.players[playerindex].champ.center);
            }

            //debug respawn and insta kill
            if (Input.KeyPressed(Keys.K))
            {
                if (gstate.players[playerindex].champ.attribute.alive == true)
                    gstate.players[playerindex].champ.attribute.alive = false;
                else
                {
                    gstate.players[playerindex].champ.attribute.alive = true;
                    gstate.players[playerindex].champ.attribute.Health = gstate.players[playerindex].champ.attribute.maxhealth;
                }

            }

            //keyboard q
            if (Input.KeyPressed(Keys.Q))
            {
                gstate.players[playerindex].champ.activeability = 1;

                gstate.players[playerindex].champ.FocusObject(Input.FindUnderMouse(camera, gstate,playerindex));
                gstate.players[playerindex].champ.ability();
            }

            //keyboard w
            if (Input.KeyPressed(Keys.W))
            {
                gstate.players[playerindex].champ.activeability = 2;
                gstate.players[playerindex].champ.FocusObject(Input.FindUnderMouse(camera, gstate,playerindex));
                gstate.players[playerindex].champ.ability();
            }
            //keyboard e
            if (Input.KeyPressed(Keys.E))
            {
                gstate.players[playerindex].champ.activeability = 3;
                gstate.players[playerindex].champ.FocusObject(Input.FindUnderMouse(camera, gstate,playerindex));
                gstate.players[playerindex].champ.ability();
            }
            //keyboard r
            if (Input.KeyPressed(Keys.R))
            {
                gstate.players[playerindex].champ.activeability = 4;
                gstate.players[playerindex].champ.ability();
            }
            //keyboard d
            if (Input.KeyPressed(Keys.D))
            {
                gstate.players[playerindex].champ.activeability = 5;
                gstate.players[playerindex].champ.Spell();
            }
            //keyboard f
            if (Input.KeyPressed(Keys.F))
            {
                gstate.players[playerindex].champ.activeability = 6;
                gstate.players[playerindex].champ.Spell();
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
            //move the camera based on the mouse location
            camera.ScreenBorderMove(Input.MousePosition);
            #endregion


            //if the game is not over, update it
            if(!gstate.GameOver)
                gstate.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //if in a lobby
            if ((networking.isServer && !networking.GameIsRunning) || networking.inLobby)
            {
                UI.DrawLobby(spriteBatch, networking,gstate);
            }
            //if looking for a session
            else if (networking.searching)
            {
                UI.DrawAvailableSessions(spriteBatch, networking);
            }
            //if on the title menu
            else if (!networking.GameIsRunning)
            {
                UI.DrawTitleScreen(spriteBatch, networking);
            }
            //if in the game
            else if (networking.GameIsRunning)
            {
                if (gstate != null)
                    DrawMain(gameTime);
            }



            base.Draw(gameTime);
        }

        protected void DrawMain(GameTime gameTime)
        {
            //draw main sprites and map
            DrawGame(gameTime);

            //draw the fog of war over the top of the main draw
            DrawFog(gameTime);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            fogeffect.Parameters["lightMask"].SetValue(fog);
            fogeffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(mainscene, new Vector2(0, 0), Color.White);
            spriteBatch.End();


            //draw the interface, free of fog of war
            #region interface
            spriteBatch.Begin();

            UI.Draw(spriteBatch, gstate.players[playerindex], Input.MousePosition, gameTime,gstate.GameOver,gstate.Winner);

            //draw pointer to be drawn last so its over top everything
            spriteBatch.Draw(UI.textures[0], Input.MousePosition - new Vector2(5, 5), Color.White);

            spriteBatch.End();
            #endregion
            base.Draw(gameTime);
        }

        protected void DrawGame(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(mainscene);
            GraphicsDevice.Clear(Color.Gray);

            //if player is alive draw normaly, otherwise dray grayed out
            if (gstate.players[playerindex].champ.attribute.alive)
                drawcolor = Color.White;
            else
                drawcolor = Color.Gray;

            #region game camera
            //draw based off camera location
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearWrap,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                camera.calc_transformation(1, 1)
                );

            //draw map tiled
            spriteBatch.Draw(UI.textures[5],
                map.rect,   
                new Rectangle((int)map.position.X, (int)map.position.Y, map.texturewidth*20, map.textureheight*2),
               drawcolor);

            spriteBatch.End();

            //draw everything else normally
            spriteBatch.Begin(
                            SpriteSortMode.Immediate,
                            BlendState.AlphaBlend,
                            null,
                            null,
                            null,
                            null,
                            camera.calc_transformation(1, 1)
                    );

            //draw the state of the game
            gstate.Draw(spriteBatch, UI.font, drawcolor);


            spriteBatch.End();
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

            //draw every objects vision
            gstate.DrawVision(spriteBatch, Color.White, lightmask,playerindex);

            spriteBatch.End();
            #endregion

            GraphicsDevice.SetRenderTarget(null);
        }




    }
}
