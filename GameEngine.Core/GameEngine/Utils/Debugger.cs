using System;
using GameEngine.Core.GameEngine.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.GameEngine.Utils
{
	public sealed class GameDebug
	{
        private static GameDebug instance = null; // singleton
        private static readonly object padlock = new();
        public static GameDebug Instance
        {
            get
            {
                // locks the Singleton instance to be once created by locking one of its objects.
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameDebug();
                    }
                    return instance;
                }
            }
        }

        private static SpriteBatch  SpriteBatch;
        private static TextDrawer TextDrawer;
        private static Texture2D DebugTexture;

        public static void Init(SpriteBatch spriteBatch,  TextDrawer textDrawer, Texture2D debugTexture)
        {
            SpriteBatch = spriteBatch;
            TextDrawer = textDrawer;
            DebugTexture = debugTexture;

        }

        public static void DrawRectangle(Rectangle  rect, Color color, bool filled, float thickness = 1f)
        {
            if (filled)
            {
                SpriteBatch.Draw(DebugTexture, rect, color);
            } else
            {
                var tl = new Vector2(rect.X, rect.Y);
                var tr = new Vector2(rect.X + rect.Width, rect.Y);
                var br = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);
                var bl = new Vector2(rect.X, rect.Y + rect.Height);
                DrawLine(tl, tr, color, thickness);
                DrawLine(tr, br, color, thickness);
                DrawLine(br, bl, color, thickness);
                DrawLine(bl, tl, color, thickness);
            }
        }

        public static void DrawText(string text, Color color, Vector2 position, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Right, VerticalAlignment verticalAlignment = VerticalAlignment.Bottom)
        {
            TextDrawer.Draw(text, position, color, horizontalAlignment, verticalAlignment);
        }

        public static void DrawText(string text, Color color, Vector2 position, float scale, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Right, VerticalAlignment verticalAlignment = VerticalAlignment.Bottom)
        {
            TextDrawer.Draw(text, position, color, scale, horizontalAlignment, verticalAlignment);
        }

        public static void DrawLine(Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(point1, distance, angle, color, thickness);
        }

        public static void DrawLine(Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            SpriteBatch.Draw(DebugTexture, point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }

    }
}

