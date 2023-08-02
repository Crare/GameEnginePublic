using System.Collections.Generic;
using GameEngine.Core.GameEngine.UI;
using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.GameEngine.Window;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    public class PacmanUIManager : UIManager
    {
        public Dictionary<Globals.PacmanGameState, UIElementGroup> ElementGroups;
        public UITheme Theme;

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


            var gameLoopElements = new List<UIElement>() {
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


            var newHighscoreElements = new List<UIElement>()
            {
                new UITextElement(
                    "New highscore!",
                    Theme.Title,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter(),
                        100,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI
                ),
                new UITextElement(
                    "Score: 0",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter(),
                        120,
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
                        140,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI
                ),
                new UITextElement(
                    "Add your initials:",
                    Theme.Text,
                    graphics,
                    new Rectangle(
                        (int)window.GetHorizontalCenter(),
                        160,
                        100, 40
                    ),
                    (float)Globals.SpriteLayers.UI
                ),
            };


            ElementGroups = new Dictionary<Globals.PacmanGameState, UIElementGroup>
            {
                { Globals.PacmanGameState.MainMenu, new UIElementGroup(mainMenuElements) },
                { Globals.PacmanGameState.GameLoop, new UIElementGroup(gameLoopElements) },
                { Globals.PacmanGameState.GameOver, new UIElementGroup(gameOverElements) },
                { Globals.PacmanGameState.Highscores, new UIElementGroup(highscoresElements) },
                { Globals.PacmanGameState.NewHighscore, new UIElementGroup(newHighscoreElements) }
            };
        }

        public void UpdateUIElements(GameTime gameTime, Globals.PacmanGameState currentGameState)
        {
            ElementGroups[currentGameState].Update(gameTime);
        }

        public void DrawUIElements(Globals.PacmanGameState currentGameState)
        {
            ElementGroups[currentGameState].Draw(SpriteBatch, TextDrawer);
        }
    }
}

