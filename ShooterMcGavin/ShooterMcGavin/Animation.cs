using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ShooterMcGavin
{
    class Animation
    {
        #region Properties

        Texture2D spriteStrip;
        float scale;
        int elapsedTime;
        int frameTime;
        int frameCount;
        int currentFrame;

        Color color;
        Rectangle sourceRect = new Rectangle();
        Rectangle destinationRect = new Rectangle();

        public int FrameWidth;
        public int FrameHeight;

        public bool Active;
        public bool Looping;

        public Vector2 Position;

        #endregion

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frameTime, Color color, float scale, bool looping)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            // set the time and starting position of the frame
            elapsedTime = 0;
            currentFrame = 0;

            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            if (Active == false) return;

            // update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // update the frames when the elapsed time
            // is more than the frame time

            if (elapsedTime > frameTime)
            {
                // move to the next frame
                currentFrame++;

                // if the current frame reaches the frame count
                // reset the frame back to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    // check to see if the animation needs to loop
                    if (Looping == false)
                    {
                        Active = false;
                    }
                }

                // reset the elapsed time
                elapsedTime = 0;
            }

            // Lets find the frame source
            sourceRect = new Rectangle(
                currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            destinationRect = new Rectangle(
                (int)Position.X - (int)(FrameWidth * scale) / 2,
                (int)Position.Y - (int)(FrameHeight * scale) / 2,
                (int)(FrameWidth * scale),
                (int)(FrameHeight * scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
            }

        }
    }
}
