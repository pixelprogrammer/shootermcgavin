using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShooterMcGavin
{
    class ParallaxingBackground
    {
        Texture2D texture;
        // create an array of positions
        Vector2[] positions;
        int speed;

        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed)
        {
            // load the background
            this.texture = content.Load<Texture2D>(texturePath);

            this.speed = speed;

            // divide the screen width with the texture width. This will determine how much tiles we will need to create a continuous loop of backgrounds
            // add 1 so there isn't a gap in the tiling
            this.positions = new Vector2[screenWidth / texture.Width + 1];

            // now lets set the initial positions of the parallaxing backgrounds
            for (int i = 0; i < positions.Length; i++)
            {
                // this is pretty self explanitory
                // the tiles will be positioned side by side next to eachother
                positions[i] = new Vector2(i * texture.Width, 0);
            }
        }

        public void Update()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                // lets start moving this background
                positions[i].X += speed;
                // lets check which direction the background is moving and decide which direction we should be moving in
                if (speed <= 0)
                {
                    // clearly we are going to be scrolling left
                    if (positions[i].X <= -texture.Width)
                    {
                        positions[i].X = texture.Width * (positions.Length - 1);
                    }
                }
                else
                {
                    if (positions[i].X >= texture.Width * (positions.Length - 1))
                    {
                        positions[i].X = -texture.Width;
                    }

                    
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                spriteBatch.Draw(texture, positions[i], Color.White);
            }
        }
    }
}
