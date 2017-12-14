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
    public class Ship : GameObject
    {
        private Texture2D texture,text;
        private KeyboardState oldKB = Keyboard.GetState();
        private Laser bullet;
        private Rectangle rect;
        private const int Y = 600;
        private int count;
        
        
        public Ship()
        {
            type = Type.SHIP;

            count = 0;
            this.texture = game.getPlayerS();
            this.text = game.getPlayerBulletS();
            rect = new Rectangle(225, 590, 30, 30);
            Init();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            count = Math.Max(0, count - 1);
            
            if (kb.IsKeyDown(Keys.Right))
            {
                rect.X = Math.Min(rect.X + 6, 450);
            }
            if (kb.IsKeyDown(Keys.Left))
            {
                rect.X = Math.Max(0, rect.X - 6);
            }

            foreach (GameObject obj in game.getObjects())
            {
                if (obj.getType() == Type.ENEMY)
                {
                    Enemy enemy = (Enemy)obj;
                    if (rect.Intersects(enemy.getEnemyRect()))
                    {
                        this.Hit();
                    }
                }
            }


            if (kb.IsKeyDown(Keys.Space) && oldKB.IsKeyUp(Keys.Space) && count == 0)
            {
                bullet = new Laser(text, rect.X + 25, rect.Y - 25, -1);
                game.addObject(bullet);
                count = 20;
            }

            oldKB = kb;
        }

        public Rectangle getShipRect() { return this.rect; }

        public void Hit()
        {
            LivesUI.subtractLives();
            
            this.Destroy();
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(texture, this.getShipRect(), Color.White);
        }

    }
}
