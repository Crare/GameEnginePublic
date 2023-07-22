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

        GameStats() {
            // dummy data
            highScores = new List<PongScoreStats> {
                new PongScoreStats()
                {
                    Score = 100,
                    Name = "nub"
                },
                new PongScoreStats()
                {
                    Score = 200,
                    Name = "cas"
                },
                new PongScoreStats()
                {
                    Score = 1000,
                    Name = "pro"
                }
            }.OrderByDescending(s => s.Score).ToList();

            PongEventSystem.OnNewGame += OnNewGame;
        }

        private void OnNewGame()
        {
            PlayerLives = Globals.PLAYER_LIVES_AT_START;
            PlayerScore = 0;
            PongEventSystem.GameStateChanged(PongGameState.GameLoop);
        }

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

        public int PlayerLives = Globals.PLAYER_LIVES_AT_START;
        public int PlayerScore = 0;

        private static string highScoreFileName = "highscores.json";

        public List<PongScoreStats> highScores = new(); // TODO: load from file/memory.

        public void LoadHighScores()
        {
            highScores = File.Exists(highScoreFileName) ? JsonSerializer.Deserialize<List<PongScoreStats>>(File.ReadAllText(highScoreFileName)) : new();
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

            File.WriteAllText(highScoreFileName, JsonSerializer.Serialize(highScores));
        }

        public bool IsNewHighScore()
        {
            return highScores.Any(s => PlayerScore > s.Score) 
                || highScores.Count < 10;
        }
    }
}
