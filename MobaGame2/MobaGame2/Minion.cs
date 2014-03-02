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
    [Serializable]
    class Minion : NewChamp
    {
        GameEntity enemybase;
        public Minion(GameEntity ge,Map m,List<Ability> ga) : base(m,ga)
        {
            this.Name = "Minion";
            //this.texturename = "texture\\Minion";
            this.textureindex = 2;

            this.height = 32;
            this.width = 32;
            this.position = new Vector2(10, 10);
            this.attribute.range = 200;
            this.attribute.speed = 1;
            this.attribute.visionrange = 200;
            this.attribute.attackDamage=10;
            this.attribute.maxhealth = 500;
            this.attribute.health= 500;


            this.abilities.Add(new Ability());
            this.abilities[0].name = "Basic Attack";
            this.abilities[0].attribute.range = this.attribute.range;
            this.abilities[0].physicalDamage = this.attribute.attackDamage;
            this.abilities[0].magicDamage = 0;
            this.abilities[0].coolDown = 3;
            //this.abilities[0].texturename = "texture\\fireball";
            enemybase = ge;
        }
        public bool Agro(GameEntity target)
        {
            if (this.attribute.range > (distance = Vector2.Distance(this.position, target.position)))
            {
                //Console.WriteLine("SHOULD BE AGRO ");

                FocusObject(target);
                return true;
            }
            else
            {
                FocusObject(enemybase);
                return false;
            }
        }
        public void Updater(Rectangle rect, GameTime gametime)
        {
            if (focus != null && !focus.attribute.alive)
            {
                    FocusObject(enemybase);
            }



            this.Update(rect);
            attribute.Update();
            BasicAttack();

            if (this.attribute.health < 0)
            {
                Console.WriteLine("Minion is dead!");
                this.attribute.alive = false;
                this.attribute.clickable = false;
                this.attribute.visible= false;
            }
            foreach (var ability in abilities)
            {
                ability.Update(gametime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (this.attribute.visible)
            {
                spriteBatch.Draw(UI.textures[this.textureindex], this.rect, color);
                attribute.Draw(spriteBatch,this.position);
            }
        }


    }
}
