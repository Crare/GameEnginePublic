namespace Pacman
{
    public static class Globals
    {
        public static bool DEBUG_DRAW = false;

        public static int PACMAN_SPEED = 20;
        public static int PACMAN_SPEED_WHEN_INVULNERABLE = 30;
        public static int GHOST_SPEED = 20;
        public static int PACMAN_TILESIZE = 16;

        public static int SCORE_ON_SMALL_DOT_PICKED = 33;
        public static int SCORE_ON_BIG_DOT_PICKED = 150;

        public static bool GameStarted = false;

        public enum PacmanGameState
        {
            MainMenu,
            GameLoop,
            GameOver,
            NewHighscore,
            Highscores
        }

        public enum PacmanTiles
        {
            NONE = 0,
            FLOOR,
            WALL,
            GATE
        }

        public enum SpriteLayers
        {
            BACKGROUND,
            MIDDLEGROUND,
            FOREGROUND,
            UI
        }

        public enum PacmanTags
        {
            None = 0,
            Pacman,
            RedGhost,
            BlueGhost,
            PinkGhost,
            OrangeGhost,
            DotSmall,
            DotBig,
            Wall
        }
    }
}
