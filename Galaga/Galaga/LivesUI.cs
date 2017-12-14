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
    public class LivesUI : GameObject
    {
        private static int lives = 3;
        private Texture2D texture;
        public static bool respawn = false;
        private int timer;

        public LivesUI(Texture2D texture)
        {
            Init();
            type = Type.DEFAULT;

            timer = 0;
            this.texture = texture;
        }

        public override void Update(GameTime gameTime)
        {
            if (respawn)
            {
                timer++;
                if (timer >= 200)
                {
                    game.addObject(new Ship());
                    respawn = false;
                    timer = 0;
                }
            }
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            for (int i = 0; i < lives; i++) {
                sb.Draw(texture, new Rectangle(9 + 44 * i, 600, 35, 35), Color.White);
            }
        }

        public static void subtractLives()
        {
            lives--;

            if (lives == 0)
            {
                game.endScreen();
            }
            else
            {
                respawn = true;
            }
        }

        public static void addLives()
        {
            lives++;
        }

        public static void setLives(int l)
        {
            lives = l;
        }
    }
}
