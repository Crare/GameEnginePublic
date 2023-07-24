using GameEngine.Core.GameEngine.Audio;
using GameEngine.Core.GameEngine.FileManagement;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pong
{
    public class PongScoreStats
    {
        public int Score { get; set; }
        public string Name { get; set; }
    }

    public sealed class GameStats
    {
        private static GameStats instance = null; // singleton
        private static readonly object padlock = new();
        public static GameStats Instance
        {
            get
            {
                // locks the Singleton instance to be once created by locking one of its objects.
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameStats();
                    }
                    return instance;
                }
            }
        }

        GameStats() {
            PongEventSystem.OnNewGame += OnNewGame;
            PongEventSystem.OnStarPicked += OnStarPicked;
        }

        private void OnStarPicked()
        {
            PlayerScore += Globals.SCORE_ON_STAR_PICKED;
            AudioManager.Instance.PlaySound((int)PongSoundEffects.StarPicked);
        }

        private void OnNewGame()
        {
            PlayerLives = Globals.PLAYER_LIVES_AT_START;
            PlayerScore = 0;
            PongEventSystem.GameStateChanged(PongGameState.GameLoop);
        }

        public int PlayerLives = Globals.PLAYER_LIVES_AT_START;
        public int PlayerScore = 0;
        private static string highScoreFileName = "highscores.json";
        public List<PongScoreStats> highScores = new();

        public void LoadHighScores()
        {
            highScores = FileSystem.LoadFromJson<List<PongScoreStats>>(highScoreFileName);
        }

        public void SaveNewHighScore(string name)
        {
            highScores.Add(new PongScoreStats() {
                    Score = PlayerScore,
                    Name = name
                }
            );

            highScores = highScores
                .OrderByDescending(highScores => highScores.Score)
                .ToList();

            FileSystem.SaveAsJson(highScoreFileName, highScores); // simple json saving for now.
        }

        public bool IsNewHighScore()
        {
            return highScores.Any(s => PlayerScore > s.Score) 
                || highScores.Count < 10;
        }
    }
}
