using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Sprites;
using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.GameEngine.Window;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.GameObjects;
using System.Diagnostics;

namespace Pacman
{
    public class MainPacman : Game
    {
        // monogame stuff
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // common core stuff
        public Window _window;
        Point _gameResolution = new Point(800, 480);
        private EntityManager _entityManager;
        private TextDrawer _textDrawer;
        private KeyboardState _keyboardState;
        private KeyboardState _lastKeyboardState;

        // game specific stuff

        public MainPacman()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Debug.WriteLine("Initializing...");
            _window = new Window(_gameResolution,
                    new RenderTarget2D(GraphicsDevice, _gameResolution.X, _gameResolution.Y),
                    _graphics
                );
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _entityManager = new EntityManager(_spriteBatch, _window.RenderTarget);

            base.Initialize();
            Debug.WriteLine("Initialization done!");
        }

        protected override void LoadContent()
        {
            Debug.WriteLine("Loading content...");
            // load fonts
            // TODO: FONT

            // load textures
            var pacmanTexture = Content.Load<Texture2D>("sprites/pacman");
            var ghostTexture = Content.Load<Texture2D>("sprites/pacman_ghost");

            // assign entities
            var pacman = new PacmanEntity(
                new Rectangle(
                    _window.RenderTarget.Width / 2, 
                    _window.RenderTarget.Height / 2, 
                    16,
                    16),
                pacmanTexture);
            _entityManager.AddEntity(pacman);

            var redGhost = new Ghost(
                new Rectangle(
                    _window.RenderTarget.Width / 2,
                    _window.RenderTarget.Height / 2 + 16,
                    16,
                    16),
                ghostTexture,
                Color.Red);
            _entityManager.AddEntity(redGhost);
            var blueGhost = new Ghost(
                new Rectangle(
                    _window.RenderTarget.Width / 2 + 16,
                    _window.RenderTarget.Height / 2 + 16,
                    16,
                    16),
                ghostTexture,
                Color.Blue);
            _entityManager.AddEntity(blueGhost);
            var OrangeGhost = new Ghost(
                new Rectangle(
                    _window.RenderTarget.Width / 2 + 32,
                    _window.RenderTarget.Height / 2 + 16,
                    16,
                    16),
                ghostTexture,
                Color.Orange);
            _entityManager.AddEntity(OrangeGhost);
            var pinkGhost = new Ghost(
                new Rectangle(
                    _window.RenderTarget.Width / 2 - 16,
                    _window.RenderTarget.Height / 2 + 16,
                    16,
                    16),
                ghostTexture,
                Color.Pink);
            _entityManager.AddEntity(pinkGhost);

            // add audio
            // TODO: AUDIO
            Debug.WriteLine("Loading content done!");
        }

        protected override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _entityManager.UpdateEntities(gameTime, _keyboardState);

            _lastKeyboardState = _keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _window.StartDrawToRenderTarget(_spriteBatch);
            // draw code below

            _entityManager.DrawEntities(gameTime);

            // end of draw code
            _window.EndDrawToRenderTarget(_spriteBatch);
            _window.DrawToDestination(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}