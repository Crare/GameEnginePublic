using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameEngine.Core.GameEngine.TileMap;
using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;

namespace GameEngine.Core.GameEngine.Pathfinding
{
    public enum PathFindingType
    {
        AStarPathfinding
    }

    public enum CheckNeighbours
    {
        FourNeighbours,
        EightNeighbours
    }

    public class PathfindingNode
    {
        public int X, Y;
        /// <summary>
        /// gScore[n] is the cost of the cheapest path from start to n currently known.
        /// </summary>
        public float gScore;
        /// <summary>
        /// fScore[n] represents our current best guess as to
        /// how cheap a path could be from start to finish if it goes through n.
        /// </summary>
        public float fScore;

        public PathfindingNode Parent;

        public PathfindingNode(PathNode node, float weight = 1)
        {
            X = node.X;
            Y = node.Y;
            Parent = null;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = weight;
        }

        public PathfindingNode(int x, int y, float weight, PathfindingNode parent)
        {
            X = x;
            Y = y;
            Parent = parent;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = weight;
        }

        public float DistanceToTarget;
        public float Cost;
        public float Weight;
        public float F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }

    }

    public class Pathfinding<TTile> where TTile : Tile
	{
        public PathNode[,] PathNodes;
		internal TileMap<TTile> TileMap;
        internal int[] OpenTileTypes;

        internal List<PathNode> debugPath;
        internal Point debugPathStart = new Point(1, 1);
        internal Point debugPathEnd = new Point(17, 19);

		public Pathfinding(TileMap<TTile> tileMap, int[] openTileTypes)
		{
			TileMap = tileMap;
			OpenTileTypes = openTileTypes;
		}

        public void SetDebugPathStart(Point point)
        {
            debugPathStart = point;
        }

        public void SetDebugPathEnd(Point point)
        {
            debugPathEnd = point;
        }

        public void DebugPath()
        {
            debugPath = GetPath(debugPathStart, debugPathEnd);
            if (debugPath == null)
            {
                Debug.WriteLine("no debug path found");
            } else
            {
                Debug.WriteLine($"got debug path with length of {debugPath.Count}");
            }
        }

        public void DrawDebugPath()
        {
            DrawPath(debugPath);
        }

        public void DrawPath(List<PathNode> path)
        {
            if (path == null || !path.Any())
            {
                return;
            }
            var pathColor = new Color(0f, 0.5f, 0f, 0.5f);
            var tz = TileMap.TileSize;

            for (var i = 0; i < path.Count; i++)
            {
                if (i == path.Count-1)
                {
                    return;
                }
                var node = path[i];
                var nextNode = path[i + 1];
                GameDebug.DrawLine(
                    new Vector2(node.X * tz, node.Y * tz),
                    new Vector2(nextNode.X * tz, nextNode.Y * tz),
                    pathColor,
                    1);
            }
        }

        public void DrawDebugNodes(bool drawCoordinates = false)
        {
            var rectPadding = 5;
            var blue = new Color(0f,0f,0.8f,0.9f);
            var green = new Color(0f, 0.8f, 0f, 0.9f);
            for (var y = 0; y < TileMap.Height; y++)
            {
                for (var x = 0; x < TileMap.Width; x++)
                {
                    var node = PathNodes[x, y];
                    if (node != null)
                    {
                        var rect = new Rectangle(
                            (int)node.Position.X - rectPadding / 2,
                            (int)node.Position.Y - rectPadding / 2,
                            rectPadding,
                            rectPadding);
                        GameDebug.DrawRectangle(rect, blue, false);
                        if (drawCoordinates)
                        {
                            GameDebug.DrawText($"{node.X}, {node.Y}", green, node.Position, 0.5f, HorizontalAlignment.Center, VerticalAlignment.Middle);
                        }
                    }
                }
            }
        }

        public void DrawDebugConnections()
        {
            var green = new Color(0f, 0.8f, 0f, 0.9f);
            for (var y = 0; y < TileMap.Height; y++)
            {
                for (var x = 0; x < TileMap.Width; x++)
                {
                    var node = PathNodes[x, y];
                    if (node != null)
                    {
                        if (node.Neighbours.Any())
                        {
                            node.Neighbours.ForEach(n => {
                                if (n.X > node.X || n.Y > node.Y)
                                {
                                    // draw only lines to neighbours left and down.
                                    // so  we don't draw lines multiple times.
                                    GameDebug.DrawLine(new Vector2(node.Position.X, node.Position.Y), new Vector2(n.Position.X, n.Position.Y), green, 1f);
                                }
                            });
                        }
                    }
                }
            }
        }

        public void Init()
        {
			PathNodes = new PathNode[TileMap.Width, TileMap.Height]; 

			// create path nodes
            for (var y = 0; y < TileMap.Height; y++)
			{
                for (var x = 0; x < TileMap.Width; x++)
                {
					if (OpenTileTypes.Any(t => t == TileMap.Tiles[x,y]?.TileType))
					{
						PathNodes[x, y] = new PathNode(
                            new Vector2(x *  TileMap.TileSize, y  * TileMap.TileSize),
                            x,
                            y);
					}
                }
            }

            // add neighbours for path nodes.
            for (var y = 0; y < TileMap.Height; y++)
            {
                for (var x = 0; x < TileMap.Width; x++)
                {
                    if (OpenTileTypes.Any(t => t == TileMap.Tiles[x, y]?.TileType))
                    {
						PathNodes[x, y].SetNeighbours(GetNeighbouringPathNodes(x, y));
                    }
                }
            }
        }

        public List<PathNode> GetPath(Point a, Point b, PathFindingType pathFindingType = PathFindingType.AStarPathfinding, CheckNeighbours checkNeighbours = CheckNeighbours.FourNeighbours)
        {
            if (pathFindingType == PathFindingType.AStarPathfinding)
            {
                return AStarPathfinding2(a, b, checkNeighbours);
            }
            return null;
        }

        public Point GetFurthestNodePositionFromPoint(Point point)
        {
            Point furthestPoint = point;
            var distance = 0f;
            for (int y = 0;  y < TileMap.Height; y++)
            {
                for (int x = 0; x < TileMap.Width; x++)
                {
                    if (PathNodes[x,y] != null)
                    {
                        var node = PathNodes[x, y];
                        if (Distance(point.X, point.Y, node.X, node.Y) > distance)
                        {
                            furthestPoint = new Point(node.X, node.Y);
                            distance = Distance(point.X, point.Y, node.X, node.Y);
                        }
                    }
                }
            }
            return furthestPoint;
        }

        private float Distance(int aX, int aY,  int bX, int bY)
        {
            return Math.Abs(aX - bX) + Math.Abs(aY - bY);
        }

        private List<PathNode> AStarPathfinding2(Point a, Point b, CheckNeighbours checkNeighbours = CheckNeighbours.FourNeighbours)
        {
            PathfindingNode start = new(PathNodes[a.X, a.Y]);
            PathfindingNode goal = PathNodes[b.X, b.Y] != null ? new PathfindingNode(PathNodes[b.X, b.Y]) : null;
            if (goal ==  null)
            {
                return null;
            }

            Stack<PathNode> Path = new();
            PriorityQueue<PathfindingNode, float> OpenList = new();
            List<PathfindingNode> ClosedList = new();
            List<PathfindingNode> neighbours;
            PathfindingNode current = start;

            // add start node to Open List
            OpenList.Enqueue(start, start.F);

            while (OpenList.Count != 0 && !ClosedList.Exists(node => node.X == goal.X && node.Y == goal.Y))
            {
                current = OpenList.Dequeue();
                ClosedList.Add(current);
                neighbours = GetNeighbours(current.X, current.Y, checkNeighbours);

                foreach (PathfindingNode neighbour in neighbours)
                {
                    if (!ClosedList.Contains(neighbour))
                    {
                        bool isFound = false;
                        foreach (var oLNode in OpenList.UnorderedItems)
                        {
                            if (oLNode.Element == neighbour)
                            {
                                isFound = true;
                            }
                        }
                        if (!isFound)
                        {
                            neighbour.Parent = current;
                            neighbour.DistanceToTarget = Distance(neighbour.X, neighbour.Y, goal.X, goal.Y);
                            neighbour.Cost = neighbour.Weight + neighbour.Parent.Cost;
                            OpenList.Enqueue(neighbour, neighbour.F);
                        }
                    }
                }
            }

            // construct path, if end was not closed return null
            if (!ClosedList.Exists(n => n.X == goal.X  && n.Y == goal.Y))
            {
                return null;
            }

            // if all good, return path
            PathfindingNode temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                Path.Push(PathNodes[temp.X,temp.Y]);
                temp = temp.Parent;
            } while (temp != start && temp != null);

            return Path.ToList();
        }

        private static float HeuristicCost(PathfindingNode node, PathfindingNode targetNode)
        {
            return DistanceWeight(node, targetNode);
        }

        // d(current,neighbor) is the weight of the edge from current to neighbor
        private static float DistanceWeight(PathfindingNode current, PathfindingNode neighbour)
        {
            return Math.Abs(current.X - neighbour.X) + Math.Abs(current.Y - neighbour.Y);
        }

        private List<PathfindingNode> GetNeighbours(int x, int y, CheckNeighbours checkNeighbours)
        {
            List<PathfindingNode> neighbours = new();

            // left
            if (x - 1 > 0 && PathNodes[x - 1, y] != null)
            {
                neighbours.Add(new PathfindingNode(PathNodes[x - 1, y]));
            }
            // right
            if (x + 1 < TileMap.Width && PathNodes[x + 1, y] != null)
            {
                neighbours.Add(new PathfindingNode(PathNodes[x + 1, y]));
            }
            // up
            if (y - 1 > 0 && PathNodes[x, y - 1] != null)
            {
                neighbours.Add(new PathfindingNode(PathNodes[x, y - 1]));
            }
            // down
            if (y + 1 < TileMap.Height && PathNodes[x, y + 1] != null)
            {
                neighbours.Add(new PathfindingNode(PathNodes[x, y + 1]));
            }

            if (checkNeighbours == CheckNeighbours.EightNeighbours)
            {
                // TODO check eight neighbours
            }

            return neighbours;
        }


        private List<PathNode> GetNeighbouringPathNodes(int x,int y)
		{
            List<PathNode> neighbours = new();

			// left
			if (x-1 > 0 && PathNodes[x-1, y] != null)
			{
				neighbours.Add(PathNodes[x - 1, y]);
			}
			// right
            if (x + 1 < TileMap.Width && PathNodes[x + 1, y] != null)
            {
                neighbours.Add(PathNodes[x + 1, y]);
            }
            // up
            if (y - 1 > 0 && PathNodes[x, y - 1] != null)
            {
                neighbours.Add(PathNodes[x, y - 1]);
            }
            // down
            if (y + 1 < TileMap.Height && PathNodes[x, y + 1] != null)
            {
                neighbours.Add(PathNodes[x, y + 1]);
            }

            return neighbours;
		}

    }
}

