using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.GameEngine.CameraView
{
    public class Camera
    {
        protected float _zoom;
        public Matrix _transform;
        public Vector2 _pos;
        protected float _rotation;

        public Camera()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
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
        public Vector3 GetCameraOffset()
        {
            return _transform.Translation;
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
