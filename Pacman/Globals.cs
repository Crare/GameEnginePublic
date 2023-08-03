namespace Pacman
{
    public static class Globals
    {
        public static bool DEBUG_DRAW = false;
        public static bool GHOSTS_MOVING = false;

        public static readonly int PACMAN_SPEED = 20;
        public static readonly int PACMAN_SPEED_WHEN_INVULNERABLE = 20;
        public static readonly int GHOST_SPEED = 20;
        public static readonly int PACMAN_TILESIZE = 16;

        public static readonly int VULNERABLE_SECONDS = 8;
        public static readonly float UPDATE_PATH_SECONDS = 1;

        public static readonly int SCORE_ON_SMALL_DOT_PICKED = 33;
        public static readonly int SCORE_ON_BIG_DOT_PICKED = 150;
        public static readonly int SCORE_ON_GHOST_EATEN = 1000;


        public enum PacmanSoundEffects
        {
            buttonClick,
            ghostDeath,
            pacmanDeath,
            pacmanMove,
            pickupBigDot,
            pickupSmallDot,
        }

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
