using System;
using GameEngine.Core.GameEngine.Pathfinding;
using Pacman.GameObjects.tiles;
using static Pacman.Globals;

namespace Pacman
{
	public class PacmanPathfinding :  Pathfinding<PacmanTile>
	{
		public PacmanPathfinding(PacmanTileMap tileMap, int[] openTileTypes)
            : base(tileMap, openTileTypes)
		{
        }
    }
}

