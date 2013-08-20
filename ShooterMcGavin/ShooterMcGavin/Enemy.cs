using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShooterMcGavin
{
    class Enemy
    {
        public Animation EnemyAnimation;
        public Vector2 Position;
        public bool Active;
        public int Health;
        public int Damage;
        public int PointValue;
        
        public int Width
        {
            get { return EnemyAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return EnemyAnimation.FrameHeight; }
        }

        Texture2D pixel;
        float enemySpeed;


        public void Initialize(Animation animation, Vector2 position, Texture2D pixel)
        {
            EnemyAnimation = animation;
            Position = position;
            Active = true;
            Health = 2;
            Damage = 10;

            enemySpeed = 4f;

            PointValue = 100;

            // for collisions
            this.pixel = pixel;
        }

        public void Update(GameTime gameTime)
        {
            // this enemy has simple AI. All it does is move to the left
            Position.X -= enemySpeed;
            // we have to update the animation position
            EnemyAnimation.Position = Position;
            EnemyAnimation.Update(gameTime);

            // if the enemies health reaches 0 or goes off screen set it to
            // active = false so that it will be removed later
            if (Position.X < -Width || Health <= 0)
            {
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           
            EnemyAnimation.Draw(spriteBatch);
        }

        public Rectangle UpdateHitbox()
        {
            return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                Width - 10,
                Height - 6);
        }
    }
}
