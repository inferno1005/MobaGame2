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
    class NewChamp : GameEntity
    {
        public string Name;
        public List<Ability> abilities;
        public int activeability;

        private Map map;
        protected List<Ability> globalabilities;

        public NewChamp(Map m,List<Ability> ga)
        {
            abilities = new List<Ability>();
            attribute = new Attributes();
            map = m;
            globalabilities = ga;
        }

        public void Updater(GameTime gametime)
        {
            //make sure we arent attacking a dead thing
            if (focus!=null && !focus.attribute.alive)
            {
                FocusObject(null);
            }

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


                spriteBatch.Draw(UI.textures[this.textureindex], this.rect, color);
                attribute.Draw(spriteBatch, this.position);
            }
        }

        public void BasicAttack()
        {
            if (this.focus != null &&        //if focused on object
                this.focus.Distance(this.position)< this.attribute.range && //if in range
                !this.abilities[0].cast) //if timer is good
            {
                //Console.WriteLine("{2}range checker {0} < {1}", focus.distance ,this.attribute.range,focus.texturename);
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

        //stuff that travels across the map
        public void ability()
        {
            if (focus != null &&                         //if focused on object
                this.focus.Distance(this.position)< this.attribute.range && //if in range
                !this.abilities[activeability].cast  &&
                this.abilities[activeability].manaCost<attribute.mana)
            {
                this.attribute.mana -= this.abilities[activeability].manaCost;
                this.abilities[activeability].cast = true;
                this.abilities[activeability].position = this.position;
                this.abilities[activeability].focus = this.focus;
                this.abilities[activeability].attribute.visible =
                    this.abilities[activeability].castvisibile;

                Ability temp = new Ability(this.abilities[activeability]);
                temp.position = this.position;
                temp.focus = this.focus;
                temp.textureindex = this.abilities[activeability].textureindex;

                globalabilities.Add(temp);
            }
        }
        //stuff that applies to this champ 
        public void Spell()
        {
            if (!this.abilities[activeability].cast)
            {
                this.abilities[activeability].cast = true;
                this.abilities[activeability].position = this.position;
                this.attribute.mana -= this.abilities[activeability].manaCost;

                Ability temp = new Ability(this.abilities[activeability]);

                temp.attribute.visible = this.abilities[activeability].castvisibile;
                temp.position=this.position;
                temp.focus = this;

                globalabilities.Add(temp);

            }
        }

    }
}
