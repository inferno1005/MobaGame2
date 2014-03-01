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
//using Microsoft.Xna.Framework.Net;
using Lidgren.Network;



namespace MobaGame2
{
    [Serializable]
    class GameState
    {
        public List<Player> players;
        public List<Minion> minions;
        public List<Tower> towers;
        public List<GameEntity> entities;
        public List<Ability> abilities;
        private Map map;

        public GameState(Map map)
        {
            map = new Map();
            this.map = map;


            players = new List<Player>();
            players.Add(new Player());
            players[0].name = "inferno1005";

            abilities = new List<Ability>();

            towers = new List<Tower>();
            towers.Add(new Tower(map, abilities));


            players[0].champ = new FiddleSticks(map, abilities);
            minions = new List<Minion>();
            minions.Add(new Minion(towers[0], map, abilities));
        }

        public void LoadContent(ContentManager Content)
        {
            foreach (var Player in players)
            {
                Player.champ.texture = Content.Load<Texture2D>(Player.champ.texturename);
                Player.champ.attribute.texture = Content.Load<Texture2D>(Player.champ.attribute.texturename);
                for (int i = 0; i < 7; i++)
                {
                    Player.champ.abilities[i].texture = Content.Load<Texture2D>(Player.champ.abilities[i].texturename);
                    Player.champ.abilities[i].icon = Content.Load<Texture2D>(Player.champ.abilities[i].iconname);
                }
            }

            foreach (var Minion in minions)
            {
                Minion.texture = Content.Load<Texture2D>(Minion.texturename);
                Minion.abilities[0].texture = Content.Load<Texture2D>(Minion.abilities[0].texturename);
            }

            foreach (var tower in towers)
            {
                tower.texture = Content.Load<Texture2D>(tower.texturename);
                tower.abilities[0].texture = Content.Load<Texture2D>(tower.abilities[0].texturename);
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Color drawcolor)
        {
            //draw players champs
            foreach (var player in players)
                player.Draw(spriteBatch, font, drawcolor);

            //draw minion 
            foreach (var minion in minions)
                minion.Draw(spriteBatch, drawcolor);

            //tower
            foreach (var tower in towers)
                tower.Draw(spriteBatch, drawcolor);

            //draw abilities
            foreach (var ability in abilities)
                ability.Draw(spriteBatch, drawcolor);


        }

        public void DrawVision(SpriteBatch spriteBatch, Color drawcolor, Texture2D lightmask)
        {
            foreach (var player in players)
            {
                if (player.champ.attribute.alive)
                    spriteBatch.Draw(lightmask, player.champ.visionrect, Color.White);
            }

            //draw minion 
            foreach (var minion in minions)
                spriteBatch.Draw(lightmask, minion.visionrect, Color.White);

            foreach (var tower in towers)
                spriteBatch.Draw(lightmask, tower.visionrect, Color.White);


        }

        public void Update(GameTime gameTime)
        {
            foreach (var player in this.players)
            {
                player.Update(this.map.rect, gameTime, this.abilities);
            }

            for (int i = 0; i < minions.Count; i++)
            {

                this.minions[i].Updater(map.rect, gameTime);

                if (!this.minions[i].attribute.alive)
                {
                    this.minions.RemoveAt(i);
                }
                else
                {
                    foreach (var player in this.players)
                    {
                        this.minions[i].Agro(player.champ);
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


        }

    }
}
