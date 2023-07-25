# TODO

- pong game:
    - scaleable windowed mode: https://community.monogame.net/t/handling-user-controlled-window-resizing/7828
      - keeping aspect ratio
      - free aspect ratio mode?
    - powerups:
      - requires Timer-class to handle boost-lifespan? store effects globally or by per paddle/ball?
      - speedboost to paddle
      - multiple balls
      - smaller balls
      - speedball that vanishes after hitting either side of wall.
    - make ball go in the direction the paddle hit it, as in changing it's direction slightly.
- gamepad input / controller support. https://docs.monogame.net/api/Microsoft.Xna.Framework.Input.html
- text input it bit slow, can't press next key before letting go of last to register next one.
- ball is not going all the way down and all the way up on screen before it turns

- UI elements
    - menu buttons
        - allow using keyboard & gamepad to select (WASD or arrow keys to navigate, ESC to go back, Enter to select)
        - allow using mouse to select (Pressing buttons does the action)
   
- custom font