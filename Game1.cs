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

                snowFlakes[i] = new(
                    new Rectangle(
                    snowFlakes[i].Rectangle.X + (int)fallspeed.X,
                    snowFlakes[i].Rectangle.Y + (int)fallspeed.Y,
                    snowFlakes[i].Rectangle.Width,
                    snowFlakes[i].Rectangle.Height
                    ),
                snowFlakes[i].Timer
                );

                if (snowFlakes[i].Rectangle.Y > window.Height)
                {
                    snowFlakes[i] = new(
                    new Rectangle(
                    generator.Next(window.Width),
                    generator.Next(-10, 0),
                    8,
                    8),
                    snowFlakes[i].Timer
                    );
                }
                if (snowCollectRect.Contains(snowFlakes[i].Rectangle))
                {
                    
                    snowFlakes[i] = new(
                      new Rectangle(
                    snowFlakes[i].Rectangle.X + (int)stop.X,
                    snowFlakes[i].Rectangle.Y + (int)stop.Y,
                    snowFlakes[i].Rectangle.Width,
                    snowFlakes[i].Rectangle.Height
                    ),
                    snowFlakes[i].Timer - (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (seconds < 0)
                    {
                        snowFlakes[i] = new(
                        new Rectangle(
                        generator.Next(window.Width),
                        generator.Next(-10, 0),
                        8,
                        8),
                        snowFlakes[i].Timer);
                    }
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
