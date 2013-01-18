﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ShooterMcGavin
{
    class Projectile
    {
        // setup the properties
        public Texture2D Texture;
        public Vector2 Position;
        public bool Active;
        public int Damage;

        // represent the viewable boundary 
        Viewport viewport;

        public int Width
        {
            get { return Texture.Width; }
        }

        public int Height
        {
            get { return Texture.Height; }
        }

        float projectileMoveSpeed;

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 postion)
        {
            Texture = texture;
            Position = postion;
            this.viewport = viewport;

            Active = true;
            Damage = 2;
            projectileMoveSpeed = 20f;
        }

        public void Update()
        {
            // move the projectile
            Position.X += projectileMoveSpeed;

            if (Position.X + Texture.Width / 2 > viewport.Width) Active = false;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }

    }
}
