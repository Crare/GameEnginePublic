using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.Input
{
    public static class InputManager
    {
        public static MouseState _mouseState;
        public static MouseState _lastMouseState;

        public static KeyboardState _keyboardState;
        public static KeyboardState _lastKeyboardState;

        public static GamePadState _gamePadState;
        public static GamePadState _lastGamePadState;


        public static void Update()
        {
            _lastMouseState = _mouseState;
            _mouseState = Mouse.GetState();

            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            _lastGamePadState = _gamePadState;
            _gamePadState = GamePad.GetState(0);
        }

        // can be extended in inherited class with game specific keybindings.
    }
}
