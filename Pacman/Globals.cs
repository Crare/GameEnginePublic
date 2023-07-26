using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public static class Globals
    {
        public static int PACMAN_SPEED = 100;

        public enum SpriteLayers
        {
            BACKGROUND,
            MIDDLEGROUND,
            FOREGROUND
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
