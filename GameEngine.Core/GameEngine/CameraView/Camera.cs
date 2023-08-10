using GameEngine.Core.GameEngine.TileMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace GameEngine.Core.GameEngine.CameraView
{
    public class Camera
    {
        protected float _zoom;
        public Matrix _transform;
        public Vector2 _pos;
        protected float _rotation;
        private float _scale;
        private float MoveAmount;
        protected GameEngine.Window.GameWindow Window;
        private bool MoveHoldingDownKey;

        public Camera(Window.GameWindow window, float moveAmount = 1.0f, bool moveHoldingDownKey = true)
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
            _scale = 4.0f;
            Window = window;
            MoveAmount = moveAmount;
            MoveHoldingDownKey = moveHoldingDownKey;
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Vector2 ScreenToUIPosition(Point pos)
        {
            return new Vector2(pos.X, pos.Y) * Window.Scaling;
        }

        public Vector2 ScreenToWorldPosition(Point pos)
        {
            return new Vector2(pos.X, pos.Y) * Window.Scaling - GetCameraOffset();
        }

        public bool IsMouseInsideWindow(Point mousePos)
        {
            return mousePos.X >= 0 
                && mousePos.X < Window.RenderTargetDestination.Width 
                && mousePos.Y >= 0 
                && mousePos.Y < Window.RenderTargetDestination.Height;
        }

        /// <summary>
        /// updates camera position if numpad keys are pressed.
        /// </summary>
        public void Update(KeyboardState keyboardState, KeyboardState lastKeyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.NumPad7))
            {
                Zoom += 0.01f;
            }
            if (keyboardState.IsKeyDown(Keys.NumPad9))
            {
                Zoom -= 0.01f;
            }

            if ((MoveHoldingDownKey && keyboardState.IsKeyDown(Keys.NumPad8)) ||
                (!MoveHoldingDownKey && keyboardState.IsKeyDown(Keys.NumPad8) && !lastKeyboardState.IsKeyDown(Keys.NumPad8)))
            {
                Move(new Vector2(0, -MoveAmount));
            }
            if ((MoveHoldingDownKey && keyboardState.IsKeyDown(Keys.NumPad2)) ||
                (!MoveHoldingDownKey && keyboardState.IsKeyDown(Keys.NumPad2) && !lastKeyboardState.IsKeyDown(Keys.NumPad2)))
            {
                Move(new Vector2(0, MoveAmount));
            }
            if ((MoveHoldingDownKey && keyboardState.IsKeyDown(Keys.NumPad6)) ||
                (!MoveHoldingDownKey && keyboardState.IsKeyDown(Keys.NumPad6) && !lastKeyboardState.IsKeyDown(Keys.NumPad6)))
            {
                Move(new Vector2(MoveAmount, 0));
            }
            if ((MoveHoldingDownKey && keyboardState.IsKeyDown(Keys.NumPad4)) ||
                (!MoveHoldingDownKey && keyboardState.IsKeyDown(Keys.NumPad4) && !lastKeyboardState.IsKeyDown(Keys.NumPad4)))
            {
                Move(new Vector2(-MoveAmount, 0));
            }
            if (keyboardState.IsKeyDown(Keys.NumPad5) && !lastKeyboardState.IsKeyDown(Keys.NumPad5))
            {
                Debug.WriteLine(
                    $"camera pos: '{Pos}'\n" +
                    $"camera Zoom: '{Zoom}'\n" +
                    $"camera transform.Translation: '{_transform.Translation}'\n" +
                $"camera rotation: '{Rotation}'\n"
                    );
            }
            if (keyboardState.IsKeyDown(Keys.NumPad1) && !lastKeyboardState.IsKeyDown(Keys.NumPad1))
            {
                // reset zoom
                Zoom = 1f;
            }
            if (keyboardState.IsKeyDown(Keys.NumPad3) && !lastKeyboardState.IsKeyDown(Keys.NumPad3))
            {
                Pos = new Vector2(0,0);
            }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }

        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        /// <summary>
        ///  returns camera offset that needs to be applied when rendering or checking collisions
        ///  on objects that are offsetted by camera.
        ///  Decrement offset X and Y from object position.
        /// </summary>
        public Vector2 GetCameraOffsetWithScreenScaling()
        {
            return new Vector2(_transform.Translation.X * _scale, _transform.Translation.Y * _scale);
        }

        public Vector2 ScreenToWorldSpace(in Vector2 point)
        {
            Matrix invertedMatrix = Matrix.Invert(_transform);
            return Vector2.Transform(point, invertedMatrix);
        }

        public Vector2 GetCameraOffset()
        {
            return new Vector2(_transform.Translation.X, _transform.Translation.Y);
        }

        public Matrix get_transformation(int viewportWidth, int viewportHeight)
        {
            _transform =
              Matrix.CreateTranslation(
                new Vector3(-_pos.X, -_pos.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(viewportWidth * 0.5f, viewportHeight * 0.5f, 0));
            return _transform;
        }
    }
}
