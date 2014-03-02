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
    static class MinionSpawner
    {

        static public float coolDown = 60f;
        static public float timer = 0f;
        static public float cooldowntimer { get { return coolDown - timer; } }
        static private bool wave=false;
        static private bool firstwavewait=true;

        static public void Update(GameState gamestate)
        {
            //cool down wave spawner
            if (wave)
            {
                timer += (float)gamestate.gametime.ElapsedGameTime.TotalSeconds;
                if (timer >= coolDown)
                {
                    timer = 0;
                    wave = false;
                }
            }
            else //spawn a wave
            {
                wave = true;

                Console.WriteLine("SPAWNING MINION WAVE");

                //true team
                for (int i = 0; i < 10; i++)
                {
                    gamestate.minions.Add(new Minion(gamestate.nexuses[0], gamestate.map, gamestate.abilities));
                    gamestate.minions.Last().position = new Vector2(9700 - 700 - 256, 200+i*20);
                    gamestate.minions.Last().attribute.team = true;
                }


                //false team
                for (int i = 0; i < 10; i++)
                {
                    gamestate.minions.Add(new Minion(gamestate.nexuses[1], gamestate.map, gamestate.abilities));
                    gamestate.minions.Last().position = new Vector2( 700 + 256, 200+i*20);
                    gamestate.minions.Last().attribute.team = false;
                }
                Console.WriteLine("TOTAL MINIONS = {0}", gamestate.minions.Count);
            }
        }




    }
}
