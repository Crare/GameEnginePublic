using System;
namespace Pacman
{
    using GameEngine.Core.GameEngine.Audio;
    using GameEngine.Core.GameEngine.FileManagement;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using static global::Pacman.Globals;

    namespace Pacman
    {
        public class PacmanScoreStats
        {
            public int Score { get; set; }
            public string Name { get; set; }
            public TimeSpan ElapsedTime { get; set; }
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

            GameStats()
            {
                PacmanEventSystem.OnNewGame += OnNewGame;
                PacmanEventSystem.OnBigDotPicked += OnBigDotPicked;
                PacmanEventSystem.OnSmallDotPicked += OnSmallDotPicked;
            }

            private void OnBigDotPicked()
            {
                PlayerScore += Globals.SCORE_ON_BIG_DOT_PICKED;
                //AudioManager.Instance.PlaySound((int)PongSoundEffects.StarPicked);
            }

            private void OnSmallDotPicked()
            {
                PlayerScore += Globals.SCORE_ON_SMALL_DOT_PICKED;
                //AudioManager.Instance.PlaySound((int)PongSoundEffects.StarPicked);
            }

            private void OnNewGame()
            {
                //PlayerLives = Globals.PLAYER_LIVES_AT_START;
                PlayerScore = 0;
            }

            //public int PlayerLives = Globals.PLAYER_LIVES_AT_START;
            public int PlayerScore = 0;
            private static string highScoreFileName = "highscores.json";
            public List<PacmanScoreStats> highScores = new();
            public Stopwatch ElapsedTime;

            public void LoadHighScores()
            {
                highScores = FileSystem.LoadFromJson<List<PacmanScoreStats>>(highScoreFileName) ?? new();
            }

            public void SaveNewHighScore(string name)
            {
                highScores.Add(
                    new PacmanScoreStats()
                    {
                        Score = PlayerScore,
                        Name = name,
                        ElapsedTime = ElapsedTime.Elapsed
                    }
                );

                highScores = highScores
                    .OrderByDescending(highScores => highScores.Score)
                    .ToList();

                FileSystem.SaveAsJson(highScoreFileName, highScores); // simple json saving for now.

                PacmanEventSystem.GameStateChanged(PacmanGameState.Highscores);
            }

            public bool IsNewHighScore()
            {
                return highScores.Any(s => PlayerScore > s.Score)
                    || highScores.Count < 10;
            }
        }
    }

}

