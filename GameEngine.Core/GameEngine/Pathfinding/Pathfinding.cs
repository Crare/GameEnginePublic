using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameEngine.Core.GameEngine.TileMap;
using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using static System.Formats.Asn1.AsnWriter;

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

    public class Pathfinding<TTile> where TTile : Tile
    {
        public PathNode[,] PathNodes;
        internal TileMap<TTile> TileMap;
        internal int[] OpenTileTypes;

        internal List<PathNode> debugPath;
        internal Point debugPathStart = new(1, 1);
        internal Point debugPathEnd = new(17, 19);

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
            }
            else
            {
                Debug.WriteLine($"got debug path with length of {debugPath.Count}");
            }
        }

        public void DrawDebugPath()
        {
            DrawPath(debugPath);
        }

        public void DrawPath(List<PathNode> path, Color color = default, int offset = 0)
        {
            if (path == null || !path.Any())
            {
                return;
            }
            var pathColor = color == default ? new Color(0f, 0.5f, 0f, 0.5f) : color;
            var tz = TileMap.TileSize;

            for (var i = 0; i < path.Count; i++)
            {
                if (i == path.Count - 1)
                {
                    return;
                }
                var node = path[i];
                var nextNode = path[i + 1];
                GameDebug.DrawLine(
                    new Vector2(node.X * tz + offset, node.Y * tz + offset),
                    new Vector2(nextNode.X * tz + offset, nextNode.Y * tz + offset),
                    pathColor,
                    1);
            }
        }

        public void DrawDebugNodes(bool drawCoordinates = false)
        {
            var rectPadding = 5;
            var blue = new Color(0f, 0f, 0.8f, 0.9f);
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
                            node.Neighbours.ForEach(n =>
                            {
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
                    if (OpenTileTypes.Any(t => t == TileMap.Tiles[x, y]?.TileType))
                    {
                        PathNodes[x, y] = new PathNode(
                            new Vector2(x * TileMap.TileSize, y * TileMap.TileSize),
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

        public List<PathNode> GetPath(Point a, Vector2 b, PathFindingType pathFindingType = PathFindingType.AStarPathfinding, CheckNeighbours checkNeighbours = CheckNeighbours.FourNeighbours)
        {
            if (pathFindingType == PathFindingType.AStarPathfinding)
            {
                var bb = TileMap.WorldPositionToTilePosition(b);
                return AStarPathfindingGetPath(a, bb, checkNeighbours);
            }
            return null;
        }

        public List<PathNode> GetPath(Vector2 a, Point b, PathFindingType pathFindingType = PathFindingType.AStarPathfinding, CheckNeighbours checkNeighbours = CheckNeighbours.FourNeighbours)
        {
            if (pathFindingType == PathFindingType.AStarPathfinding)
            {
                var aa = TileMap.WorldPositionToTilePosition(a);
                return AStarPathfindingGetPath(aa, b, checkNeighbours);
            }
            return null;
        }

        public List<PathNode> GetPath(Vector2 a, Vector2 b, PathFindingType pathFindingType = PathFindingType.AStarPathfinding, CheckNeighbours checkNeighbours = CheckNeighbours.FourNeighbours)
        {
            if (pathFindingType == PathFindingType.AStarPathfinding)
            {
                var aa = TileMap.WorldPositionToTilePosition(a);
                var bb = TileMap.WorldPositionToTilePosition(b);
                return AStarPathfindingGetPath(aa, bb, checkNeighbours);
            }
            return null;
        }

        public List<PathNode> GetPath(Point a, Point b, PathFindingType pathFindingType = PathFindingType.AStarPathfinding, CheckNeighbours checkNeighbours = CheckNeighbours.FourNeighbours)
        {
            if (pathFindingType == PathFindingType.AStarPathfinding)
            {
                return AStarPathfindingGetPath(a, b, checkNeighbours);
            }
            return null;
        }

        public Point GetFurthestNodePositionFromPosition(Vector2 worldPosition)
        {
            var point = TileMap.WorldPositionToTilePosition(worldPosition);
            return GetFurthestNodePositionFromPoint(point);
        }

        public Point GetFurthestNodePositionFromPoint(Point point)
        {
            Point furthestPoint = point;
            var distance = 0f;
            for (int y = 0; y < TileMap.Height; y++)
            {
                for (int x = 0; x < TileMap.Width; x++)
                {
                    if (PathNodes[x, y] != null)
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

        private List<PathNode> AStarPathfindingGetPath(Point a, Point b, CheckNeighbours checkNeighbours = CheckNeighbours.FourNeighbours)
        {
            return AStarPathfinding.GetPath(PathNodes, new GridPoint(a.X, a.Y), new GridPoint(b.X, b.Y), checkNeighbours);
        }

        private float Distance(int aX, int aY, int bX, int bY)
        {
            return Math.Abs(aX - bX) + Math.Abs(aY - bY);
        }
        
        private List<PathNode> GetNeighbouringPathNodes(int x, int y)
        {
            List<PathNode> neighbours = new();

            // left
            if (x - 1 > 0 && PathNodes[x - 1, y] != null)
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

