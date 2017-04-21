using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SquareTime {
    class Camera {
        protected float zoom;
        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = (value < 0.005f) ? 0.005f : value;
            }
        }
        public Matrix transform;
        private Matrix lastTransform;
        private Matrix lastInverseTransform;
        public Matrix inverseTransform;
        public Vector2 Position { get; set; }
        public Vector2 screenToWorld(Vector2 screen) {
            if(transform == null) {
                return Vector2.Zero;
            }
            return Vector2.Transform(screen,inverseTransform);
        }
        public Vector2 lastscreenToWorld(Vector2 screen) {
            if (transform == null) {
                return Vector2.Zero;
            }
            return Vector2.Transform(screen, lastInverseTransform);
        }
        public Vector2 worldToScreen(Vector2 world) {
            if(transform == null) {
                return Vector2.Zero;
            }
            return Vector2.Transform(world, transform);
        }
        public Camera() {
            Zoom = 1.0f;
            Position = Vector2.Zero;
        }
        public Matrix getTransform(GraphicsDevice d) {
            lastTransform = transform;
            lastInverseTransform = inverseTransform;
            transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0) )*
                                                Matrix.CreateRotationZ(0.0f) *
                                                Matrix.CreateScale(zoom)*
                                                Matrix.CreateTranslation(new Vector3(d.Viewport.Width * 0.5f,d.Viewport.Height * 0.5f,0));
            inverseTransform = Matrix.Invert(transform);
            return transform;
        }
    }
}
