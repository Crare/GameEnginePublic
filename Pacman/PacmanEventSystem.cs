using System;
using static Pacman.Globals;

namespace Pacman
{
    public static class PacmanEventSystem
    {
        public static event Action OnGameStarted;
        public static void GameStarted() => OnGameStarted?.Invoke();

        public static event Action OnExitGame;
        public static void ExitGame() => OnExitGame?.Invoke();

        public static event Action OnGameOver;
        public static void GameOver() => OnGameOver?.Invoke();

        public static event Action OnNewGame;
        public static void NewGame() => OnNewGame?.Invoke();

        public static event Action<PacmanGameState> OnGameStateChanged;
        public static void GameStateChanged(PacmanGameState state) => OnGameStateChanged?.Invoke(state);

        public static event Action OnSmallDotPicked;
        public static void SmallDotPicked() => OnSmallDotPicked?.Invoke();

        public static event Action OnBigDotPicked;
        public static void BigDotPicked() => OnBigDotPicked?.Invoke();
    }
}

