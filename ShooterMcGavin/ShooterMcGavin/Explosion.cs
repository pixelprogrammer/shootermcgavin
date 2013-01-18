using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShooterMcGavin
{
    class Explosion
    {
        Animation animation;
        float movementSpeed;

        public bool finished;

        public void Initialize(Animation animation)
        {
            this.animation = animation;

            movementSpeed = -6f;
            finished = false;
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
            // move the animation according to the move speed
            animation.Position.X += movementSpeed;
            // let the caller know that this is finished
            if (animation.Active == false) { finished = true; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }

    }
}
