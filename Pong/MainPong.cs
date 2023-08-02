using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Audio;
using GameEngine.Core.GameEngine.InputManagement;
using GameEngine.Core.GameEngine.Particles;
using GameEngine.Core.GameEngine.Sprites;
using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.GameEngine.Window;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pong
{
    public class MainPong : Game
    {
        // monogame stuff
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // common core stuff
        public Window _window;
        Point _gameResolution = new Point(800, 480);
        private EntityManager _entityManager;
        private TextDrawer _textDrawer;
        private KeyboardState _keyboardState;
        private KeyboardState _lastKeyboardState;

        // game specific stuff
        private Texture2D _debugTexture;
        private Color _debugColor = new Color(1f, 0f, 0f, 0.1f);
        private Color _debugColor2 = new Color(1f, 1f, 1f, 0.5f);
        private ScrollingBackground _starBackground;
        private Texture2D _pongSpritesheet;
        private bool playOneUpdateOnPaused = false;
        public string _playerInputText = "";
        private PongGameState _currentGameState = PongGameState.Scoreboard;

        public MainPong()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            PongEventSystem.OnGameStateChanged += OnGameStateChanged;
            PongEventSystem.OnGameOver += OnGameOver;
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

            ParticleSystem.Instance.Init(_spriteBatch, _graphics);
            GameStats.Instance.LoadHighScores();

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

            // load textures
            //var ballTexture = Content.Load<Texture2D>("Sprites/ball");
            //var paddleTexture = Content.Load<Texture2D>("Sprites/paddle");
            _pongSpritesheet = Content.Load<Texture2D>("Sprites/pong_spritesheet");

            // generated textures
            _debugTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            _debugTexture.SetData(new Color[] { Color.White });

            _starBackground = new ScrollingBackground(_window.RenderTarget);

            // assign entities
            var starAmount = 100;
            var edgePadding = 20;
            for (int i = 0; i < starAmount; i++)
            {
                var rnd = new Random();
                var randX = rnd.Next(edgePadding, _window.RenderTarget.Width - edgePadding);
                var randY = rnd.Next(edgePadding, _window.RenderTarget.Height - edgePadding);
                var randFrame = rnd.Next(0, 4);
                var starAnimationRects = new Rectangle[4];
                starAnimationRects[0] = new Rectangle(16, 16, 2, 2);
                starAnimationRects[1] = new Rectangle(26, 18, 4, 4);
                starAnimationRects[2] = new Rectangle(18, 16, 6, 6);
                starAnimationRects[3] = new Rectangle(24, 16, 8, 8);

                var star = new Star(
                    new Vector2(randX, randY), 
                    new Rectangle(randX-2, randY-2, 4,4), 
                    new SpriteAnimation(_pongSpritesheet, randFrame, 16, true, starAnimationRects));
                _entityManager.AddEntity(star);
            }
            var ballDimensions = new Point(16,16);
            var ball = new Ball(
                new Vector2(_window.RenderTarget.Width / 2, _window.RenderTarget.Height / 2),
                new Rectangle(_window.RenderTarget.Width / 2, _window.RenderTarget.Height / 2, ballDimensions.X, ballDimensions.Y),
                new Sprite(_pongSpritesheet, new Rectangle(16,0, 16,16))
            );
            _entityManager.AddEntity(ball);

            var paddleDimensions = new Point(16, 96);
            var leftPaddle = new PlayerPaddle(
                new Sprite(_pongSpritesheet, new Rectangle(0,0, paddleDimensions.X, paddleDimensions.Y)),
                new Vector2(paddleDimensions.X, _window.RenderTarget.Height / 2)
            );
            _entityManager.AddEntity(leftPaddle);

            var rightPaddle = new AIPaddle(
                new Sprite(_pongSpritesheet, new Rectangle(0, 0, paddleDimensions.X, paddleDimensions.Y)),
                new Vector2(_window.RenderTarget.Width - paddleDimensions.X, _window.RenderTarget.Height / 2)
            );
            _entityManager.AddEntity(rightPaddle);

            // add audio
            Dictionary<int, SoundEffect> soundEffects = new()
            {
                {
                    (int)PongSoundEffects.BallHit, Content.Load<SoundEffect>("Audio/hit")
                },
                {
                    (int)PongSoundEffects.PaddleHit, Content.Load<SoundEffect>("Audio/paddle")
                },
                {
                    (int)PongSoundEffects.GameOver, Content.Load<SoundEffect>("Audio/gameOver")
                },
                {
                    (int)PongSoundEffects.LiveLost, Content.Load<SoundEffect>("Audio/liveLost")
                },
                {
                    (int)PongSoundEffects.Scored, Content.Load<SoundEffect>("Audio/scored")
                },
                {
                    (int)PongSoundEffects.StarPicked, Content.Load<SoundEffect>("Audio/starPicked")
                }
            };
            AudioManager.Instance.Init(soundEffects);

            Debug.WriteLine("Loading content done!");
        }

        protected override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || _keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Toggle Pause state: P
            if (_keyboardState.IsKeyDown(Keys.P) 
                && !_lastKeyboardState.IsKeyDown(Keys.P) 
                && (_currentGameState == PongGameState.GameLoop || _currentGameState == PongGameState.GamePaused))
            {
                var newState = _currentGameState == PongGameState.GamePaused ? PongGameState.GameLoop : PongGameState.GamePaused;
                PongEventSystem.GameStateChanged(newState);
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

            if (_keyboardState.IsKeyDown(Keys.F11) && !_lastKeyboardState.IsKeyDown(Keys.F11)
                || _keyboardState.IsKeyDown(Keys.I) && !_lastKeyboardState.IsKeyDown(Keys.I))
            {
                _window.ToggleFullScreen();
            }

#if DEBUG
            if (_keyboardState.IsKeyDown(Keys.O) && !_lastKeyboardState.IsKeyDown(Keys.O))
            {
                playOneUpdateOnPaused = true;
            }
#endif

            // Pause mode on: no updates to be done!
            if (_currentGameState == PongGameState.GamePaused && !playOneUpdateOnPaused)
            {
                _lastKeyboardState = _keyboardState;
                base.Update(gameTime);
                return;
            }

            _starBackground.Update(gameTime, _window.RenderTarget);

            if (_currentGameState == PongGameState.GameLoop 
                || (_currentGameState == PongGameState.GamePaused && playOneUpdateOnPaused))
            {
                _entityManager.UpdateEntities(gameTime, _keyboardState);
                ParticleSystem.Instance.Update(gameTime);

                if (GameStats.Instance.PlayerLives <= 0)
                {
                    PongEventSystem.GameOver();
                }

            } else if (_currentGameState == PongGameState.Scoreboard)
            {
                if (_keyboardState.IsKeyDown(Keys.Space))
                {
                    PongEventSystem.NewGame();
                }
            } else if (_currentGameState == PongGameState.AddNewHighScore)
            {

                if (TextInputHelper.TryGetPressedKey(_keyboardState, _lastKeyboardState, out string key))
                {
                    // allow only 3 letters to be added.
                    if (key == "backspace" && _playerInputText.Length > 0)
                    {
                        _playerInputText = _playerInputText.Substring(0, _playerInputText.Length-1);
                    } else if (key != "backspace" && _playerInputText.Length < 3)
                    {
                        _playerInputText += key;
                    }
                }
                if (_playerInputText.Length == 3 && _keyboardState.IsKeyDown(Keys.Enter))
                {
                    GameStats.Instance.SaveNewHighScore(_playerInputText);
                    PongEventSystem.GameStateChanged(PongGameState.Scoreboard);
                }
            }

            _lastKeyboardState = _keyboardState;

            base.Update(gameTime);

            if (playOneUpdateOnPaused) {
                playOneUpdateOnPaused = false;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            _window.StartDrawToRenderTarget(_spriteBatch);
            // draw code below

            _starBackground.Render(_spriteBatch);

            if (_currentGameState == PongGameState.GameLoop || _currentGameState == PongGameState.GamePaused)
            {
                _entityManager.DrawEntities(gameTime);
                ParticleSystem.Instance.Draw();

                if (Globals.DEBUG_DRAW)
                {
                    _entityManager.DebugDrawEntities(_debugTexture, _debugColor, _debugColor2);
                }

                DrawCurrentScore();
            }
            else if (_currentGameState == PongGameState.Scoreboard)
            {
                DrawCurrentScore();
                DrawHighScores();

                string help = $"Start new game by pressing 'Space'";
                _textDrawer.Draw(help, new Vector2(_window.RenderTarget.Width / 2, 60), HorizontalAlignment.Center);
            }
            else if (_currentGameState == PongGameState.AddNewHighScore)
            {
                string help1 = $"You got a new highscore!";
                string help2 = $"Please enter 3 characters and press 'Enter':";
                string help3 = $"{_playerInputText}";
                _textDrawer.Draw(help1, new Vector2(_window.RenderTarget.Width / 2, 60), HorizontalAlignment.Center);
                _textDrawer.Draw(help2, new Vector2(_window.RenderTarget.Width / 2, 80), HorizontalAlignment.Center);
                _textDrawer.Draw(help3, new Vector2(_window.RenderTarget.Width / 2, 100), HorizontalAlignment.Center);
            }

            if (_currentGameState == PongGameState.GamePaused)
            {
                string paused = $"Game paused";
                _textDrawer.Draw(paused, new Vector2(_window.RenderTarget.Width / 2, _window.RenderTarget.Height / 2), HorizontalAlignment.Center);
            }

            // end of draw code
            _window.EndDrawToRenderTarget(_spriteBatch);
            _window.DrawToDestination(_spriteBatch);
            base.Draw(gameTime);
        }

        private void OnGameOver()
        {
            AudioManager.Instance.PlaySound((int)PongSoundEffects.GameOver);
            ParticleEvents.ParticlesReset();
            if (GameStats.Instance.IsNewHighScore())
            {
                PongEventSystem.GameStateChanged(PongGameState.AddNewHighScore);

            }
            else
            {
                PongEventSystem.GameStateChanged(PongGameState.Scoreboard);
            }
        }

        private void OnGameStateChanged(PongGameState state)
        {
            _currentGameState = state;
        }

        private void DrawCurrentScore()
        {
            string lives = $"lives: {GameStats.Instance.PlayerLives}";
            string score = $"score: {GameStats.Instance.PlayerScore}";
            _textDrawer.Draw(lives, new Vector2(_window.RenderTarget.Width / 2, 10), HorizontalAlignment.Center);
            _textDrawer.Draw(score, new Vector2(_window.RenderTarget.Width / 2, 30), HorizontalAlignment.Center);
        }

        private void DrawHighScores()
        {
            string highscores = $"highscores:";
            _textDrawer.Draw(highscores, new Vector2(_window.RenderTarget.Width / 2, 100), HorizontalAlignment.Center);
            for (int i = 0; i < GameStats.Instance.highScores.Count;i++)
            {
                string name = $"{i + 1}. {GameStats.Instance.highScores[i].Name}";
                string score = $"{GameStats.Instance.highScores[i].Score}";
                _textDrawer.Draw(name, new Vector2(_window.RenderTarget.Width / 2 - 40, 125 + i * 20), HorizontalAlignment.Right);
                _textDrawer.Draw(score, new Vector2(_window.RenderTarget.Width / 2 + 20, 125 + i * 20), HorizontalAlignment.Right);
            }
        }

        private void ToggleDebugMode()
        {
            Globals.DEBUG_DRAW = !Globals.DEBUG_DRAW;
        }
    }
}