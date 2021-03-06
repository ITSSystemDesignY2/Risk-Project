﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Risk_Project.Players;
using Risk_Project.Components;
using Risk_Project.World_Objects;
using InputManager;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Input;
using System;
using Risk_Project.UI;
using System.Collections.ObjectModel;

namespace Risk_Project
{
    public class GameRoot : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int _width = 1280;
        private int _height = 720;
        private BasicUI basicUI;
        private BasicUIViewModel viewModel;
        private int nativeScreenWidth;
        private int nativeScreenHeight;

        #region Properties

        public static Vector2 WorldBounds
        {
            get
            {
                return new Vector2(1920, 1080);
            }
        }
        public static Dictionary<string, Texture2D> TextureResource;
        public static Queue<Texture2D> BackgroundResource;
        public static List<Player> Players;
        private Camera currentCamera;
        private Board currentBoard;
        public static SpriteFont SystemFontBold;
        public static SpriteFont SystemFontLight;

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

            graphics.PreferredBackBufferWidth = _width;
            graphics.PreferredBackBufferHeight = _height;
            graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            graphics.DeviceCreated += graphics_DeviceCreated;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            IsMouseVisible = true;
            IsFixedTimeStep = false;

            Window.Title = "Systems Design - CA Assignment";
            Window.AllowAltF4 = false;
        }

        #region UI Window Events

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (basicUI != null)
            {
                Viewport viewPort = GraphicsDevice.Viewport;
                basicUI.Resize(viewPort.Width, viewPort.Height);
            }
        }

        protected void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            nativeScreenWidth = graphics.PreferredBackBufferWidth;
            nativeScreenHeight = graphics.PreferredBackBufferHeight;

            // Define resolution
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.IsFullScreen = true;
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
        }

        protected void graphics_DeviceCreated(object sender, EventArgs e)
        {
            Engine engine = new MonoGameEngine(GraphicsDevice, nativeScreenWidth, nativeScreenHeight);
        }

        #endregion

        protected override void Initialize()
        {
            GameInit();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);

            // Load Fonts
            SystemFontBold = Content.Load<SpriteFont>("Fonts\\systemBold");
            SystemFontLight = Content.Load<SpriteFont>("Fonts\\systemLight");

            // Load Texture Resources
            TextureResource = new Dictionary<string, Texture2D>();
            TextureResource = Loader.ContentLoad<Texture2D>(Content, "Textures\\Territories");

            // Load Background Resources
            BackgroundResource = new Queue<Texture2D>();
            BackgroundResource = Loader.ContentLoadQueue<Texture2D>(Content, "Backgrounds\\");

            LoadEmptyKeysUI();
            CreateBoard();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Update Empty Keys UI
            basicUI.UpdateInput(gameTime.ElapsedGameTime.Milliseconds);
            basicUI.UpdateLayout(gameTime.ElapsedGameTime.Milliseconds);

            currentBoard.Update(gameTime);
            currentBoard.UpdateAnimation(gameTime);

            if (currentCamera != null && InputEngine.IsMouseRightHeld())
                Camera.Follow((InputEngine.MousePosition + Camera.CamPos), Helper.GraphicsDevice.Viewport, currentCamera.CameraSpeed);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(
                samplerState: SamplerState.LinearWrap, 
                blendState: BlendState.AlphaBlend, 
                transformMatrix: Camera.CurrentCameraTranslation);
            currentBoard.Draw(gameTime);
            spriteBatch.End();

            basicUI.Draw(gameTime.ElapsedGameTime.TotalMilliseconds);

            base.Draw(gameTime);
        }

        #region Initialize

        private void GameInit()
        {
            // Set Window position to center screen
            Window.Position = new Point(
            (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) -
            (graphics.PreferredBackBufferWidth / 2),
            (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) -
            (graphics.PreferredBackBufferHeight / 2));

            new InputEngine(this);
            Helper.GraphicsDevice = this.GraphicsDevice;
            InitCamera(this);

            InitPlayers();
            CreatePlayers();
        }

        private void InitCamera(Game game)
        {
            currentCamera = new Camera(game, Vector2.Zero, WorldBounds);
        }

        private void CreateBoard()
        {
            currentBoard = new Board(this);
        }

        #endregion

        #region Load Content

        private void LoadEmptyKeysUI()
        {
            FontManager.DefaultFont = Engine.Instance.Renderer.CreateFont(SystemFontLight);

            Viewport viewport = GraphicsDevice.Viewport;
            basicUI = new BasicUI(viewport.Width, viewport.Height);
            // Load View Model
            viewModel = new BasicUIViewModel();
            basicUI.DataContext = viewModel;

            FontManager.Instance.LoadFonts(Content);
            // Load Image and Sound content if necessary
            //ImageManager.Instance.LoadImages(Content);
            //SoundManager.Instance.LoadSounds(Content);
            
            // This replaces MonoGame's Update exit command
            RelayCommand command = new RelayCommand(new Action<object>(ExitEvent));
            KeyBinding keyBinding = new KeyBinding(command, KeyCode.Escape, ModifierKeys.None);
            basicUI.InputBindings.Add(keyBinding);
        }

        private void ExitEvent(object obj)
        {
            Exit();
        }

        #endregion

        #region Game Controls

        public static void GameRestart()
        {

        }

        #endregion

        #region Player Controls

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

        #endregion
    }
}
