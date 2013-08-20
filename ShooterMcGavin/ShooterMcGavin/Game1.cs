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

namespace ShooterMcGavin
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;

        // parallaxing backgrounds
        ParallaxingBackground bgLayer0;
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;

        // player input and movements properties

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        float playerSpeed;

        // initializing enemies
        Texture2D enemyTexture;
        List<Enemy> enemies;

        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        Random random;

        // intialize the projectiles variables
        Texture2D projectileTexture;
        List<Projectile> projectiles;

        TimeSpan fireTime;
        TimeSpan previousFireTime;

        // initialize the explosion variables
        Texture2D explosionTexture;
        List<Explosion> explosions;

        // initialize the sound vars
        SoundEffect laserSound;
        SoundEffect explosionSound;
        Song gameplayMusic;

        // score
        int score;

        // fonts
        SpriteFont font;

        // this is just to draw the hit boxes
        Texture2D pixel;
        Texture2D greenColor;
        Texture2D blueColor;
        Texture2D redColor;

        // we will be using the Rectangles built in intersect function
        // to determine collision
        Rectangle playerHitbox;
        Rectangle enemyHitbox;
        Rectangle projectileHitbox;

        Rectangle testHitbox;
        Rectangle testEnemyHitbox;
        List<Rectangle> testEnemies;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here
            player = new Player();
            playerSpeed = 4.0f;

            bgLayer0 = new ParallaxingBackground();
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();

            enemies = new List<Enemy>();

            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);

            random = new Random();

            projectiles = new List<Projectile>();
            fireTime = TimeSpan.FromSeconds(.15f);

            explosions = new List<Explosion>();

            score = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("shipAnimation");
            playerAnimation.Initialize(playerTexture, new Vector2(0,0), 115, 69, 8, 15, Color.White, 1f, true);
            Vector2 playerPosition = new Vector2(0, 100 + playerAnimation.FrameHeight);

             // set the collision border colors
            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });

            player.Initialize(playerAnimation, playerPosition, pixel);

            // load the backgrounds
            // background = Content.Load<Texture2D>("mainbackground");
            bgLayer0.Initialize(Content, "mainbackground", GraphicsDevice.Viewport.Width,-1);
            bgLayer1.Initialize(Content, "bgLayer1", GraphicsDevice.Viewport.Width, -2);
            bgLayer2.Initialize(Content, "bgLayer2", GraphicsDevice.Viewport.Width, -4);
            
            // load enemies
            enemyTexture = Content.Load<Texture2D>("mineAnimation");

            // Load projectile textures
            projectileTexture = Content.Load<Texture2D>("laser");

            // load explosions.....PRRRKKKkakkkarrrrrrrrggkkkkkkkkk
            explosionTexture = Content.Load<Texture2D>("explosion");

            // load the music and sounds
            gameplayMusic = Content.Load<Song>("audio/music/gameMusic");
            laserSound = Content.Load<SoundEffect>("audio/sound/laserFire");
            explosionSound = Content.Load<SoundEffect>("audio/sound/explosion");

            // load fonts
            font = Content.Load<SpriteFont>("gameFont");
            // start music
            PlayMusic(gameplayMusic);

            
        }

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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            UpdatePlayer(gameTime);
            UpdateEnemies(gameTime);

            UpdateCollision();
            UpdateProjectiles();
            UpdateExplosions(gameTime);

            bgLayer0.Update();
            bgLayer1.Update();
            bgLayer2.Update();

            base.Update(gameTime);
        }

        


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // draw the background
            // spriteBatch.Draw(background, Vector2.Zero, Color.White);
            bgLayer0.Draw(spriteBatch);
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);
            // draw the enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }
            // draw projectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }
            
            // draw the player
            player.Draw(spriteBatch);
            // draw explosions
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(spriteBatch);
            }
            // draw the fonts
            // score
            spriteBatch.DrawString(
                font,
                "Score: " + score,
                new Vector2(
                    GraphicsDevice.Viewport.TitleSafeArea.X,
                    GraphicsDevice.Viewport.TitleSafeArea.Y),
                Color.White);

            // health
            spriteBatch.DrawString(
                font,
                "Health: " + player.Health,
                new Vector2(
                    GraphicsDevice.Viewport.TitleSafeArea.X,
                    GraphicsDevice.Viewport.TitleSafeArea.Y + 30),
                Color.White);

            // player position
            spriteBatch.DrawString(
               font,
               "X: " + player.Position.X,
               new Vector2(
                   GraphicsDevice.Viewport.TitleSafeArea.X,
                   GraphicsDevice.Viewport.TitleSafeArea.Y + 60),
               Color.White);

            // player position
            spriteBatch.DrawString(
               font,
               "Y: " + player.Position.Y,
               new Vector2(
                   GraphicsDevice.Viewport.TitleSafeArea.X,
                   GraphicsDevice.Viewport.TitleSafeArea.Y + 90),
               Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        // custom functions
        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);
            // Get the thumbstick controls
            player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerSpeed;
            player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerSpeed;

            // using the Keyboard or DPad

            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                player.Position.X -= playerSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                player.Position.X += playerSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) || currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                player.Position.Y -= playerSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) || currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                player.Position.Y += playerSpeed;
            }

            // lets make sure the player doesn't go out of bounds

            player.Position.X = MathHelper.Clamp(player.Position.X, -player.CollisionOffset.X, GraphicsDevice.Viewport.Width - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);

            // lets add some shooting controls
            if(currentKeyboardState.IsKeyDown(Keys.Space)) {
                if (gameTime.TotalGameTime - previousFireTime > fireTime)
                {
                    // reset the previous time for the fire rate
                    previousFireTime = gameTime.TotalGameTime;
                    AddProjectile(player.Position + new Vector2(player.Width, player.Height/2));
                    laserSound.Play();
                }
            }

        }

        private void AddEnemy()
        {
            Animation enemyAnimation = new Animation();
            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);

            Vector2 position = new Vector2(
                GraphicsDevice.Viewport.Width + enemyTexture.Width, random.Next(100, GraphicsDevice.Viewport.Height - 100));

            Enemy enemy = new Enemy();
            enemy.Initialize(enemyAnimation, position, pixel);

            enemies.Add(enemy);

        }

        private void UpdateEnemies(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                AddEnemy();
                
            }

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);

                if (enemies[i].Active == false)
                {
                    if (enemies[i].Health <= 0)
                    {
                        // add an explosion
                        AddExplosion(enemies[i].Position);
                        explosionSound.Play();
                        score += enemies[i].PointValue;
                    }
                    enemies.RemoveAt(i);
                }
            }
        }

        private void UpdateCollision()
        {
            

            // we will declare the hitbox for the player just once
            playerHitbox = player.UpdateHitbox();

            // check the collision between the player and enemies
            for (int i = 0; i < enemies.Count; i++)
            {

                // create the enemies hit box

                if (playerHitbox.Intersects(enemies[i].UpdateHitbox()))
                {
                    enemies[i].Health = 0;
                    // damage player
                    player.Health -= enemies[i].Damage;
                    // kill the player
                    if (player.Health <= 0)
                    {
                        player.Active = false;

                    }

                }
            }

            // projectile vs enemies
            for(int i = 0; i < projectiles.Count; i++) {
                for(int j =0; j < enemies.Count; j++) {
                    // create the rectangles needed
                    // lets start with the projectiles
                    projectileHitbox = new Rectangle(
                        (int)projectiles[i].Position.X,
                        (int)projectiles[i].Position.Y,
                        projectiles[i].Width,
                        projectiles[i].Height);

                    // lets do the enemies now
                    enemyHitbox = enemies[j].UpdateHitbox();

                    // do the 2 objects collide
                    if (projectileHitbox.Intersects(enemyHitbox))
                    {
                        enemies[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }

        }

        private void TestCollision()
        {
            testHitbox = new Rectangle(
                (int)player.Position.X,
                (int)player.Position.Y + 150,
                player.Width,
                player.Height);
        }

        private void TestUpdate(GameTime gameTime)
        {
            // move the test enemies

        }
        private void TextDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, testHitbox, Color.SeaGreen);
        }

        private void AddTestEnemy()
        {

        }
        private void AddProjectile(Vector2 position)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position);
            projectiles.Add(projectile);
        }

        private void UpdateProjectiles()
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();
                if (projectiles[i].Active == false) projectiles.RemoveAt(i);
            }
        }

        private void AddExplosion(Vector2 position)
        {
            Animation explosionAnimation = new Animation();
            explosionAnimation.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);

            Explosion explosion = new Explosion();
            explosion.Initialize(explosionAnimation);

            explosions.Add(explosion);
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].finished == true) { explosions.RemoveAt(i); }
            }
        }

        private void PlayMusic(Song song)
        {
            try
            {
                MediaPlayer.Play(song);

                MediaPlayer.IsRepeating = true;
            }
            catch { }

        }
    }
}
