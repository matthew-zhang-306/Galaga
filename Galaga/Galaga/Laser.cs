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
    public class Laser : GameObject
    {
       
        Texture2D texture;
        private Rectangle rect;
        KeyboardState oldKB = Keyboard.GetState();
        private int direction;
        
        public Laser(Texture2D text, int x, int y, int d)
        {
            this.Init();
            type = Type.LASER;

            rect = new Rectangle(x,y,3,18);
            texture = text;
            direction = d;
        }

        public override void Update(GameTime gameTime)
        {
            rect.Y += (direction < 0 ? 8 : 6 + game.levelCount) * direction;
            if (rect.Y <= -20 || rect.Y >= 650)
                Destroy();

            foreach (GameObject obj in game.getObjects())
            {
                if(direction < 0)
                {
                    if (obj.getType() == Type.ENEMY)
                    {
                        Enemy enemy = (Enemy)obj;
                        if (getLaserRect().Intersects(enemy.getEnemyRect()))
                        {
                            enemy.Damage();
                            Destroy();
                            game.score += 100;
                            if (game.score % 7200 == 0)
                                LivesUI.addLives();
                        }
                    }
                }
                else if (direction > 0)
                {
                    if (obj.getType() == Type.SHIP)
                    {
                        Ship ship = (Ship)obj;
                        if (getLaserRect().Intersects(ship.getShipRect()))
                        {
                            ship.Hit();
                            Destroy();
                        }
                    }
                }
                
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(texture, this.getLaserRect(), Color.White);

        }

        public Rectangle getLaserRect()
        {
            return this.rect;
        }
    }
}
