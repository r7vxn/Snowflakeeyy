using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Snowflakeeyy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Rectangle window;
        Random generator;
        Rectangle snowFlake;
        List<(Rectangle Rectangle, float Timer)> snowFlakes;
        Texture2D snowFlakeTexture;
        Vector2 fallspeed;
        Vector2 stop;
        Rectangle snowCollectRect;
        Texture2D rectTexture;
        float seconds;
        List<(int x, float y)> tuple;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            snowCollectRect = new Rectangle(240, 400, 300, 20);
            window = new Rectangle(0, 0, 800, 600);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();
            generator = new Random();
            snowFlakes = new List<(Rectangle Rectangle, float Timer)>();
            Rectangle tempSnowflake;
            for (int i = 0; i < 150; i++)
            {
                tempSnowflake = new Rectangle(
                generator.Next(window.Width),
                generator.Next(window.Height),
                8,
                8);
                snowFlakes.Add((tempSnowflake, 5));
            }
            fallspeed = new Vector2(0, 2);
            stop = new Vector2(0, -2);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            snowFlakeTexture = Content.Load<Texture2D>("snowflakes");
            rectTexture = Content.Load<Texture2D>("rectangle");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            for (int i = 0; i < snowFlakes.Count; i++)
            {
                Rectangle snowFlakeRectangle = snowFlakes[i].Rectangle;
                float snowFlakeTimer = snowFlakes[i].Timer;

                snowFlakeRectangle.X += (int)fallspeed.X;
                snowFlakeRectangle.Y += (int)fallspeed.Y;

                if (snowCollectRect.Contains(snowFlakeRectangle))
                {
                    snowFlakeRectangle = snowFlakes[i].Rectangle;
                    snowFlakeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (snowFlakeTimer < 0)
                    {
                        snowFlakeRectangle = new Rectangle(generator.Next(window.Width), generator.Next(-10, 0), 8, 8);
                        snowFlakeTimer = 5;
                    }
                }

                snowFlakes[i] = (snowFlakeRectangle, snowFlakeTimer);

                if (snowFlakes[i].Rectangle.Y > window.Height)
                {
                    snowFlakes[i] = new(
                    new Rectangle(
                    generator.Next(window.Width),
                    generator.Next(-10, 0),
                    8,
                    8),
                    5
                    );
                }


               
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(rectTexture, snowCollectRect, Color.DarkGray);
            foreach (var (rect, value) in snowFlakes)
            {
                _spriteBatch.Draw(snowFlakeTexture, rect, Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
