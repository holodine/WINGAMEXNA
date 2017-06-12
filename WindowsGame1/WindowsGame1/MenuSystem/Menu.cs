using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1.MenuSystem
{
    class Menu
    {
        public List<MenuItem> Items { get; set; }
        SpriteFont font;

        int currentItem;
        KeyboardState oldState;

        MouseState oldMouse;

        public Menu()
        {
            Items = new List<MenuItem>();
        }

        public void Update()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter))
                Items[currentItem].OnClick();

            int delta = 0;
            if (state.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
                delta = -1;
            if (state.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
                delta = 1;

            currentItem += delta;
            bool ok = false;
            while (!ok)
            {
                if (currentItem < 0)
                    currentItem = Items.Count - 1;
                else if (currentItem > Items.Count - 1)
                    currentItem = 0;
                else if (Items[currentItem].Active == false)
                    currentItem += delta;
                else ok = true;
            }

            oldState = state;

            MouseState mouse = Mouse.GetState();
            Rectangle mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            int x = 100;
            int y = 100;

            foreach (MenuItem item in Items)
            {
                Vector2 v = font.MeasureString(item.Name);
                Rectangle r = new Rectangle(x, y, (int)v.X, (int)v.Y);

                if (mouseRect.Intersects(r))
                {
                    if (item.Active)
                    {
                        currentItem = Items.IndexOf(item);
                        if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                            item.OnClick();
                    }
                }
                y += 80;
            }

            oldMouse = mouse;
        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("menuFont");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            int y = 100;
            foreach (MenuItem item in Items)
            {
                Color color = Color.Brown;
                if (item.Active == false)
                    color = Color.Gray;
                if (item == Items[currentItem])
                    color = Color.White;
                spriteBatch.DrawString(font, item.Name, new Vector2(100, y), color);
                y += 80;
            }


            spriteBatch.End();
        }
    }
}
