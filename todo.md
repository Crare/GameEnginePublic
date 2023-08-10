# TODO

make game collection screen to choose game from?

## FIX:


## DOING NOW / NEXT:
just a space to concentrate on what needs to be done next:

- UIInput wont get deactive when clicking outside element.
- pong game needs  small  fixes

## general

- scaleable windowed mode: https://community.monogame.net/t/handling-user-controlled-window-resizing/7828
  - keeping aspect ratio
  - free aspect ratio mode?
  - refactor Window stuff to Window-class
  - options for window sizing to window settings at top of window.

- gamepad input / controller support. https://docs.monogame.net/api/Microsoft.Xna.Framework.Input.html
  - maybe make generalized input thing,
    - setting view with keymappings that can be changed

- text input it bit slow, can't press next key before letting go of last to register next one.
- text input holding down key  should add iit multiple times  after held down for second or so.

- UI elements
  - menu buttons
    - allow using keyboard & gamepad to select (WASD or arrow keys to navigate, ESC to go back, Enter to select)
  - background layout box
    - use sprite with 9-slicing like in unity https://docs.unity3d.com/Manual/9SliceSprites.html
  -  ui  buttons don't work  in  fullscreen on mac.
  - movable windows,  draggable, resizeable, closeable.

- custom font
