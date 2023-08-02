using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameEngine.Core.GameEngine.Pathfinding
{
	public class PathNode
	{
		// grid position
		public int X, Y;

		// actual world position with tilesize offset
		public Vector2 Position;

		public List<PathNode> Neighbours;

		public PathNode(Vector2 position, int x, int y)
		{
			Position = position;
			X = x;
			Y = y;
        }

		public void SetNeighbours(List<PathNode> neighbours)
		{
			Neighbours = neighbours;
        }
	}
}

