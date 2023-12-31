﻿using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Audio;
using GameEngine.Core.GameEngine.CameraView;
using GameEngine.Core.GameEngine.FileManagement;
using GameEngine.Core.GameEngine.Particles;
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
        public GameEngine.Core.GameEngine.Window.GameWindow _window;
        Point _gameResolution = new(960, 540);
        private EntityManager _entityManager;
        private TextDrawer _textDrawer;
        private KeyboardState _keyboardState;
        private KeyboardState _lastKeyboardState;
        private MouseState _mouseState;
        private PacmanUIManager _UIManager;
        private Camera _camera;

        // game specific stuff
        private Texture2D _debugTexture;
        private readonly Color _debugColor = new(1f, 0f, 0f, 0.1f);
        private readonly Color _debugColor2 = new(1f, 1f, 1f, 0.5f);
        private PacmanTileMap _tileMap;
        private PacmanPathfinding _pathfinding;
        private string[] levels;
        private readonly UITheme _theme = new()
        {
            Button = new UIElementTheme()
            {
                BackgroundColor = new(0f, 0.8f, 0.8f),
                BackgroundColorPressed = new(0, 1f, 1f),
                BackgroundColorHover = new(0, 0.9f, 0.9f),
                BackgroundColorActive = new(0, 0.9f, 0.9f),
                BackgroundColorDisabled = new(0.5f, 0.5f, 0.5f),

                TextColor = new(1f, 1f, 1f),
                TextColorPressed = new(0.7f, 0.7f, 0.7f),
                TextColorHover = new(0.5f, 0.5f, 0.5f),
                TextColorActive = new(0.5f, 0.5f, 0.5f),
                TextColorDisabled = new Color(0.3f, 0.3f, 0.3f),

                TextSize = 1f,
                TextSizePressed = 1f,
                TextSizeHover = 1f,
                TextSizeActive = 1f,
                TextSizeDisabled = 1f,

                HAlign = HorizontalAlignment.Center,
                VAlign = VerticalAlignment.Middle
            },
            Title = new UIElementTheme()
            {
                TextColor = new(1f, 1f, 1f),
                TextSize = 2f,
                HAlign = HorizontalAlignment.Center,
                VAlign = VerticalAlignment.Middle
            },
            Text = new UIElementTheme()
            {
                TextColor = new(1f, 1f, 1f),
                TextSize = 1f,
                HAlign = HorizontalAlignment.Center,
                VAlign = VerticalAlignment.Middle
            },
            Input = new UIElementTheme()
            {
                BorderColor = new(0f, 0.8f, 0.8f),
                BorderColorPressed = new(0, 1f, 1f),
                BorderColorHover = new(0, 0.9f, 0.9f),
                BorderColorActive = new(0, 0.9f, 0.9f),

                BackgroundColor = new(0.9f, 0.9f, 0.9f),
                BackgroundColorPressed = new(0.85f, 0.85f, 0.85f),
                BackgroundColorHover = new(1f, 1f, 1f),
                BackgroundColorActive = new(1f, 1f, 1f),

                PlaceholderTextColor = new(0.8f, 0.8f, 0.8f),

                TextColor = new(0f, 0f, 0f),
                TextColorPressed = new(0.1f, 0.1f, 0.1f),
                TextColorHover = new(0.3f, 0.3f, 0.3f),
                TextColorActive = new(0.3f, 0.3f, 0.3f),

                TextSize = 1f,
                TextSizePressed = 1f,
                TextSizeHover = 1f,
                TextSizeActive = 1f,

                BorderWidth = 3,
                InputPadding = 5,

                HAlign = HorizontalAlignment.Right,
                VAlign = VerticalAlignment.Middle
            }
        };

        private Globals.PacmanGameState _currentGameState = Globals.PacmanGameState.MainMenu;

        public MainPacman()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.IsBorderless = true; // TODO add settings for window layouts

            PacmanEventSystem.OnExitGame += OnExitGame;
            PacmanEventSystem.OnGameOver += OnGameOver;
            PacmanEventSystem.OnNewGame += OnNewGame;
            PacmanEventSystem.OnGameStateChanged += OnGameStateChanged;
            PacmanEventSystem.OnLevelLoaded += OnLevelLoaded;
            PacmanEventSystem.OnLoadLevel += OnLoadLevel;
        }

        private void OnLoadLevel(int level)
        {
            _tileMap.LoadLevel(level);
        }

        private void OnLevelLoaded(int level)
        {
            if (level == 0)
            {
                PacmanEventSystem.NewGame();
            }
            // else TODO!
        }

        private void OnNewGame()
        {
            Globals.GHOSTS_MOVING = false;
            PacmanEventSystem.GameStateChanged(Globals.PacmanGameState.GameLoop);
        }

        private void OnGameStateChanged(Globals.PacmanGameState state)
        {
            _currentGameState = state;
        }

        private void OnGameOver()
        {
            Globals.GHOSTS_MOVING = false;

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
            _window = new GameEngine.Core.GameEngine.Window.GameWindow(_gameResolution,
                    new RenderTarget2D(GraphicsDevice, _gameResolution.X, _gameResolution.Y),
                    _graphics
                );
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            CoreGlobals.SpriteBatch = _spriteBatch;
            CoreGlobals.GraphicsDevice = GraphicsDevice;
            _entityManager = new EntityManager(_spriteBatch, _window.RenderTarget);
            _tileMap = new PacmanTileMap(_entityManager);

            var openTileTypes = new int[3];
            openTileTypes[0] = (int)Globals.PacmanTiles.FLOOR;
            openTileTypes[1] = (int)Globals.PacmanTiles.NONE;
            openTileTypes[2] = (int)Globals.PacmanTiles.GATE;
            _pathfinding = new PacmanPathfinding(_tileMap, openTileTypes);

            ParticleSystem.Instance.Init(_spriteBatch, _graphics);
            GameStats.Instance.LoadHighScores();

            _camera = new(_window)
            {
                // center tilemap to view
                Pos = new Vector2(
                    (int)(_gameResolution.X / 2 - (_tileMap.Width / 1.2f) * _tileMap.TileSize),
                    (int)(_gameResolution.Y / 2 - (_tileMap.Height / 3) * _tileMap.TileSize)
                    )
            };

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
            _UIManager = new PacmanUIManager(_theme, _spriteBatch, _textDrawer, _graphics.GraphicsDevice, _window, _camera);

            // load textures
            _debugTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            _debugTexture.SetData(new Color[] { Color.White });
            GameDebug.Init(_spriteBatch, _textDrawer, _debugTexture);

            var pacmanTexture = Content.Load<Texture2D>("sprites/pacman");
            var ghostTexture = Content.Load<Texture2D>("sprites/pacman_ghost");
            var tilesTexture = Content.Load<Texture2D>("sprites/pacman_tiles");
            var dotTexture = Content.Load<Texture2D>("sprites/pacman_dot");

            // load levels
            levels = new string[1];
            levels[0] = FileSystem.LoadFromFileOrThrowException("levels/level0.csv");

            // assign entities
            var smallDot = new SmallDot(
                // original reference entity, position off the screen
                new Vector2(_window.RenderTarget.Width, _window.RenderTarget.Height),
                dotTexture
                );
            _entityManager.AddEntity(smallDot);

            var bigDot = new BigDot(
                // original reference entity, position off the screen
                new Vector2(_window.RenderTarget.Width, _window.RenderTarget.Height),
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
            //_tileMap.LoadLevel(0);

            //init pathfinding
            _pathfinding.Init();
            _pathfinding.DebugPath();

            // add audio
            Dictionary<int, string> soundEffects = new()
            {
                {
                    (int)Globals.PacmanSoundEffects.pacmanDeath, "Audio/pacmanDeath"
                },
                {
                    (int)Globals.PacmanSoundEffects.pacmanMove, "Audio/pacmanMove"
                },
                {
                    (int)Globals.PacmanSoundEffects.buttonClick, "Audio/buttonClick"
                },
                {
                    (int)Globals.PacmanSoundEffects.ghostDeath, "Audio/ghostDeath"
                },
                {
                    (int)Globals.PacmanSoundEffects.pickupBigDot, "Audio/pickupBigDot"
                },
                {
                    (int)Globals.PacmanSoundEffects.pickupSmallDot, "Audio/pickupSmallDot"
                }
            };
            AudioManager.Instance.Init(soundEffects, Content);

            //_window.ToggleFullScreen();
            Debug.WriteLine("Loading content done!");
        }

        protected override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();

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

            if (_keyboardState.IsKeyDown(Keys.F11) && !_lastKeyboardState.IsKeyDown(Keys.F11))
            {
                _window.ToggleFullScreen();
            }

#if DEBUG
            // Toggle Debug mode
            if (_keyboardState.IsKeyDown(Keys.F1) && !_lastKeyboardState.IsKeyDown(Keys.F1))
            {
                ToggleDebugMode();
            }
#endif

            if (_currentGameState == Globals.PacmanGameState.GameLoop)
            {
                _entityManager.UpdateEntities(gameTime, _keyboardState);
                _tileMap.UpdateTiles(gameTime);
                ParticleSystem.Instance.Update(gameTime);
            }

            if (_keyboardState.IsKeyDown(Keys.NumPad7))
            {
                _camera.Zoom += 0.01f;
            }
            if (_keyboardState.IsKeyDown(Keys.NumPad9))
            {
                _camera.Zoom -= 0.01f;
            }

            if (_keyboardState.IsKeyDown(Keys.NumPad8))
            {
                _camera.Move(new Vector2(0, -1f));
            }
            if (_keyboardState.IsKeyDown(Keys.NumPad2))
            {
                _camera.Move(new Vector2(0, 1f));
            }
            if (_keyboardState.IsKeyDown(Keys.NumPad6))
            {
                _camera.Move(new Vector2(1f, 0));
            }
            if (_keyboardState.IsKeyDown(Keys.NumPad4))
            {
                _camera.Move(new Vector2(-1f, 0));
            }
            if (_keyboardState.IsKeyDown(Keys.NumPad5) && !_lastKeyboardState.IsKeyDown(Keys.NumPad5))
            {
                Debug.WriteLine(
                    $"camera pos: '{_camera.Pos}'\n"+
                    $"camera Zoom: '{_camera.Zoom}'\n"+
                    $"camera transform.Translation: '{_camera._transform.Translation}'\n" + 
                    $"camera rotation: '{_camera.Rotation}'\n"
                    );
            }
            if (_keyboardState.IsKeyDown(Keys.NumPad1) && !_lastKeyboardState.IsKeyDown(Keys.NumPad1))
            {
                // reset zoom
                _camera.Zoom = 1f;
            }
            if (_keyboardState.IsKeyDown(Keys.NumPad3) && !_lastKeyboardState.IsKeyDown(Keys.NumPad3))
            {
                // center tilemap to view
                _camera.Pos = new Vector2(
                    (int)(_gameResolution.X / 2 - (_tileMap.Width / 1.2f) * _tileMap.TileSize),
                    (int)(_gameResolution.Y / 2 - (_tileMap.Height / 3) * _tileMap.TileSize)
                    );
            }

            _UIManager.UpdateUIElements(gameTime, _currentGameState);

            _lastKeyboardState = _keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _window.StartDrawToRenderTarget(_spriteBatch, _camera);
            // draw code below

            if (_currentGameState ==  Globals.PacmanGameState.GameLoop
                || _currentGameState == Globals.PacmanGameState.GameOver)
            {
                _tileMap.DrawTiles();
                _entityManager.DrawEntities(gameTime);
                ParticleSystem.Instance.Draw();
            }

            if (Globals.DEBUG_DRAW)
            {
                //_tileMap.DebugDrawTiles(_spriteBatch, _textDrawer);
                _entityManager.DebugDrawEntities(_debugColor);
                //_pathfinding.DrawDebugNodes(false);
                //_pathfinding.DrawDebugConnections();
                //_pathfinding.DrawDebugPath();
            }

            // end of draw code
            _window.EndDrawToRenderTarget(_spriteBatch, true);
            _window.StartDrawUIToRenderTarget(_spriteBatch);
            // UI-draw code below

            _UIManager.DrawUIElements(_currentGameState, gameTime);
            if (Globals.DEBUG_DRAW)
            {
                //_UIManager.DebugDrawUIElements(_debugTexture, _debugColor, _debugColor2);
                _UIManager.DebugDrawUIElements(_currentGameState, _debugTexture, _debugColor, _debugColor2);
                GameDebug.DebugMousePosition(_mouseState, _debugColor, _debugColor2, _camera);
            }

            // end of UI-draw code
            _window.EndDrawUIToRenderTarget(_spriteBatch);
            _window.DrawToDestination(_spriteBatch);
            base.Draw(gameTime);
        }

        private void ToggleDebugMode()
        {
            Globals.DEBUG_DRAW = !Globals.DEBUG_DRAW;
        }
    }
}