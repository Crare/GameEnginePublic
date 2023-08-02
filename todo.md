# TODO

make game collection screen to choose game from?

## DOING NOW / NEXT:
just a space to concentrate on what needs to be done next:

- center pacman game


## general

- refactor:
  - Camera-class: offset rendertarget inside the window.
  - generate sourceRectangles based on dimension and amount of frames.

- scaleable windowed mode: https://community.monogame.net/t/handling-user-controlled-window-resizing/7828
  - keeping aspect ratio
  - free aspect ratio mode?
  - refactor Window stuff to Window-class

- gamepad input / controller support. https://docs.monogame.net/api/Microsoft.Xna.Framework.Input.html
  - maybe make generalized input thing,
    - setting view with keymappings that can be changed

- text input it bit slow, can't press next key before letting go of last to register next one.

- UI elements
  - menu buttons
    - allow using keyboard & gamepad to select (WASD or arrow keys to navigate, ESC to go back, Enter to select)
    - allow using mouse to select (Pressing buttons does the action)
  - background layout box
    - use sprite with 9-slicing like in unity https://docs.unity3d.com/Manual/9SliceSprites.html

- custom font

## game related
- pacman:
  - ghosts logic
  - pacman logic
  - sounds
  - scoreboard
  - particle effects

- pong game:
  - trailing particles behind ball
  - powerups:
    - requires Timer-class to handle boost-lifespan? store effects globally or by per paddle/ball?
    - speedboost to paddle
    - multiple balls
    - smaller balls
    - speedball that vanishes after hitting either side of wall.
  - make ball go in the direction the paddle hit it, as in changing it's direction slightly.
  - ball is not going all the way down and all the way up on screen before it turns
