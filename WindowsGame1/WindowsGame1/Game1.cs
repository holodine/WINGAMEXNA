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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using WindowsGame1.MenuSystem;

namespace WindowsGame1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D blockTexture1;
        Texture2D blockTexture2;

        List<Block> blocks;
        KeyboardState oldState;

        int currentLevel = 1;
        int maxLevel = 3;

        // Audio engine
        AudioEngine engine;
        WaveBank waveBank;
        SoundBank soundBank;

        Cue music;

        // Game World
        Texture2D myTexture;
        Rectangle myRectangle;

        // Screen Settings
        int screenWidth;
        int screenHeight;

        // Initialize Menu
        Menu menu;
        GameState gameState = GameState.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // Settings Screen
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 950;
            this.Window.Title = "XNA - 2D SUPER SPACE GAME!";           
            Content.RootDirectory = "Content";
            // Settings Mouse
            IsMouseVisible = true;
        }

        
        protected override void Initialize()
        {
            // Game Menu
            menu = new Menu();
            MenuItem newGame = new MenuItem("Новая Игра");
            MenuItem resumeGame = new MenuItem("Продолжить");
            MenuItem exitGame = new MenuItem("Выход");

            resumeGame.Active = false;

            newGame.Click += new EventHandler(newGame_Click);
            resumeGame.Click += new EventHandler(resumeGame_Click);
            exitGame.Click += new EventHandler(exitGame_Click);

            this.IsMouseVisible = true;

            menu.Items.Add(newGame);
            menu.Items.Add(resumeGame);
            menu.Items.Add(exitGame);


            // Audio Engine Initialize
            engine = new AudioEngine("Content\\test.xgs");
            waveBank = new WaveBank(engine, "Content\\Wave Bank.xwb");
            soundBank = new SoundBank(engine, "Content\\Sound Bank.xsb");

            music = soundBank.GetCue("music");
            music.Play();
            
            base.Initialize();
        }

        void exitGame_Click(object sender, EventArgs e)
        {
            this.Exit();
        }

        void resumeGame_Click(object sender, EventArgs e)
        {
            gameState = GameState.Game;
            this.IsMouseVisible = false;
        }

        void newGame_Click(object sender, EventArgs e)
        {
            menu.Items[1].Active = true;
            gameState = GameState.Game;
            this.IsMouseVisible = false;

            CreateLevel1();
        }

        private ScrollingBackground myBackground;
        protected override void LoadContent()
        {           
            spriteBatch = new SpriteBatch(GraphicsDevice);
            myTexture = Content.Load<Texture2D>("player");
            myRectangle = new Rectangle(400, 450, 64, 64);
            // Collisions Window Player Settings
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            myBackground = new ScrollingBackground();
            Texture2D background = Content.Load<Texture2D>("space");
            myBackground.Load(GraphicsDevice, background);

            blockTexture1 = Content.Load<Texture2D>("textures/block");
            blockTexture2 = Content.Load<Texture2D>("textures/block2");

            CreateLevel1();

            menu.LoadContent(Content);
        }

        void CreateLevel1()
        {
            blocks = new List<Block>();
            string[] lines = File.ReadAllLines("content/Levels/level" + currentLevel + ".txt");


            int x = 0;
            int y = 0;
            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    Rectangle rect = new Rectangle(x, y, 40, 40);
                    if (c == 'X')
                    {

                        Block block = new Block(rect, blockTexture1);
                        blocks.Add(block);
                    }
                    if (c == 'Y')
                    {

                        Block block = new Block(rect, blockTexture2);
                        blocks.Add(block);
                    }
                    x += 40;
                }
                x = 0;
                y += 40;
            }
        }

        protected override void UnloadContent()
        {
            
        }


        protected override void Update(GameTime gameTime)
        {
            if (gameState == GameState.Game)
                UpdateGameLogic(gameTime);
            else menu.Update();            

            base.Update(gameTime);
        }

        private void UpdateGameLogic(GameTime gameTime)
        {
            /*if (Keyboard.GetState().GetPressedKeys().Contains(Keys.Escape))
                Exit();*/
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                this.IsMouseVisible = true;
                gameState = GameState.Menu;
            }
            if (state.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                currentLevel++;
                if (currentLevel > maxLevel)
                    currentLevel = 1;
                CreateLevel1();
            }

            oldState = state;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            myBackground.Update(elapsed * 100);

            // Audio Engine Update
            engine.Update();

            // Keyboard Settings
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                myRectangle.X += 3;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                myRectangle.X -= 3;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                myRectangle.Y -= 3;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                myRectangle.Y += 3;

            // Collision Window Player
            if (myRectangle.X <= 0)
                myRectangle.X = 0;
            if (myRectangle.X + myTexture.Width >= screenWidth)
                myRectangle.X = screenWidth - myTexture.Width;

            if (myRectangle.Y <= 0)
                myRectangle.Y = 0;
            if (myRectangle.Y + myTexture.Height >= screenHeight)
                myRectangle.Y = screenHeight - myTexture.Height;
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Прописываем в методе Draw наше меню
            if (gameState == GameState.Game)
                DrawGame();
            else
                menu.Draw(spriteBatch);
            
            base.Draw(gameTime);
        }

        private void DrawGame()
        {
            spriteBatch.Begin();
            myBackground.Draw(spriteBatch);
            foreach (Block block in blocks)
            {
                block.Draw(spriteBatch);
            }
            spriteBatch.Draw(myTexture, myRectangle, Color.White);
            spriteBatch.End();
        }
    }
    enum GameState
    {
        Game,
        Menu
    }
}
