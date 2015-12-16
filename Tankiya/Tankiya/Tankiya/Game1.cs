﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//custom packages used in this class
using tank_game;

namespace Tankiya
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GraphicsDevice device;

        Texture2D backgroundTexture;
        Texture2D foregroundTexture;
        Texture2D tankTexture;
        Texture2D waterTexture;
        Texture2D bricktexture;

        KeyboardState keyboardState;


        int screenWidth;
        int screenHeight;
        int gridWidth;



        /**
         * Command sender and similar variables to connect with the server.
         * 
         */
        private BasicCommandSender commandSender;
        private Map map;

        /// <summary>
        /// Colors array to color the tanks
        /// </summary>
        private Color[] playerColors = new Color[] { Color.White, Color.Blue, Color.Yellow, Color.Pink, Color.Black };




        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();

            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "The Tankiya";

            keyboardState = Keyboard.GetState();
            commandSender = new BasicCommandSender();
            map = Map.GetInstance();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            device = graphics.GraphicsDevice;

            //screenWidth = device.PresentationParameters.BackBufferWidth;
            //screenHeight = device.PresentationParameters.BackBufferHeight;

            screenWidth = 600;
            screenHeight = 600;
            gridWidth = 60;

            backgroundTexture = Content.Load<Texture2D>("back");
            foregroundTexture = new Texture2D(device, screenWidth, screenHeight, false, SurfaceFormat.Color);
            foregroundTexture.SetData(GenerateMap());
            tankTexture = Content.Load<Texture2D>("tank_min");
            waterTexture = Content.Load<Texture2D>("water_min");
            

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            //read the keyboard input
            GetInput();

            base.Update(gameTime);
        }




        /// <summary>
        /// Read the keyboard inputs and call the command sender class to give the corresponding response
        /// </summary>
        private void GetInput()
        {

            KeyboardState newState = Keyboard.GetState();

            //if "J" is pressed, send the command to join to the server
            if (newState.IsKeyDown(Keys.J))
            {
                if (!keyboardState.IsKeyDown(Keys.J))
                {
                    commandSender.Join();
                }
            }

            //if "Space" is pressed, send the command to shoot
            if (newState.IsKeyDown(Keys.Space))
            {
                if (!keyboardState.IsKeyDown(Keys.Space))
                {
                    commandSender.Shoot();
                }
            }


            //if "Up" is pressed, send the command to go up
            if (newState.IsKeyDown(Keys.Up))
            {
                if (!keyboardState.IsKeyDown(Keys.Up))
                {
                    commandSender.Up();
                }
            }

            //if "Down" is pressed, send the command to go down
            if (newState.IsKeyDown(Keys.Down))
            {
                if (!keyboardState.IsKeyDown(Keys.Down))
                {
                    commandSender.Down();
                }
            }

            //if "Left" is pressed, send the command to go Left
            if (newState.IsKeyDown(Keys.Left))
            {
                if (!keyboardState.IsKeyDown(Keys.Left))
                {
                    commandSender.Left();
                }
            }

            //if "Right" is pressed, send the command to go right
            if (newState.IsKeyDown(Keys.Right))
            {
                if (!keyboardState.IsKeyDown(Keys.Right))
                {
                    commandSender.Right();
                }
            }

            keyboardState = newState;
        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            DrawScenery();

            DrawObstacles();

            DrawTanks();

            spriteBatch.End();

            base.Draw(gameTime);
        }



        /// <summary>
        /// Draws the background and the foreground.
        /// </summary>
        private void DrawScenery()
        {

            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
            spriteBatch.Draw(foregroundTexture, screenRectangle, Color.White);
        }

        /*
        texture
        Type: Texture2D
        A texture.
        position
        Type: Vector2
        The location (in screen coordinates) to draw the sprite.
        sourceRectangle
        Type: Nullable<Rectangle>
        A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture.
        color
        Type: Color
        The color to tint a sprite. Use Color.White for full color with no tinting.
        rotation
        Type: Single
        Specifies the angle (in radians) to rotate the sprite about its center.
        origin
        Type: Vector2
        The sprite origin; the default is (0,0) which represents the upper-left corner.
        scale
        Type: Vector2
        Scale factor.
        effects
        Type: SpriteEffects
        Effects to apply.
        layerDepth
        Type: Single
        The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. 
                 * Use SpriteSortMode if you want sprites to be sorted during drawing.
         * */

        private void DrawTanks()
        {
            Player[] players = Map.GetInstance().GetPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                if(players[i]!=null){

                    spriteBatch.Draw(tankTexture, new Vector2(players[i].cordinateX*60+30, players[i].cordinateY*60+30),
                        null, playerColors[i], GetRotation(players[i].direction), new Vector2(30, 30), 1, SpriteEffects.None, 1);
                
                }
            }
        }




        private void DrawObstacles() { 
            MapItem[,] grid = Map.GetInstance().GetGrid();
            for (int i = 0; i < grid.GetLength(0);i++ )
            {
                for (int j = 0; j < grid.GetLength(1); j++) { 
                    
                    /**
                     * Draw water
                     */
                    if(grid[i,j]!=null && grid[i, j].GetType()==typeof(Water)){

                        spriteBatch.Draw(waterTexture, new Vector2(i* 60 , j * 60 ),
                        null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

                    }

                    /**
                     * Draw bricks
                     */
                    if (grid[i, j] != null && grid[i, j].GetType() == typeof(Brick))
                    {
                        
                    }
                }
                
            }
        }

/*
        0 North
        1 East,
        2 South 
        3 West 
 */


        private float GetRotation(int direction) {
            switch (direction) { 
                case 0:
                    return ToRadian(0);
                    break;
                case 1:
                    return ToRadian(90);
                    break;
                case 2:
                    return ToRadian(180);
                    break;
                case 3:
                    return ToRadian(-90);
                    break;
            }

            return 0;
        
        }

        private float ToRadian(int degrees) {
            return (float)(Math.PI / 180) * degrees;
        }




        /// <summary>
        /// Generates the grid where the game will be played. A 10x10 grid
        /// </summary>
        /// <returns></returns>
        private Color[] GenerateMap()
        {
            Color[] grid = new Color[screenHeight * screenWidth];

            for (int i = 0; i < screenWidth; i++)
            {
                for (int j = 0; j < screenHeight; j++)
                {
                    grid[i + screenWidth * j] = Color.Transparent;
                    if (i % gridWidth == 0)
                    {
                        grid[i + screenWidth * j] = Color.White;
                    }

                    if (j % gridWidth == 0)
                    {
                        grid[i + screenWidth * j] = Color.White;
                    }
                }
            }

            return grid;
        }
    }
}
