using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Audio;
using GameEngine.Core.GameEngine.InputManagement;
using GameEngine.Core.GameEngine.Particles;
using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pong
{
    public class Game1 : Game
    {
        Texture2D ballTexture;
        Texture2D debugTexture;
        Color debugColor;

        public string PlayerInputText = "";

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private KeyboardState keyboardState;
        private KeyboardState lastKeyboardState;

        private EntityManager _entityManager;
        private TextDrawer _textDrawer;

        private PongGameState _currentGameState = PongGameState.Scoreboard;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            PongEventSystem.OnGameStateChanged += OnGameStateChanged;
            PongEventSystem.OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            AudioManager.Instance.PlaySound((int)PongSoundEffects.GameOver);
            ParticleEvents.ParticlesReset();
            if (GameStats.Instance.IsNewHighScore())
            {
                PongEventSystem.GameStateChanged(PongGameState.AddNewHighScore);

            } else
            {
                PongEventSystem.GameStateChanged(PongGameState.Scoreboard);
            }
        }

        private void OnGameStateChanged(PongGameState state)
        {
            _currentGameState = state;
        }

        protected override void Initialize()
        {
            Debug.WriteLine("Initializing...");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _entityManager = new EntityManager(_spriteBatch, _graphics);
            ParticleSystem.Instance.Init(_spriteBatch, _graphics);
            GameStats.Instance.LoadHighScores();

            base.Initialize();
            Debug.WriteLine("Initialization done!");
        }

        protected override void LoadContent()
        {
            Debug.WriteLine("Loading content...");

            var font = Content.Load<SpriteFont>("Fonts/Arial");
            var defaultTextColor = Color.White;
            _textDrawer = new TextDrawer(_spriteBatch, font, defaultTextColor);

            // load textures
            ballTexture = Content.Load<Texture2D>("Sprites/ball");

            // generated textures
            debugTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            debugTexture.SetData(new Color[] { Color.White });

            var paddleTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            paddleTexture.SetData(new Color[] { Color.Blue });

            // set colors
            debugColor = new Color(1f, 0f, 0f, 0.3f);

            // assign entities
            var ballEntity = new Ball(
                new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2),
                new Rectangle(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2, ballTexture.Width, ballTexture.Height),
                new Sprite(ballTexture)
            );
            _entityManager.AddEntity(ballEntity);

            var leftPaddle = new Paddle(
                paddleTexture,
                10,
                100,
                new Vector2(5, _graphics.PreferredBackBufferHeight / 2),
                "leftPaddle",
                new Rectangle(5, _graphics.PreferredBackBufferHeight / 2, 10, 100),
                true
            );
            _entityManager.AddEntity(leftPaddle);

            var rightPaddle = new Paddle(
                paddleTexture,
                10,
                100,
                new Vector2(_graphics.PreferredBackBufferWidth - 15, _graphics.PreferredBackBufferHeight / 2),
                "rightPaddle",
                new Rectangle(_graphics.PreferredBackBufferWidth - 15, _graphics.PreferredBackBufferHeight / 2, 10, 100),
                false
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
                }
            };
            AudioManager.Instance.Init(soundEffects);

            Debug.WriteLine("Loading content done!");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (_currentGameState == PongGameState.GameLoop)
            {

                _entityManager.UpdateEntities(gameTime, keyboardState);
                ParticleSystem.Instance.Update(gameTime);

                if (GameStats.Instance.PlayerLives <= 0)
                {
                    PongEventSystem.GameOver();
                }

            } else if (_currentGameState == PongGameState.Scoreboard)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    PongEventSystem.NewGame();
                }
            } else if (_currentGameState == PongGameState.AddNewHighScore)
            {

                if (TextInputHelper.TryGetPressedKey(keyboardState, lastKeyboardState, out string key))
                {
                    // allow only 3 letters to be added.
                    if (key == "backspace" && PlayerInputText.Length > 0)
                    {
                        PlayerInputText = PlayerInputText.Substring(0, PlayerInputText.Length-1);
                    } else if (key != "backspace" && PlayerInputText.Length < 3)
                    {
                        PlayerInputText += key;
                    }
                }
                if (PlayerInputText.Length == 3 && keyboardState.IsKeyDown(Keys.Enter))
                {
                    GameStats.Instance.SaveNewHighScore(PlayerInputText);
                    PongEventSystem.GameStateChanged(PongGameState.Scoreboard);
                }
            }

            lastKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            if (_currentGameState == PongGameState.GameLoop)
            {
                _entityManager.RenderEntities();
                ParticleSystem.Instance.Draw();

                if (Globals.DEBUG_DRAW)
                {
                    _entityManager.DebugRenderEntities(debugTexture, debugColor);
                }

                DrawCurrentScore();
            }
            else if (_currentGameState == PongGameState.Scoreboard)
            {
                DrawCurrentScore();
                DrawHighScores();

                string help = $"Start new game by pressing 'Space'";
                _textDrawer.Draw(help, new Vector2(_graphics.PreferredBackBufferWidth / 2, 60), Alignment.Center);
            }
            else if (_currentGameState == PongGameState.AddNewHighScore)
            {
                string help1 = $"You got a new highscore!";
                string help2 = $"Please enter 3 characters and press 'Enter':";
                string help3 = $"{PlayerInputText}";
                _textDrawer.Draw(help1, new Vector2(_graphics.PreferredBackBufferWidth / 2, 60), Alignment.Center);
                _textDrawer.Draw(help2, new Vector2(_graphics.PreferredBackBufferWidth / 2, 80), Alignment.Center);
                _textDrawer.Draw(help3, new Vector2(_graphics.PreferredBackBufferWidth / 2, 100), Alignment.Center);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawCurrentScore()
        {
            string lives = $"lives: {GameStats.Instance.PlayerLives}";
            string score = $"score: {GameStats.Instance.PlayerScore}";
            _textDrawer.Draw(lives, new Vector2(_graphics.PreferredBackBufferWidth / 2, 10), Alignment.Center);
            _textDrawer.Draw(score, new Vector2(_graphics.PreferredBackBufferWidth / 2, 30), Alignment.Center);
        }

        private void DrawHighScores()
        {
            string highscores = $"highscores:";
            _textDrawer.Draw(highscores, new Vector2(_graphics.PreferredBackBufferWidth / 2, 100), Alignment.Center);
            for (int i = 0; i < GameStats.Instance.highScores.Count;i++)
            {
                string name = $"{i + 1}. {GameStats.Instance.highScores[i].Name}";
                string score = $"{GameStats.Instance.highScores[i].Score}";
                _textDrawer.Draw(name, new Vector2(_graphics.PreferredBackBufferWidth / 2 - 40, 125 + i * 20), Alignment.Right);
                _textDrawer.Draw(score, new Vector2(_graphics.PreferredBackBufferWidth / 2 + 20, 125 + i * 20), Alignment.Right);
            }
        }
    }
}