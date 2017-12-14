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
using System.Collections;

namespace Galaga
{
    public abstract class GameObject
    {
        protected bool isAlive;
        protected static Game1 game;
        protected static int enemies=0;
        public enum Type {SHIP, ENEMY, LASER, DEFAULT};
        protected Type type;

        public static void setGame(Game1 g)
        {
            game = g;
        }

        protected void Init()
        {
            isAlive = true;
            
        }

        protected void Destroy()
        {
            isAlive = false;
        }

        public bool isDestroyed()
        {
            return !this.isAlive;
        }

        public Type getType()
        {
            return type;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch sb);

    }
}
