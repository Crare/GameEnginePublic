namespace Pong
{
    public static class Globals
    {
        public static bool DEBUG_DRAW = false;

        public static readonly int PLAYER_BASE_SPEED = 100;
        public static readonly int PADDLE_BASE_SPEED = 200;
        public static readonly int BALL_BASE_SPEED = 150;
        public static readonly int BALL_MAX_SPEED = 300;

        public static readonly int PLAYER_LIVES_AT_START = 3;

        public static readonly int SCORE_ON_WALL_HIT = 100;
        public static readonly int SCORE_ON_PADDLE_HIT = 50;

        public enum SpriteLayers {
            BACKGROUND,
            PLAYER,
            FOREGROUND
        }
    }
}
