using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.FileManagement;
using GameEngine.Core.GameEngine.UI;
using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.GameEngine.Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.GameObjects;
using Pacman.GameObjects.tiles;
using Pacman.Pacman;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pacman
{
    public class MainPacman : Game
    {
        // monogame stuff
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // common core stuff
        public Window _window;
        Point _gameResolution = new(800, 480);
        private EntityManager _entityManager;
        private TextDrawer _textDrawer;
        private KeyboardState _keyboardState;
        private KeyboardState _lastKeyboardState;
        private PacmanUIManager _UIManager;

        // game specific stuff
        private Texture2D _debugTexture;
        private readonly Color _debugColor = new(1f, 0f, 0f, 0.1f);
        private readonly Color _debugColor2 = new(1f, 1f, 1f, 0.5f);
        private PacmanTileMap _tileMap;
        private PacmanPathfinding _pathfinding;
        private readonly UITheme _theme = new()
        {
            Button = new UIElementTheme()
            {
                BackgroundColor = new(0f, 0.8f, 0.8f),
                BackgroundColorPressed = new(0, 1f, 1f),
                BackgroundColorHover = new(0, 0.9f, 0.9f),
                BackgroundColorActive = new(0, 0.9f, 0.9f),

                TextColor = new(1f, 1f, 1f),
                TextColorPressed = new(0.7f, 0.7f, 0.7f),
                TextColorHover = new(0.5f, 0.5f, 0.5f),
                TextColorActive = new(0.5f, 0.5f, 0.5f),

                TextSize = 1f,
                TextSizePressed = 1f,
                TextSizeHover = 1f,
                TextSizeActive = 1f
            },
            Title = new UIElementTheme()
            {
                TextColor = new(1f, 1f, 1f),
                TextSize = 2f
            },
            Text = new UIElementTheme()
            {
                TextColor = new(1f, 1f, 1f),
                TextSize = 1f
            }
        };

        private Globals.PacmanGameState _currentGameState = Globals.PacmanGameState.MainMenu;

        public MainPacman()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            PacmanEventSystem.OnExitGame += OnExitGame;
            PacmanEventSystem.OnGameOver += OnGameOver;
            PacmanEventSystem.OnNewGame += OnNewGame;
            PacmanEventSystem.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnNewGame()
        {
            Globals.GhostsMoving = false;
            _tileMap.LoadLevel(0);
            PacmanEventSystem.GameStateChanged(Globals.PacmanGameState.GameLoop);
        }

        private void OnGameStateChanged(Globals.PacmanGameState state)
        {
            _currentGameState = state;
        }

        private void OnGameOver()
        {
            Globals.GhostsMoving = false;

            if (GameStats.Instance.IsNewHighScore())
            {
                PacmanEventSystem.GameStateChanged(Globals.PacmanGameState.NewHighscore);
            }  else
            {
                PacmanEventSystem.GameStateChanged(Globals.PacmanGameState.GameOver);
            }
        }

        private void OnExitGame()
        {
            Exit();
        }

        protected override void Initialize()
        {
            Debug.WriteLine("Initializing...");
            _window = new Window(_gameResolution,
                    new RenderTarget2D(GraphicsDevice, _gameResolution.X, _gameResolution.Y),
                    _graphics
                );
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _entityManager = new EntityManager(_spriteBatch, _window.RenderTarget);
            _tileMap = new PacmanTileMap(_entityManager);

            var openTileTypes = new int[3];
            openTileTypes[0] = (int)Globals.PacmanTiles.FLOOR;
            openTileTypes[1] = (int)Globals.PacmanTiles.NONE;
            openTileTypes[2] = (int)Globals.PacmanTiles.GATE;
            _pathfinding = new PacmanPathfinding(_tileMap, openTileTypes);

            base.Initialize();
            Debug.WriteLine("Initialization done!");
        }

        protected override void LoadContent()
        {
            Debug.WriteLine("Loading content...");
            // load fonts
            var font = Content.Load<SpriteFont>("Fonts/Arial");
            var defaultTextColor = Color.White;
            _textDrawer = new TextDrawer(_spriteBatch, font, defaultTextColor);
            _UIManager = new PacmanUIManager(_theme, _spriteBatch, _textDrawer, _graphics.GraphicsDevice, _window);

            // load textures
            _debugTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            _debugTexture.SetData(new Color[] { Color.White });
            GameDebug.Init(_spriteBatch, _textDrawer, _debugTexture);

            var pacmanTexture = Content.Load<Texture2D>("sprites/pacman");
            var ghostTexture = Content.Load<Texture2D>("sprites/pacman_ghost");
            var tilesTexture = Content.Load<Texture2D>("sprites/pacman_tiles");
            var dotTexture = Content.Load<Texture2D>("sprites/pacman_dot");

            // load levels
            var levels = new string[1];
            levels[0] = FileSystem.LoadFromFileOrThrowException("levels/level0.csv");

            // assign entities
            var smallDot = new SmallDot(
                new Vector2(_window.RenderTarget.Width / 2, _window.RenderTarget.Height / 2),
                dotTexture
                );
            _entityManager.AddEntity(smallDot);

            var bigDot = new BigDot(
                new Vector2(_window.RenderTarget.Width / 2, _window.RenderTarget.Height / 2),
                dotTexture
                );
            _entityManager.AddEntity(bigDot);

            var pacman = new PacmanEntity(
                new Vector2(_window.RenderTarget.Width / 2, _window.RenderTarget.Height / 2),
                pacmanTexture,
                _pathfinding);
            _entityManager.AddEntity(pacman);

            var redGhost = new RedGhost(
                new Vector2(_window.RenderTarget.Width / 2 + 16, _window.RenderTarget.Height / 2),
                ghostTexture,
                _pathfinding,
                _entityManager);
            _entityManager.AddEntity(redGhost);
            var blueGhost = new BlueGhost(
                new Vector2(_window.RenderTarget.Width / 2 + 32, _window.RenderTarget.Height / 2 ),
                ghostTexture,
                _pathfinding,
                _entityManager);
            _entityManager.AddEntity(blueGhost);
            var OrangeGhost = new OrangeGhost(
                new Vector2(_window.RenderTarget.Width / 2 - 16, _window.RenderTarget.Height / 2),
                ghostTexture,
                _pathfinding,
                _entityManager);
            _entityManager.AddEntity(OrangeGhost);
            var pinkGhost = new PinkGhost(
                new Vector2(_window.RenderTarget.Width / 2 - 32, _window.RenderTarget.Height / 2 ),
                ghostTexture,
                _pathfinding,
                _entityManager);
            _entityManager.AddEntity(pinkGhost);

            // init tilemap
            _tileMap.Initialize(tilesTexture, levels);
            _tileMap.LoadLevel(0);

            //init pathfinding
            _pathfinding.Init();
            _pathfinding.DebugPath();

            // add audio
            // TODO: AUDIO

            //_window.ToggleFullScreen();
            Debug.WriteLine("Loading content done!");
        }

        protected override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || (_keyboardState.IsKeyDown(Keys.Escape) && !_lastKeyboardState.IsKeyDown(Keys.Escape)))
            {
                if (_currentGameState == Globals.PacmanGameState.MainMenu)
                {
                    Exit();
                }
                else
                {
                    PacmanEventSystem.GameStateChanged(Globals.PacmanGameState.MainMenu);
                }
            }

            if (_keyboardState.IsKeyDown(Keys.F11) && !_lastKeyboardState.IsKeyDown(Keys.F11)
                || _keyboardState.IsKeyDown(Keys.I) && !_lastKeyboardState.IsKeyDown(Keys.I))
            {
                _window.ToggleFullScreen();
            }

#if DEBUG
            // Toggle Debug mode: ctrl + shift + D
            if (_keyboardState.IsKeyDown(Keys.D)
                && (_keyboardState.IsKeyDown(Keys.LeftShift) || _keyboardState.IsKeyDown(Keys.RightShift))
                && (_keyboardState.IsKeyDown(Keys.LeftControl) || _keyboardState.IsKeyDown(Keys.RightControl))
                &&
                    !(_lastKeyboardState.IsKeyDown(Keys.D)
                    && (_lastKeyboardState.IsKeyDown(Keys.LeftShift) || _lastKeyboardState.IsKeyDown(Keys.RightShift))
                    && (_lastKeyboardState.IsKeyDown(Keys.LeftControl) || _lastKeyboardState.IsKeyDown(Keys.RightControl))
                    )
                )
            {
                ToggleDebugMode();
            }
#endif

            if (_currentGameState == Globals.PacmanGameState.GameLoop)
            {
                _entityManager.UpdateEntities(gameTime, _keyboardState);
                _tileMap.UpdateTiles(gameTime);
            }

            _UIManager.UpdateUIElements(gameTime, _currentGameState);

            _lastKeyboardState = _keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _window.StartDrawToRenderTarget(_spriteBatch);
            // draw code below

            if (_currentGameState ==  Globals.PacmanGameState.GameLoop
                || _currentGameState == Globals.PacmanGameState.GameOver)
            {
                _tileMap.DrawTiles(_spriteBatch);
                _entityManager.DrawEntities(gameTime);
            }

            if (Globals.DEBUG_DRAW)
            {
                //_tileMap.DebugDrawTiles(_spriteBatch, _textDrawer);
                _entityManager.DebugDrawEntities(_debugTexture, _debugColor, _debugColor2);
                //_pathfinding.DrawDebugNodes(false);
                //_pathfinding.DrawDebugConnections();
                //_pathfinding.DrawDebugPath();
            }

            _UIManager.DrawUIElements(_currentGameState);
            if (Globals.DEBUG_DRAW)
            {
                _UIManager.DebugDrawUIElements(_debugTexture, _debugColor, _debugColor2);
            }

            _textDrawer.Draw($"score: {GameStats.Instance.PlayerScore}",
                new Vector2(_window.RenderTarget.Width / 2, 15),
                HorizontalAlignment.Center,
                VerticalAlignment.Middle);

            // end of draw code
            _window.EndDrawToRenderTarget(_spriteBatch);
            _window.DrawToDestination(_spriteBatch);
            base.Draw(gameTime);
        }

        private void ToggleDebugMode()
        {
            Globals.DEBUG_DRAW = !Globals.DEBUG_DRAW;
        }

        private void OnPressReleaseButton(object sender, EventArgs e)
        {
            Debug.WriteLine("OnPressReleaseButton");
        }
        private void OnPressedDownButton(object sender, EventArgs e)
        {
            Debug.WriteLine("OnPressedDownButton");
        }
    }
}