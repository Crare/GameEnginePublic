namespace Pacman
{
    public static class Globals
    {
        public static bool DEBUG_DRAW = false;

        public static int PACMAN_SPEED = 100;
        public static int GHOST_SPEED = 100;
        public static int PACMAN_TILESIZE = 16;

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
            Ghost,
            DotSmall,
            DotBig,
            Wall
        }
    }
}
