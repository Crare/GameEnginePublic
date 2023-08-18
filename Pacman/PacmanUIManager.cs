using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameEngine.Core.GameEngine.CameraView;
using GameEngine.Core.GameEngine.UI;
using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.GameEngine.Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private UITextElement HighscoresText1;
        private UITextElement HighscoresText2;
        private UITextElement HighscoresText3;

        public PacmanUIManager(UITheme theme, SpriteBatch spriteBatch, TextDrawer textDrawer, GraphicsDevice graphics,
            GameEngine.Core.GameEngine.Window.GameWindow window, Camera camera
            )
            : base(spriteBatch, textDrawer, camera)
        {
            Theme = theme;
            var mainMenuElements = new List<UIElement>()
            {
                new UITextElement(
                    "PACMAN CLONE",
                    Theme.Title,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        (int)window.GetVerticalCenter()  - 80,
                        100, 40
                    ),
                    (float)Globals.SPRITE_LAYER_UI
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
                    (float)Globals.SPRITE_LAYER_UI,
                    null,
                    () => PacmanEventSystem.LoadLevel(0),
                    (int)Globals.PacmanSoundEffects.buttonClick
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
                    (float)Globals.SPRITE_LAYER_UI,
                    null,
                    () => PacmanEventSystem.GameStateChanged(Globals.PacmanGameState.Highscores),
                    (int)Globals.PacmanSoundEffects.buttonClick
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
                    (float)Globals.SPRITE_LAYER_UI,
                    null,
                    PacmanEventSystem.ExitGame,
                    (int)Globals.PacmanSoundEffects.buttonClick
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
                (float)Globals.SPRITE_LAYER_UI
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
                (float)Globals.SPRITE_LAYER_UI
            );

            var gameLoopElements = new List<UIElement>() {
                ScoreText,
                TimeText
            };

            var gameOverElements = new List<UIElement>()
            {
                ScoreText,
                TimeText,
                new UITextElement(
                    "GAME OVER",
                    Theme.Title,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        150,
                        100, 40
                    ),
                    (float)Globals.SPRITE_LAYER_UI
                ),
                new UITextElement(
                "Press Escape to go back to menu",
                Theme.Text,
                graphics,
                new Rectangle(
                    (int)window.GetHorizontalCenter(),
                    100,
                    100, 40
                ),
                (float)Globals.SPRITE_LAYER_UI
            )
        };

            HighscoresText1 = new UITextElement(
                "no scores yet",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 150,
                        140,
                        100, 40
                    ),
                    (float)Globals.SPRITE_LAYER_UI,
                    HorizontalAlignment.Right,
                    VerticalAlignment.Bottom
                );
            HighscoresText2 = new UITextElement(
                "",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter(),
                        140,
                        100, 40
                    ),
                    (float)Globals.SPRITE_LAYER_UI,
                    HorizontalAlignment.Right,
                    VerticalAlignment.Bottom
                );
            HighscoresText3 = new UITextElement(
                "",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() + 100,
                        140,
                        100, 40
                    ),
                    (float)Globals.SPRITE_LAYER_UI,
                    HorizontalAlignment.Right,
                    VerticalAlignment.Bottom
                );

            var highscoresElements = new List<UIElement>()
            {
                new UITextElement(
                    "Highscores",
                    Theme.Title,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        100,
                        100, 40
                    ),
                    (float)Globals.SPRITE_LAYER_UI
                ),
                HighscoresText1,
                HighscoresText2,
                HighscoresText3,
                new UIButton(
                    graphics,
                    "Back",
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        400,
                        100, 40
                        ),
                    Theme,
                    (float)Globals.SPRITE_LAYER_UI,
                    null,
                    () => PacmanEventSystem.GameStateChanged(Globals.PacmanGameState.MainMenu),
                    (int)Globals.PacmanSoundEffects.buttonClick
                ),

            };

            InputInitials =
                new UIInput(
                    InputText,
                    "",
                    Theme.Input,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 100,
                        (int)window.GetVerticalCenter(),
                        200,
                        40),
                    (float)Globals.SPRITE_LAYER_UI,
                    UpdateInputText
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
                    (float)Globals.SPRITE_LAYER_UI,
                    null,
                    SaveNewScore,
                    (int)Globals.PacmanSoundEffects.buttonClick,
                    true // disabled
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
                    (float)Globals.SPRITE_LAYER_UI,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Middle
                ),
                ScoreText,
                TimeText,
                new UITextElement(
                    "Add your initials:",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter() - 50,
                        160,
                        100, 40
                    ),
                    (float)Globals.SPRITE_LAYER_UI,
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
            PacmanEventSystem.OnGameStateChanged += UpdateHighscoresText;
        }

        private void SaveNewScore()
        {
            GameStats.Instance.SaveNewHighScore(InputText);
            UpdateInputText("");
        }

        private void ScoreChanged()
        {
            ScoreText.SetText($"Score: {GameStats.Instance.PlayerScore}");
        }

        private void UpdateTimeText()
        {
            TimeText.SetText($"Time: {GameStats.Instance.ElapsedTime?.Elapsed.ToString(@"mm\:ss\:ff")}");
        }

        private void UpdateHighscoresText(Globals.PacmanGameState gameState)
        {
            if (gameState != Globals.PacmanGameState.Highscores) {
                return;
            }
            var text1 = "name\n\n";
            var text2 = "score\n\n";
            var text3 = "time\n\n";
            var i = 1;
            GameStats.Instance.highScores.ForEach(hs =>
            {
                text1 += $"{i}. {hs.Name}\n";
                text2 += $"{hs.Score}\n";
                text3 += $"{hs.ElapsedTime.ToString(@"mm\:ss\:ff")}\n";
                i++;
            });
            HighscoresText1.SetText(text1);
            HighscoresText2.SetText(text2);
            HighscoresText3.SetText(text3);
        }

        public void UpdateUIElements(GameTime gameTime, Globals.PacmanGameState currentGameState)
        {
            ElementGroups[currentGameState].Update(gameTime, Camera);
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
            SubmitButton.SetDisabled(!(InputText.Length > 0 && InputText.Length <= 8));
        }
    }
}

