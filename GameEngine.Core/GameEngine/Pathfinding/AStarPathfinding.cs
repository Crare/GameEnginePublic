using System;
using System.Collections.Generic;

namespace GameEngine.Core.GameEngine.Pathfinding
{
    public class GridPoint
    {
        public int X, Y;
        public GridPoint(int x, int y) { X = x; Y = y; }

        /// <summary>
        /// this is required for checking dictionary.ContainsKey()
        /// </summary>
        public override int GetHashCode()
        {
            return X + X * Y;
        }

        /// <summary>
        /// this is required for checking dictionary.ContainsKey()
        /// </summary>
        public override bool Equals(object obj)
        {
            GridPoint other = obj as GridPoint;
            return other != null && other.X == X && other.Y == Y;
        }
    }

    public static class AStarPathfinding
	{
        public static List<PathNode> GetPath(PathNode[,] pathNodes, GridPoint start, GridPoint goal, CheckNeighbours checkNeighbours = CheckNeighbours.FourNeighbours)
        {
            // contains already checked grid points
            Dictionary<GridPoint, bool> closedSet = new();
            // contains currently open grid points to go through
            Dictionary<GridPoint, bool> openSet = new();

            //cost of start to this key node
            Dictionary<GridPoint, int> gScore = new();
            //cost of start to goal, passing through key node
            Dictionary<GridPoint, int> fScore = new();

            // contains possible path routes
            Dictionary<GridPoint, GridPoint> nodeLinks = new();

            openSet[start] = true;
            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);

            while (openSet.Count > 0)
            {
                var current = nextBest(openSet, fScore);
                if (current.Equals(goal))
                {
                    return Reconstruct(pathNodes, current, nodeLinks);
                }

                openSet.Remove(current);
                closedSet[current] = true;

                foreach (var neighbor in Neighbors(pathNodes, current, checkNeighbours))
                {
                    if (closedSet.ContainsKey(neighbor))
                    {
                        continue;
                    }

                    var projectedG = getGScore(current, gScore) + 1;

                    if (!openSet.ContainsKey(neighbor))
                    {
                        openSet[neighbor] = true;
                    }
                    else if (projectedG >= getGScore(neighbor, gScore))
                    {
                        continue;
                    }

                    //record it
                    nodeLinks[neighbor] = current;
                    gScore[neighbor] = projectedG;
                    fScore[neighbor] = projectedG + Heuristic(neighbor, goal);

                }
            }
            return new List<PathNode>();
        }

        /// <summary>
        /// basic heuristic method, checks the distance from current point to goal
        /// </summary>
        private static int Heuristic(GridPoint start, GridPoint goal)
        {
            var dx = goal.X - start.X;
            var dy = goal.Y - start.Y;
            return Math.Abs(dx) + Math.Abs(dy);
        }

        /// <summary>
        /// gScore = cost of start to this key node
        /// </summary>
        private static int getGScore(GridPoint pt, Dictionary<GridPoint, int> gScore)
        {
            int score = int.MaxValue;
            gScore.TryGetValue(pt, out score);
            return score;
        }

        /// <summary>
        /// gScore = cost of start to goal, passing through key node
        /// </summary>
        private static int getFScore(GridPoint pt, Dictionary<GridPoint, int> fScore)
        {
            int score = int.MaxValue;
            fScore.TryGetValue(pt, out score);
            return score;
        }

        private static IEnumerable<GridPoint> Neighbors(PathNode[,] pathNodes, GridPoint center, CheckNeighbours checkNeighbours)
        {

            GridPoint pt = new GridPoint(center.X - 1, center.Y - 1);
            if (checkNeighbours == CheckNeighbours.EightNeighbours && IsValidNeighbor(pathNodes, pt))
                yield return pt;

            pt = new GridPoint(center.X, center.Y - 1);
            if (IsValidNeighbor(pathNodes, pt))
                yield return pt;

            pt = new GridPoint(center.X + 1, center.Y - 1);
            if (checkNeighbours == CheckNeighbours.EightNeighbours && IsValidNeighbor(pathNodes, pt))
                yield return pt;

            //middle row
            pt = new GridPoint(center.X - 1, center.Y);
            if (IsValidNeighbor(pathNodes, pt))
                yield return pt;

            pt = new GridPoint(center.X + 1, center.Y);
            if (IsValidNeighbor(pathNodes, pt))
                yield return pt;


            //bottom row
            pt = new GridPoint(center.X - 1, center.Y + 1);
            if (checkNeighbours == CheckNeighbours.EightNeighbours && IsValidNeighbor(pathNodes, pt))
                yield return pt;

            pt = new GridPoint(center.X, center.Y + 1);
            if (IsValidNeighbor(pathNodes, pt))
                yield return pt;

            pt = new GridPoint(center.X + 1, center.Y + 1);
            if (checkNeighbours == CheckNeighbours.EightNeighbours && IsValidNeighbor(pathNodes, pt))
                yield return pt;
        }

        private static bool IsValidNeighbor(PathNode[,] pathNodes, GridPoint pt)
        {
            int x = pt.X;
            int y = pt.Y;
            if (x < 0 || x > pathNodes.GetUpperBound(0))
                return false;

            if (y < 0 || y > pathNodes.GetUpperBound(1))
                return false;

            return pathNodes[x, y] != null;

        }

        private static List<PathNode> Reconstruct(PathNode[,] pathNodes, GridPoint current, Dictionary<GridPoint, GridPoint> nodeLinks)
        {
            List<PathNode> path = new List<PathNode>();
            while (nodeLinks.ContainsKey(current))
            {
                path.Add(pathNodes[current.X, current.Y]);
                current = nodeLinks[current];
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        /// gets next best GridPoint from openSet based on fScore (cost of start to this key node)
        /// </summary>
        private static GridPoint nextBest(Dictionary<GridPoint, bool> openSet, Dictionary<GridPoint, int> fScore)
        {
            int best = int.MaxValue;
            GridPoint bestPt = null;
            foreach (var node in openSet.Keys)
            {
                var score = getFScore(node, fScore);
                if (score < best)
                {
                    bestPt = node;
                    best = score;
                }
            }

            return bestPt;
        }

    }
}

