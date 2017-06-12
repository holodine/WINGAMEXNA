using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    public class ScrollingBackground
    {
        // class ScrollingBackground
        private Vector2 screenpos, origin, texturesize;
        private Texture2D mytextures;
        private int screenheight;
        public void Load(GraphicsDevice device, Texture2D backgroundTexture)
        {
            mytextures = backgroundTexture;
            screenheight = device.Viewport.Height;
            int screenwidth = device.Viewport.Width;

            origin = new Vector2(mytextures.Width / 2, 0);

            screenpos = new Vector2(screenwidth / 2, screenheight / 2);

            texturesize = new Vector2(0, mytextures.Height);
        }

        public void Update(float deltaY)
        {
            screenpos.Y += deltaY;
            screenpos.Y = screenpos.Y % mytextures.Height;
        }

        public void Draw(SpriteBatch batch)
        {
            if (screenpos.Y < screenheight)
            {
                batch.Draw(mytextures, screenpos, null,
                    Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }

            batch.Draw(mytextures, screenpos - texturesize, null,
                Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}
