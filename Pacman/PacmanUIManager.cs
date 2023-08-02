using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameEngine.Core.GameEngine.UI;
using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.GameEngine.Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.Pacman;

namespace Pacman
{
    public class PacmanUIManager : UIManager
    {
        public Dictionary<Globals.PacmanGameState, UIElementGroup> ElementGroups;
        public UITheme Theme;
        private string InputText = "";
        private UIInput InputInitials;
        private UIButton SubmitButton;
        private UITextElement ScoreText;
        private UITextElement TimeText;

        public PacmanUIManager(UITheme theme, SpriteBatch spriteBatch, TextDrawer textDrawer, GraphicsDevice graphics,
            Window window
            )
            : base(spriteBatch, textDrawer)
        {
            Theme = theme;
            var mainMenuElements = new List<UIElement>()
            {
                new UITextElement(
                    "PACMAN CLONE",
                    Theme.Title,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter(),
                        (int)window.GetVerticalCenter()  - 50,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI
                ),
                new UIButton(
                    graphics,
                    "Play",
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        (int)window.GetVerticalCenter()  - 20,
                        100, 40
                        ),
                    Theme,
                    (float)Globals.SpriteLayers.UI,
                    default,
                    PacmanEventSystem.NewGame
                ),
                new UIButton(
                    graphics,
                    "Highscores",
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        (int)window.GetVerticalCenter() + 40,
                        100, 40
                        ),
                    Theme,
                    default,
                    () =>
                    {
                        PacmanEventSystem.GameStateChanged(Globals.PacmanGameState.Highscores);
                    }
                ),
                new UIButton(
                    graphics,
                    "Exit Game",
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        (int)window.GetVerticalCenter() +  100,
                        100, 40
                        ),
                    Theme,
                    (float)Globals.SpriteLayers.UI,
                    default,
                    PacmanEventSystem.ExitGame
                )
            };

            ScoreText = new UITextElement(
                "Score: 0",
                Theme.Text,
                graphics,
                new Rectangle(
                    (int)window.GetHorizontalCenter(),
                    20,
                    100, 40
                ),
                (float)Globals.SpriteLayers.UI
            );
            TimeText = new UITextElement(
                "Time: 00:00:00",
                Theme.Text,
                graphics,
                new Rectangle(
                    (int)window.GetHorizontalCenter(),
                    40,
                    100, 40
                ),
                (float)Globals.SpriteLayers.UI
            );

            var gameLoopElements = new List<UIElement>() {
                ScoreText,
                TimeText
            };

            var gameOverElements = new List<UIElement>()
            {
                new UITextElement(
                    "Score: 0",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter(),
                        20,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI
                ),
                new UITextElement(
                    "Time: 0:00:00",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter(),
                        40,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI
                )
            };


            var highscoresElements = new List<UIElement>()
            {
                new UITextElement(
                    "Highscores",
                    Theme.Title,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter(),
                        100,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI
                ),
                new UIButton(
                    graphics,
                    "Back",
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        400,
                        100, 40
                        ),
                    Theme,
                    (float)Globals.SpriteLayers.UI,
                    default,
                    () =>
                    {
                        PacmanEventSystem.GameStateChanged(Globals.PacmanGameState.MainMenu);
                    }
                ),

            };

            InputInitials =
                new UIInput(
                    InputText,
                    "John Doe",
                    Theme.Input,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 100,
                        (int)window.GetVerticalCenter(),
                        200,
                        40),
                    (float)Globals.SpriteLayers.UI,
                    (string newInputText) => UpdateInputText(newInputText)
                );

            SubmitButton = new UIButton(
                graphics,
                "Save",
                new Rectangle(
                        (int)window.GetHorizontalCenter() - 100,
                        (int)window.GetVerticalCenter() + 45,
                        200,
                        40),
                theme,
                (float)Globals.SpriteLayers.UI,
                null,
                () => {
                    GameStats.Instance.SaveNewHighScore(InputText);
                }
            );

            var newHighscoreElements = new List<UIElement>()
            {
                new UITextElement(
                    "New highscore!",
                    Theme.Title,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        70,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Middle
                ),
                new UITextElement(
                    "Score: 0",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        120,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI,
                    HorizontalAlignment.Right,
                    VerticalAlignment.Middle
                ),
                new UITextElement(
                    "Time: 0:00:00",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        140,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI,
                    HorizontalAlignment.Right,
                    VerticalAlignment.Middle
                ),
                new UITextElement(
                    "Add your initials:",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        160,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI,
                    HorizontalAlignment.Right,
                    VerticalAlignment.Middle
                ),
                InputInitials,
                SubmitButton
            };


            ElementGroups = new Dictionary<Globals.PacmanGameState, UIElementGroup>
            {
                { Globals.PacmanGameState.MainMenu, new UIElementGroup(mainMenuElements) },
                { Globals.PacmanGameState.GameLoop, new UIElementGroup(gameLoopElements) },
                { Globals.PacmanGameState.GameOver, new UIElementGroup(gameOverElements) },
                { Globals.PacmanGameState.Highscores, new UIElementGroup(highscoresElements) },
                { Globals.PacmanGameState.NewHighscore, new UIElementGroup(newHighscoreElements) }
            };

            PacmanEventSystem.OnBigDotPicked += ScoreChanged;
            PacmanEventSystem.OnSmallDotPicked += ScoreChanged;
            PacmanEventSystem.OnGhostEaten += ScoreChanged;
            PacmanEventSystem.OnGameStarted += OnGameStarted;
            PacmanEventSystem.OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            GameStats.Instance.ElapsedTime?.Stop();
        }

        private void OnGameStarted()
        {
            GameStats.Instance.ElapsedTime?.Restart();
        }

        private void ScoreChanged()
        {
            ScoreText.Text = $"Score: {GameStats.Instance.PlayerScore}";
        }

        private void UpdateTimeText()
        {
            TimeText.Text = $"Time: {GameStats.Instance.ElapsedTime?.Elapsed.ToString(@"dd\.hh\:mm\:ss\:ff")}";
        }

        public void UpdateUIElements(GameTime gameTime, Globals.PacmanGameState currentGameState)
        {
            ElementGroups[currentGameState].Update(gameTime);
            UpdateTimeText();
        }

        public void DrawUIElements(Globals.PacmanGameState currentGameState, GameTime gameTime)
        {
            ElementGroups[currentGameState].Draw(SpriteBatch, TextDrawer, gameTime);
        }

        public void DebugDrawUIElements(Globals.PacmanGameState currentGameState, Texture2D debugTexture, Color debugColor, Color debugColor2)
        {
            ElementGroups[currentGameState].DebugDraw(SpriteBatch, TextDrawer, debugTexture, debugColor, debugColor2);
        }

        private void UpdateInputText(string newInputText)
        {
            InputText = newInputText;
            InputInitials.UpdateInputText(InputText);
        }
    }
}

