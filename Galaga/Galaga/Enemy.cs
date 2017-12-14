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
    public class Enemy : GameObject
    {
        public static int numDescending = 0;
        private Random r = new Random();
        private Texture2D texture, text;
        private Laser bullet;
        private Rectangle rect;
        private int timer;
        private Vector2 desiredMovement, actualMovement;
        protected int health;
        private Vector2 target, offset;
        enum Movement {ENTER, IDLE, ATTACK}
        Movement movement;
        int cuts = 20;
        int attackAngle = 0;

        public Enemy(Texture2D texture, Vector2 spawn, Vector2 target)
        {
            Init();
            type = Type.ENEMY;
            this.target = target;
            offset = new Vector2(0, 0);
            timer = 0;
            health = 1;
            this.texture = texture;
            this.text = game.getEnemyBulletS();
            rect = new Rectangle((int)spawn.X, (int)spawn.Y, 30, 30);
            this.movement = Movement.ENTER;
        }

        public override void Update(GameTime gameTime)
        {
            timer = (timer + 1) % 60;
            offset = EnemySpawn.offset;

            switch (movement)
            {
                case Movement.ENTER:
                    goToSpot();
                    break;
                case Movement.IDLE:
                    oscillate();
                    if (numDescending < 4 && !LivesUI.respawn && r.Next(300) == 0)
                    {
                        attackAngle = r.Next(5) - 2;
                        numDescending++;
                        movement = Movement.ATTACK;
                    }
                    break;
                case Movement.ATTACK:
                    descend();
                    fire(timer);
                    break;
            }

        }

        public Rectangle getEnemyRect() { return this.rect; }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(texture, this.rect, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0, new Vector2(0,0), SpriteEffects.FlipVertically, 1.0f);
        }

        public void goToSpot()
        {
            desiredMovement = target + offset - new Vector2(rect.X, rect.Y);
            actualMovement = desiredMovement / cuts;
            cuts--;

            rect.X = cuts == 0 ? (int)(target.X + offset.X) : rect.X + (int)actualMovement.X;
            rect.Y = cuts == 0 ? (int)(target.Y + offset.Y) : rect.Y + (int)actualMovement.Y;

            if (cuts == 0)
                movement = Movement.IDLE;
        }

        public void oscillate()
        {
            rect.X = (int)(target.X + offset.X);
            rect.Y = (int)(target.Y + offset.Y);
        }

        public void descend()
        {
            rect.X += attackAngle;
            rect.Y += 4 + Math.Min(game.levelCount, 6);
            if (rect.Y >= 644)
            {
                movement = Movement.ENTER;
                rect.X = r.Next(500);
                rect.Y = -30;
                cuts = 20;
                numDescending--;
            }
        }

        public void fire(int timer)
        {
            
            if (timer == 0 || timer == r.Next(60))
            {
                bullet = new Laser(text, rect.X + 25, rect.Y + 50, 1);
                game.addObject(bullet);
            }
        }

        public void Damage()
        {
            health--;
            if (health <= 0)
            {
                if (movement == Movement.ATTACK)
                    numDescending--;
                EnemySpawn.subtractEnemies();
                this.Destroy();
            }
        }
    }
}
