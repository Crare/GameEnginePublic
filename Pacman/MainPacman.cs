using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.FileManagement;
using GameEngine.Core.GameEngine.Sprites;
using GameEngine.Core.GameEngine.TileMap;
using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.GameEngine.Window;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.GameObjects;
using Pacman.GameObjects.tiles;
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
        private PacmanTileMap _tileMap;

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
            _tileMap = new PacmanTileMap();

            base.Initialize();
            Debug.WriteLine("Initialization done!");
        }

        protected override void LoadContent()
        {
            Debug.WriteLine("Loading content...");
            // load fonts
            var font = Content.Load<SpriteFont>("Fonts/Arial");
            var defaultTextColor = Color.White;
            _textDrawer = new TextDrawer(_spriteBatch, font, defaultTextColor);

            // load textures
            var pacmanTexture = Content.Load<Texture2D>("sprites/pacman");
            var ghostTexture = Content.Load<Texture2D>("sprites/pacman_ghost");
            var tilesTexture = Content.Load<Texture2D>("sprites/pacman_tiles");

            // load levels
            var levels = new string[1];
            levels[0] = FileSystem.LoadFromFileOrThrowException("levels/level0.csv");

            // init tilemap
            _tileMap.Initialize(tilesTexture, levels);
            _tileMap.LoadLevel(0);

            // assign entities
            var pacman = new PacmanEntity(
                new Vector2(_window.RenderTarget.Width / 2, _window.RenderTarget.Height / 2),
                pacmanTexture);
            _entityManager.AddEntity(pacman);

            var redGhost = new RedGhost(
                new Vector2(_window.RenderTarget.Width / 2 + 16, _window.RenderTarget.Height / 2),
                ghostTexture);
            _entityManager.AddEntity(redGhost);
            var blueGhost = new BlueGhost(
                new Vector2(_window.RenderTarget.Width / 2 + 32, _window.RenderTarget.Height / 2 ),
                ghostTexture);
            _entityManager.AddEntity(blueGhost);
            var OrangeGhost = new OrangeGhost(
                new Vector2(_window.RenderTarget.Width / 2 - 16, _window.RenderTarget.Height / 2),
                ghostTexture);
            _entityManager.AddEntity(OrangeGhost);
            var pinkGhost = new PinkGhost(
                new Vector2(_window.RenderTarget.Width / 2 - 32, _window.RenderTarget.Height / 2 ),
                ghostTexture);
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

            if (_keyboardState.IsKeyDown(Keys.F11) && !_lastKeyboardState.IsKeyDown(Keys.F11))
            {
                _window.ToggleFullScreen();
            }

#if DEBUG
            // Toggle Debug mode: ctrl + shift + D
            if (_keyboardState.IsKeyDown(Keys.D)
                && (_keyboardState.IsKeyDown(Keys.LeftShift) || _keyboardState.IsKeyDown(Keys.RightShift))
                && (_keyboardState.IsKeyDown(Keys.LeftControl) || _keyboardState.IsKeyDown(Keys.RightControl))
                &&
                    !(_lastKeyboardState.IsKeyDown(Keys.D)
                    && (_lastKeyboardState.IsKeyDown(Keys.LeftShift) || _lastKeyboardState.IsKeyDown(Keys.RightShift))
                    && (_lastKeyboardState.IsKeyDown(Keys.LeftControl) || _lastKeyboardState.IsKeyDown(Keys.RightControl))
                    )
                )
            {
                ToggleDebugMode();
            }
#endif

            _entityManager.UpdateEntities(gameTime, _keyboardState);
            _tileMap.UpdateTiles(gameTime);

            _lastKeyboardState = _keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _window.StartDrawToRenderTarget(_spriteBatch);
            // draw code below

            _tileMap.DrawTiles(_spriteBatch);
            _entityManager.DrawEntities(gameTime);

            if (Globals.DEBUG_DRAW)
            {
                _tileMap.DebugDrawTiles(_spriteBatch, _textDrawer);
            }

            // end of draw code
            _window.EndDrawToRenderTarget(_spriteBatch);
            _window.DrawToDestination(_spriteBatch);
            base.Draw(gameTime);
        }

        private void ToggleDebugMode()
        {
            Globals.DEBUG_DRAW = !Globals.DEBUG_DRAW;
        }
    }
}