﻿using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Risk_Project.Players;
using Risk_Project.Components;
using Risk_Project.World_Objects;
using Loader;

namespace Risk_Project
{
    public class GameRoot : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int _width = 1280;
        private int _height = 720;

        #region Properties
        public static Dictionary<string, Texture2D> 
            TextureResource = new Dictionary<string, Texture2D>();
        public static List<Player> Players;
        private Camera2D currentCamera;
        private Board currentBoard;
        public static SpriteFont FontTerritory;
        #endregion

        #region Default Properties
        public const int PLAYER_COUNT = 2;
        public const int DEFAULT_UNIT_AMOUNT = 1;
        public const int DEFAULT_ARMIES = 2;
        #endregion

        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = false;

            Window.Title = "Systems Design - CA Assignment";
            Window.AllowAltF4 = false;
        }

        protected override void Initialize()
        {
            GameInit();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480);
            currentCamera = new Camera2D(viewportAdapter);

            // Load Fonts
            FontTerritory = Content.Load<SpriteFont>("Fonts\\system");

            // Load Texture Resources
            TextureResource = ContentLoader.ContentLoad<Texture2D>(Content, "Textures\\Territories");

            CreateBoard();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentBoard.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: currentCamera.GetViewMatrix());
            currentBoard.Draw(gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Methods
        private void GameInit()
        {
            // Set Window position to center screen
            Window.Position = new Point(
            (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) -
            (graphics.PreferredBackBufferWidth / 2),
            (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) -
            (graphics.PreferredBackBufferHeight / 2));

            currentCamera = new Camera2D(GraphicsDevice);

            InitPlayers();
            CreatePlayers();
        }

        public static void GameRestart()
        {

        }

        private void InitPlayers()
        {
            // Initialize players
            Players = new List<Player>();

            for (int i = 0; i < PLAYER_COUNT; i++)
            {
                Players.Add(new Player());
            }
        }

        private void CreatePlayers()
        {
            Players[0].Name = "Fearon";
            Players[0].Colour = Color.LimeGreen;

            Players[1].Name = "AI";
            Players[1].Colour = Color.Maroon;
        }

        private void CheckPlayerStates()
        {
            // Remove players if eliminated.
            // Game Over if current player is eliminated.
            // Win condition if current player conquered all continents.
        }

        private void CreateBoard()
        {
            currentBoard = new Board(this);
        }
        #endregion
    }
}
