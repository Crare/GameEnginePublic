using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    public static class PongEventSystem
    {
        public static event Action<PongGameState> OnGameStateChanged;
        public static void GameStateChanged(PongGameState state) => OnGameStateChanged?.Invoke(state);

        public static event Action OnGameOver;
        public static void GameOver() => OnGameOver?.Invoke();

        public static event Action OnNewGame;
        public static void NewGame() => OnNewGame?.Invoke();
    }
}
