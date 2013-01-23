using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShooterMcGavin
{
    class Player
    {
        #region Properties

        public Vector2 Position;
        public bool Active;
        public int Health;

        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        public Animation PlayerAnimation;
        Texture2D pixel;

        #endregion

        public void Initialize(Animation animation, Vector2 position, Texture2D pixel)
        {
            PlayerAnimation = animation;

            // set the starting position in the middle of the screen and near the left side
            Position = position;
            Active = true;
            this.pixel = pixel;
            Health = 100;


        }
        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, new Rectangle(
                (int)Position.X - Width/2,
                (int)Position.Y - Height/2,
                Width,
                Height), Color.Violet);
            PlayerAnimation.Draw(spriteBatch);
            
            
        }

    }
}
