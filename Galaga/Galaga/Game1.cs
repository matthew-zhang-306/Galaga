using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public int score = 0;
        public int levelCount = 0;
        public enum State { START, INGAME, END };
        private SpriteFont big;
        private SpriteFont small;
        private String galaga, enterScreen, instruction;
        private int timerText;
        private KeyboardState oldKB;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private State state;
        private int width, height;
        private SpriteFont text;
        private Rectangle[] backdrop;
        private Texture2D playerS, playerSBullet, enemySBullet, background;
        private Texture2D[] enemyS = new Texture2D[4];
        private GameObject player, enemy, lives;
        private ArrayList objects;

        public Game1()
        {

            state = State.START;
            graphics = new GraphicsDeviceManager(this);
            GameObject.setGame(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            timerText = 0;
            galaga = "Galaga";
            enterScreen = "Press ENTER to begin";
            instruction = "         Welcome to Galaga\n         Click space to fire\n   Use left and right to move\nKill as many enemies as you can";
            backdrop = new Rectangle[3];
            for (int i = 0; i < backdrop.Length; i++)
            {
                backdrop[i].Y = i * 644;
                backdrop[i].X = 0;
                backdrop[i].Width = 500;
                backdrop[i].Height = 644;
            }
            oldKB = Keyboard.GetState();
            graphics.PreferredBackBufferWidth = width = 500;
            graphics.PreferredBackBufferHeight = height = 644;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            small = Content.Load<SpriteFont>("SpriteFont1");
            big = Content.Load<SpriteFont>("SpriteFont2");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("back");
            text = Content.Load<SpriteFont>("text");
            playerS = Content.Load<Texture2D>("player");
            for (int i = 1; i <= 4; i++)
                enemyS[i - 1] = Content.Load<Texture2D>("enemy" + i);
            playerSBullet = Content.Load<Texture2D>("bullet");
            enemySBullet = Content.Load<Texture2D>("bullete");

            Init();

            // TODO: use this.Content to load your game content here
        }

        public Texture2D getPlayerS() { return playerS; }
        public Texture2D[] getEnemyS() { return enemyS; }
        public Texture2D getPlayerBulletS() { return playerSBullet; }
        public Texture2D getEnemyBulletS() { return enemySBullet; }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                this.Exit();
            timerText++;

            updateBackdrop();

            if (state == State.START)
                startScreen();
            else
            {
                if (state == State.END)
                    endScreen();

                for (int i = 0; i < objects.Count; i++)
                {
                    GameObject obj = (GameObject)objects[i];

                    obj.Update(gameTime);
                    if (obj.isDestroyed())
                    {
                        objects.Remove(obj);
                    }
                }
            }

            oldKB = kb;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            for (int i = 0; i < backdrop.Length; i++)
            {
                spriteBatch.Draw(background, backdrop[i], Color.White);
            }


            if (state == State.START)
            {
                spriteBatch.DrawString(big, galaga, new Vector2(60, 120), Color.Green);
                spriteBatch.DrawString(small, enterScreen, new Vector2(115, 250), timerText / 40 % 2 == 0 ? Color.Maroon : new Color(0, 0, 0, 0));
                spriteBatch.DrawString(small, instruction, new Vector2(30, 400), Color.White);
            }
            else
            {

                foreach (GameObject obj in objects)
                {
                    obj.Draw(gameTime, spriteBatch);


                }
                spriteBatch.DrawString(text, "Score - " + score, new Vector2(20, 20), Color.Red);
                if (state == State.END)
                {
                    spriteBatch.DrawString(text, "GAME OVER", new Vector2(width / 2 - 95, height / 2 - 140), Color.White);
                    spriteBatch.DrawString(small, "Press ENTER to restart", new Vector2(100, 270), timerText / 40 % 2 == 0 ? Color.Maroon : new Color(0, 0, 0, 0));
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void updateBackdrop()
        {
            backdrop[0].Y += 4;
            backdrop[1].Y += 4;
            backdrop[2].Y += 4;
            for (int i = 0; i < backdrop.Length; i++)
            {
                if (backdrop[i].Y >= 1288)
                {
                    backdrop[i].Y = -644;
                }
            }
        }

        public void startScreen()
        {
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Enter) && oldKB.IsKeyUp(Keys.Enter))
            {
                state = State.INGAME;
                objects.Add(player);
                objects.Add(enemy);
                objects.Add(lives);
            }

        }

        public void endScreen()
        {
            state = State.END;

            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Enter) && oldKB.IsKeyUp(Keys.Enter))
            {
                state = State.START;
                Init();
            }

        }

        public virtual void Init()
        {
            objects = new ArrayList();
            player = new Ship();
            enemy = new EnemySpawn();
            lives = new LivesUI(playerS);

            Enemy.numDescending = 0;
            score = 0;
            levelCount = 0;
            LivesUI.setLives(3);
        }

        public void addObject(GameObject obj)
        {
            this.objects.Add(obj);
        }

        public ArrayList getObjects()
        {
            return this.objects;
        }

    }

}
