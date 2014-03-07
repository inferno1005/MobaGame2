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
using Lidgren.Network;



namespace MobaGame2
{
    [Serializable]
    class GameState
    {
        public List<Player> players;
        public List<Minion> minions;
        public List<Tower> towers;
        public List<Ability> abilities;
        public List<Nexus> nexuses;
        public List<Bush> bushes;
        public Map map;
        public bool GameIsRunning = false;
        public bool Winner = false;
        public bool GameOver= false;

        //location of team spawn points
        private Vector2 falsespawn;
        private Vector2 truespawn;

        public GameState(Map map)
        {
            map = new Map();
            this.map = map;

            //spawns
            falsespawn = new Vector2(290, 290);
            truespawn = new Vector2(map.width - 290, 290);

            abilities = new List<Ability>();

            #region towers
            towers = new List<Tower>();
            for (int i = 0; i < 8; i++)
                towers.Add(new Tower(map, abilities));

            //four for each team
            towers[0].position = new Vector2(map.position.X + 1000,map.position.Y+150);
            towers[0].attribute.team = false;

            towers[1].position = new Vector2(map.position.X + 1000,map.height-350);
            towers[1].attribute.team = false;

            towers[2].position = new Vector2(map.position.X + 2400,map.height/2-towers[3].height/2);
            towers[2].attribute.team = false;

            towers[3].position = new Vector2(map.position.X + 4000,map.height/2-towers[3].height/2);
            towers[3].attribute.team = false;


            //team 2
            towers[4].position = new Vector2(map.width - 1000,map.position.Y+150);
            towers[4].attribute.team = true;

            towers[5].position = new Vector2(map.width - 1000,map.height-350);
            towers[5].attribute.team = true;

            towers[6].position = new Vector2(map.width - 2400,map.height/2-towers[3].height/2);
            towers[6].attribute.team = true;

            towers[7].position = new Vector2(map.width - 4000,map.height/2-towers[3].height/2);
            towers[7].attribute.team = true;



            #endregion

            #region nexus
            //one for each team
            nexuses = new List<Nexus>();
            nexuses.Add(new Nexus());
            nexuses.Add(new Nexus());

            nexuses[0].position = new Vector2(map.position.X + 700, map.height / 2 - nexuses[0].height / 2);
            nexuses[0].attribute.team = false;

            nexuses[1].position = new Vector2(map.width - 700-nexuses[0].width/2, map.height / 2 - nexuses[0].height / 2);
            nexuses[1].attribute.team = true;
            #endregion

            #region bushes
            bushes = new List<Bush>();

            for(int i=0;i<1;i++)
                bushes.Add(new Bush());
            bushes[0].position = new Vector2(map.position.X + 4500, map.position.Y);



            #endregion

            #region players
            players = new List<Player>();
            players.Add(new Player());
            players[0].name = "1";

            players[0].champ = new FiddleSticks(map, abilities);
            players[0].champ.attribute.team = false;

            #endregion

            #region minions
            minions = new List<Minion>();
            ////minions.Add(new Minion(towers[0], map, abilities));
            #endregion
        }

        public void LoadContent(ContentManager Content)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Color drawcolor)
        {
            //draw players champs
            foreach (var player in players)
            {
                player.Draw(spriteBatch, font, drawcolor);
            }

            //draw minion 
            foreach (var minion in minions)
            {
                minion.Draw(spriteBatch, drawcolor);
            }

            //tower
            foreach (var tower in towers)
            {
                tower.Draw(spriteBatch, drawcolor);
            }

            foreach (var nexus in nexuses)
            {
                nexus.Draw(spriteBatch, drawcolor);
            }

            //draw abilities
            foreach (var ability in abilities)
            {
                ability.Draw(spriteBatch, drawcolor);
            }


            //draw the bush
            foreach (var bush in bushes)
            {
                bush.Draw(spriteBatch, drawcolor);
            }
                


        }

        //draw the vision of the object
        public void DrawVision(SpriteBatch spriteBatch, Color drawcolor, Texture2D lightmask,int playerindex)
        {
            foreach (var player in players)
            {
                //if the player is alive and on the same team, gives local player vision
                if (player.champ.attribute.alive &&
                    players[playerindex].champ.attribute.team==player.champ.attribute.team)

                    spriteBatch.Draw(lightmask, player.champ.visionrect, Color.White);
            }

            //draw minion 
            foreach (var minion in minions)
            {
                if (players[playerindex].champ.attribute.team == minion.attribute.team)
                    spriteBatch.Draw(lightmask, minion.visionrect, Color.White);
            }

            foreach (var tower in towers)
            {
                if (players[playerindex].champ.attribute.team == tower.attribute.team)
                    spriteBatch.Draw(lightmask, tower.visionrect, Color.White);
            }
            foreach (var nexus in nexuses)
            {

                if (players[playerindex].champ.attribute.team == nexus.attribute.team)
                    spriteBatch.Draw(lightmask, nexus.visionrect, Color.White);
            }


        }

        public void Update(GameTime gameTime)
        {
            foreach (var player in this.players)
            {
                player.Update(this.map.rect, gameTime, this.abilities);
                //respawn the player
                if (!player.champ.attribute.alive)
                {
                    //respawn at true base
                    if (player.champ.attribute.team == true)
                    {
                        player.champ.position = truespawn;
                    }
                    //respawn at false base
                    else
                    {
                        player.champ.position = falsespawn;
                    }

                    //reset stats
                    player.champ.attribute.Health = player.champ.attribute.maxhealth;
                    player.champ.attribute.mana = player.champ.attribute.maxmana;
                    player.champ.attribute.alive = true;
                }
            }

            for (int i = 0; i < minions.Count; i++)
            {


                this.minions[i].Updater(map.rect, gameTime);

                //if minion is dead, remove it
                if (!this.minions[i].attribute.alive)
                {
                    this.minions.RemoveAt(i);
                }
                else 
                {

                    //find first tower
                    //then find minions
                    //then find players
                    //if none of those ,goto enemey base


                    bool found = false;
                    foreach (var tower in this.towers)
                    {
                        if (!found && tower.attribute.team != minions[i].attribute.team)
                        {
                            found = this.minions[i].Agro(tower);
                        }
                    }
                    if (!found)
                        foreach (var minion in this.minions)
                        {
                            if (!found && minion.attribute.team != minions[i].attribute.team)
                            {
                                found = this.minions[i].Agro(minion);
                            }
                        }
                    if (!found)
                        foreach (var player in this.players)
                        {
                            if (!found && player.champ.attribute.team != minions[i].attribute.team)
                                found = this.minions[i].Agro(player.champ);
                        }


                }

            }

            for (int i = 0; i < this.towers.Count; i++)
            {

                this.towers[i].Updater(this.map.rect, gameTime);

                //remove if dead
                if (!this.towers[i].attribute.alive)
                    this.towers.RemoveAt(i);
                else
                {
                    //find minions first
                    //the find players


                    foreach (var minion in this.minions)
                    {
                        this.towers[i].Agro(minion);
                    }
                    foreach (var player in this.players)
                    {
                        this.towers[i].Agro(player.champ);
                    }
                }
            }

            for (int i = 0; i < abilities.Count; i++)
            {
                this.abilities[i].Update(gameTime);
                if (this.abilities[i].ghost)
                    this.abilities.RemoveAt(i);
            }

            foreach (var nexus in nexuses)
            {
                nexus.Update(map.rect);
                if (!nexus.attribute.alive)
                {
                    GameOver = true;
                    Winner = nexus.attribute.team;
                }

            }

            //spawn minions are regular intervales
            MinionSpawner.Update(this,gameTime);

        }


        //add a new player to the gamestate
        public void AddNewPlayer()
        {
            players.Add(new Player());
            players.Last().name = players.Count.ToString();

            players.Last().champ = new FiddleSticks(map, abilities);
            players.Last().champ.attribute.team = false;


        }

        //start the game, spawn all players at the correct spawn points
        public void StartGame()
        {
            if (!GameIsRunning)
            {
                GameIsRunning = true;

                foreach (var player in players)
                {
                    if (player.champ.attribute.team == true)
                    {
                        player.champ.position = truespawn;
                    }
                    else
                        player.champ.position = falsespawn;
                }
            }
        }
    }
}
