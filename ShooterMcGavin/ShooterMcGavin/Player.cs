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

        #endregion

        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;

            // set the starting position in the middle of the screen and near the left side
            Position = position;

            Active = true;

            Health = 100;
        }

        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }

    }
}
