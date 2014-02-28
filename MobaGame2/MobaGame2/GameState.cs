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

            public void DrawVision(SpriteBatch spriteBatch, Color drawcolor,Texture2D lightmask)
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

                foreach (var player in players)
                {
                    player.Update(map.rect, gameTime, abilities);
                }

                for (int i = 0; i < minions.Count; i++)
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

                for (int i = 0; i < towers.Count; i++)
                {

                    towers[i].Updater(map.rect, gameTime);

                    foreach (var minion in minions)
                    {
                        towers[i].Agro(minion);
                    }
                    foreach (var player in players)
                    {
                        towers[i].Agro(player.champ);
                    }
                }

                for (int i = 0; i < abilities.Count; i++)
                {
                    abilities[i].Update(gameTime);
                    if (abilities[i].ghost)
                        abilities.RemoveAt(i);
                }


            }

    }
}
