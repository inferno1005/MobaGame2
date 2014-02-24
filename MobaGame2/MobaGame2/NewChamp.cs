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
    class NewChamp : GameEntity
    {
        public string Name;
        public List<Ability> abilities;
        public int activeability;

        private Map map;
        private List<Ability> globalabilities;

        public NewChamp(Map m,List<Ability> ga)
        {
            abilities = new List<Ability>();
            attribute = new Attributes();
            map = m;
            globalabilities = ga;
        }

        public void Updater(GameTime gametime)
        {
            this.Update(rect);
            BasicAttack();

            if (this.attribute.health < 0)
                this.attribute.alive = false;

            foreach (var ability in abilities)
            {
                ability.Update(gametime);
            }
        }

        public void Draw(SpriteBatch spriteBatch,Color color)
        {
            if (this.attribute.visible)
            {
                /*
                //so slow ;-;
                this.rotation %= (float)Math.PI * 2 ;
                this.rotation += (float)Math.PI / 2;

                spriteBatch.Draw(this.texture,
                    new Rectangle((int)this.center.X,(int)this.center.Y,(int)this.width,(int)this.height),
                    null,
                    this.color,
                    this.rotation,
                    new Vector2(this.width,this.height),
                    SpriteEffects.None,0);
                 */


                spriteBatch.Draw(this.texture.texture, this.texture.rect, color);
                attribute.Draw(spriteBatch, this.position.position);
            }
        }

        public void BasicAttack()
        {
            if (focus != null &&        //if focused on object
                focus.position.distance < this.attribute.range && //if in range
                !this.abilities[0].cast) //if timer is good
            {
                this.abilities[0].cast = true;
                this.abilities[0].position= this.position;
                this.abilities[0].focus= this.focus;
                this.abilities[0].attribute.visible = true;
                Ability temp = new Ability(this.abilities[0]); 
                temp.position = this.position;
                temp.focus = this.focus;


                globalabilities.Add(temp);
            }
        }
        public void Q()
        {

            if (focus != null &&        //if focused on object
                focus.position.distance < this.attribute.range && //if in range
                !this.abilities[0].cast) //if timer is good
            {
                this.abilities[0].cast = true;
                this.abilities[0].position = this.position;
                this.abilities[0].focus = this.focus;
                this.abilities[0].attribute.visible = true;
                Ability temp = new Ability(this.abilities[0]);
                temp.position = this.position;
                temp.focus = this.focus;


                globalabilities.Add(temp);
            }

        }
    }
}
