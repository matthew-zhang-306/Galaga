using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaga
{
    public class EnemySpawn : GameObject
    {
        public static Vector2 offset = new Vector2(0, 0);
        public static int dir = 1;
        public static int numEnemies = 0;
        public static int numWaves = 4;
        static int waveIndex;
        static int enemiesAlive;
        static int enemiesSpawned;
        static bool spawning;
        int timer = 0;
        Random r = new Random();

        public EnemySpawn()
        {
            Init();
            type = Type.DEFAULT;
            spawning = true;
            numEnemies = 24;
            enemiesAlive = 0;
            enemiesSpawned = 0;
            waveIndex = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (enemiesSpawned < numEnemies)
            {
                spawn();
            } else
            {
                spawning = false;
            }

            offset.X += dir;
            if (offset.X >= 100 || offset.X <= -100)
                dir *= -1;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            
        }

        public static void subtractEnemies()
        {
            enemiesAlive--;
            if (enemiesAlive <= 0 && !spawning)
            {
                enemiesSpawned = 0;
                waveIndex = 0;
                game.levelCount++;
                spawning = true;
            }
                
                
        }

        public void spawn()
        {
            timer = timer + 1;
            if (timer % 15 == 0)
            {
                Enemy e = new Enemy(game.getEnemyS()[r.Next(game.getEnemyS().Length)], new Vector2(r.Next(500), -30),
                    new Vector2(enemiesSpawned % (numEnemies / numWaves) * 50 + 100, waveIndex * 50 + 90));
                game.addObject(e);
                enemiesSpawned++;
                enemiesAlive++;
                if (enemiesSpawned % (numEnemies / numWaves) == 0)
                {
                    waveIndex++;
                }
            }
        }
    }
}
