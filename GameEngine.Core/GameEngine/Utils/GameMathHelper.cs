using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.CameraView;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.Utils
{
    public static class GameMathHelper
    {
        /// <summary>
        /// Gets mid point between two vectors
        /// </summary>
        public static Vector2 GetMidPoint(Vector2 p1, Vector2 p2)
        {
            var p3 = new Vector2 (
                (p2.X - p1.X) / 2, 
                (p2.Y - p1.Y) / 2
                );
            return p3;
        }

        /// <summary>
        /// Gets Vector2 position from p1 towards p2 with distance d.
        /// </summary>
        public static Vector2 GetPositionTowardsPoint(Vector2 p1, Vector2 p2, float d)
        {
            var D = DistanceBetweenPoints(p1, p2);
            // u = distance units vector
            var u = new Vector2((p2.X - p1.X)/D, (p2.Y - p1.Y) / D);
            // p3 = position from p1 towards point p2 with distance d.
            var p3 = p1 + d * u;
            return p3;
        }

        public static float DistanceBetweenPoints(Vector2 p1, Vector2 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        /// <summary>
        /// Gets Vector2 position from p1 towards p2 with distance of maxDistance.
        /// if distance between points is less than maxDistance p2 is returned.
        /// </summary>
        public static Vector2 GetPositionTowardsPointWithMaxDistance(Vector2 p1, Vector2 p2, float maxD)
        {
            var D = DistanceBetweenPoints(p1, p2);
            // u = distance units vector
            var u = new Vector2((p2.X - p1.X) / D, (p2.Y - p1.Y) / D);

            //if (D < maxD)
            //{
            //    return p2;
            //}

            var distance = D < maxD ? D : maxD;
            // p3 = position from p1 towards point p2 with distance.
            var p3 = p1 + distance * u;
            return p3;
        }
    }
}
