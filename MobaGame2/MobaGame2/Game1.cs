
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
using Microsoft.Xna.Framework.Net;




namespace MobaGame2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        #region vars
        //needed
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Networking networking;

        //screen
        int SCREENHEIGHT = 720;
        int SCREENWIDTH = 1280;


        //game objects
        Camera camera;


        //scene stuff
        RenderTarget2D mainscene;
        RenderTarget2D fog;

        //Shader
        Effect fogeffect;


        //Camera minimap;

        List<Player> players;
        List<Minion> minions;
        List<Tower> towers;
        List<GameEntity> entities;
        List<Ability> abilities;

        Map map;

        //controls


        SpriteFont font1;
        //Texture2D mouseTexture;
        Texture2D lightmask;

        //buffer
        String buffer;

        Color drawcolor;
        #endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
            networking = new Networking();

            // add gamer services
            Components.Add(new GamerServicesComponent(this));

            //respond to the signed in gamer event
            SignedInGamer.SignedIn += new EventHandler<SignedInEventArgs>(networking.SignedInGamer_SignedIn);
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


            // TODO: Add your initialization logic here
            #region game entity lists
            players = new List<Player>();
            players.Add(new Player());
            players[0].name = "inferno1005";

            abilities = new List<Ability>();

            towers = new List<Tower>();
            towers.Add(new Tower());


            players[0].champ = new FiddleSticks(map,abilities);
            minions = new List<Minion>();
            minions.Add(new Minion(towers[0],map,abilities));

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
            foreach (var Player in players)
            {
                Player.champ.texture.texture = Content.Load<Texture2D>(Player.champ.texture.name);
                Player.champ.attribute.texture = Content.Load<Texture2D>(Player.champ.attribute.texturename);
                for (int i = 0; i < 7; i++)
                {
                    Player.champ.abilities[i].texture.texture = Content.Load<Texture2D>(Player.champ.abilities[0].castedtexture.name);
                    Player.champ.abilities[i].uitexture.texture= Content.Load<Texture2D>(Player.champ.abilities[0].uitexture.name);
                }
            }

            foreach (var Minion in minions)
            {
                Minion.texture.texture = Content.Load<Texture2D>(Minion.texture.name);
                Minion.abilities[0].texture.texture = Content.Load<Texture2D>(Minion.abilities[0].texture.name);
            }

            //foreach (var tower in Towers)
            //{
            //tower.texture = Content.Load<Texture2D>(tower.texturename);
            //}


            map.texture.texture = Content.Load<Texture2D>(map.texture.name);
            UI.mouseTexture = Content.Load<Texture2D>("texture\\pointer");
            UI.background= Content.Load<Texture2D>("background");

            Texture2D temp;
            temp=Content.Load<Texture2D>("texture\\FiddlesticksSquare");
            for(int i=0;i<50;i++)
                UI.ChampIcons.Add(temp);

            //lightmask = Content.Load<Texture2D>("texture\\lightmask");
            lightmask = Content.Load<Texture2D>("texture\\whitemask");
            #endregion

            #region fonts
            font1 = Content.Load<SpriteFont>("DefaultFont");
            #endregion

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            networking.EndSession();
        }

        protected override void Update(GameTime gameTime)
        {

            Input.Update();

            if (!Guide.IsVisible)
            {
                foreach (SignedInGamer signedInGamer in SignedInGamer.SignedInGamers)
                {

                    if (networking.networkSession != null)
                    {
                        if (networking.networkSession.SessionState == NetworkSessionState.Lobby)
                            Input.HandleLobbyInput(networking);
                    }
                    else if (networking.availableSessions != null)
                    {
                        // Handle the available sessions input here...
                            Input.HandleLobbyInput(networking);
                        
                    }
                    else
                    {
                        /*
                        if (Input.HandleTitleScreenInput(networking))
                        {
                            Exit();
                        }*/
                    }
                    //player.lastState = currentState;
                }
            }
            GameUpdate(gameTime);
            base.Update(gameTime);
        }

        protected void GameUpdate(GameTime gameTime)
        {
            //Input.Update();
            bool foundobject;


            #region controls
            //right button selects and targets, if not on anything but map, moves there
            #region mouse
            //if menu is not open
            if (!UI.escMenuOpen)
            {
                //right click
                if (Input.RightMouseButton() && players[0].champ.attribute.alive)
                {
                    foundobject = false;

                    //prefer plays over other objects, because they can kill you!
                    foreach (var player in players)
                    {
                        if (!foundobject)
                        {
                            if (MathHelper.ClickedOn(Input.MousePosition + camera.position, player.champ.rect))
                            {
                                //focus this player
                                foundobject = true;
                            }
                        }
                    }


                    if (!foundobject)
                        foreach (var minion in minions)
                        {
                            if (!foundobject)
                                if (MathHelper.ClickedOn(Input.MousePosition + camera.position, minion.rect))
                                {
                                    foundobject = true;
                                    players[0].champ.FocusObject(minion);
                                    if (players[0].champ.abilities[0].cast)
                                    {
                                        Console.WriteLine("CASTED");
                                    }
                                }

                        }


                    if (!foundobject)
                        if (MathHelper.ClickedOn(Input.MousePosition + camera.position, map.rect))
                        {
                            players[0].champ.position.CalcDirection(Input.MousePosition + camera.position);
                            players[0].champ.FocusObject(null);

                        }
                }
            }

            else //working with the esc menu
            {
                if (Input.LeftMouseButton())
                {
                    switch (UI.MenuChoice(Input.MousePosition))
                    {
                        case 1: Exit();
                            break;
                    }

                }



            }
            #endregion

            #region keyboard

            //center camera to champ and follow
            if (Input.KeyHeld(Keys.Space))
            {
                camera.Center(players[0].champ.position.center);
            }

            //debug respawn and insta kill
            if (Input.KeyPressed(Keys.K))
            {
                if (players[0].champ.attribute.alive == true)
                    players[0].champ.attribute.alive = false;
                else
                {
                    players[0].champ.attribute.alive = true;
                    players[0].champ.attribute.Health = players[0].champ.attribute.maxhealth;
                }

            }
            if (Input.KeyPressed(Keys.Q))
            {
                players[0].champ.Q();
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


            #region updates

            foreach (var player in players)
            {
                player.Update(map.rect, gameTime,abilities);
            }

            for (int i = 0; i < minions.Count;i++ )
            {
                foreach (var player in players)
                {
                    minions[i].Agro(player.champ);
                }

                minions[i].Updater(map.rect, gameTime);

                if (!minions[i].attribute.alive)
                {
                    minions.RemoveAt(i);
                }
            }

            for (int i = 0; i < abilities.Count; i++)
            {
                abilities[i].Update(gameTime);
                if (abilities[i].ghost)
                    abilities.RemoveAt(i);
            }
            #endregion

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //if in a lobby
            if (networking.networkSession != null)
            {
                if (networking.networkSessionState == NetworkSessionState.Lobby)
                    UI.DrawLobby(spriteBatch, font1, networking);
            }
            //if looking for a session
            else if (networking.availableSessions != null)
            {
                UI.DrawAvailableSessions(spriteBatch, font1, networking);
            }
            {
                //UI.DrawTitleScreen(spriteBatch,font1,networking);
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

                UI.Draw(spriteBatch, font1, players[0], Input.MousePosition);

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
            if (players[0].champ.attribute.alive)
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
            spriteBatch.Draw(map.texture.texture,
                map.rect,
                new Rectangle((int)map.position.position.X,(int)map.position.position.Y,map.texturewidth,map.textureheight),
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


            //draw players champs
            foreach (var player in players)
                player.Draw(spriteBatch, font1, drawcolor);

            //draw minion 
            foreach (var minion in minions)
                minion.Draw(spriteBatch, drawcolor);

            //draw abilities
            foreach (var ability in abilities)
                ability.Draw(spriteBatch, drawcolor);

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
                //SpriteSortMode.BackToFront,
                SpriteSortMode.Immediate,
                BlendState.Additive,
                null,
                null,
                null,
                null,
                //camera.calc_transformation(SCREENHEIGHT,SCREENWIDTH)
                camera.calc_transformation(1, 1)
                );
            //draw map
            //spriteBatch.Draw(map.texture, map.rect, drawcolor);

            //spriteBatch.End();



            foreach (var player in players)
            {
                if(player.champ.attribute.alive)
                    spriteBatch.Draw(lightmask, player.champ.visionrect, Color.White);
            }

            //draw minion 
            foreach (var minion in minions)
                spriteBatch.Draw(lightmask, minion.visionrect, Color.White);

            spriteBatch.End();
            #endregion

            GraphicsDevice.SetRenderTarget(null);
        }


    }
}
