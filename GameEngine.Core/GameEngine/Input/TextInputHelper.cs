using Microsoft.Xna.Framework.Input;

namespace GameEngine.Core.GameEngine.InputManagement
{
    public static class TextInputHelper
    {
        public static bool TryGetPressedKey(KeyboardState keyboard, KeyboardState oldKeyboard, out string key)
        {
            Keys[] keys = keyboard.GetPressedKeys();
            bool shift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

            if (keys.Length > 0 && !oldKeyboard.IsKeyDown(keys[0]))
            {
                switch (keys[0])
                {
                    //Alphabet keys
                    case Keys.A: key = shift ? "A" : "a"; return true;
                    case Keys.B: key = shift ? "B" : "b"; return true;
                    case Keys.C: key = shift ? "C" : "c"; return true;
                    case Keys.D: key = shift ? "D" : "d"; return true;
                    case Keys.E: key = shift ? "E" : "e"; return true;
                    case Keys.F: key = shift ? "F" : "f"; return true;
                    case Keys.G: key = shift ? "G" : "g"; return true;
                    case Keys.H: key = shift ? "H" : "h"; return true;
                    case Keys.I: key = shift ? "I" : "i"; return true;
                    case Keys.J: key = shift ? "J" : "j"; return true;
                    case Keys.K: key = shift ? "K" : "k"; return true;
                    case Keys.L: key = shift ? "L" : "l"; return true;
                    case Keys.M: key = shift ? "M" : "m"; return true;
                    case Keys.N: key = shift ? "N" : "n"; return true;
                    case Keys.O: key = shift ? "O" : "o"; return true;
                    case Keys.P: key = shift ? "P" : "p"; return true;
                    case Keys.Q: key = shift ? "Q" : "q"; return true;
                    case Keys.R: key = shift ? "R" : "r"; return true;
                    case Keys.S: key = shift ? "S" : "s"; return true;
                    case Keys.T: key = shift ? "T" : "t"; return true;
                    case Keys.U: key = shift ? "U" : "u"; return true;
                    case Keys.V: key = shift ? "V" : "v"; return true;
                    case Keys.W: key = shift ? "W" : "w"; return true;
                    case Keys.X: key = shift ? "X" : "x"; return true;
                    case Keys.Y: key = shift ? "Y" : "y"; return true;
                    case Keys.Z: key = shift ? "Z" : "z"; return true;

                    //Decimal keys
                    //case Keys.D0: if (shift) { key = ")"; } else { key = "0"; } return true;
                    //case Keys.D1: if (shift) { key = "!"; } else { key = "1"; } return true;
                    //case Keys.D2: if (shift) { key = "@"; } else { key = "2"; } return true;
                    //case Keys.D3: if (shift) { key = "#"; } else { key = "3"; } return true;
                    //case Keys.D4: if (shift) { key = "$"; } else { key = "4"; } return true;
                    //case Keys.D5: if (shift) { key = "%"; } else { key = "5"; } return true;
                    //case Keys.D6: if (shift) { key = "^"; } else { key = "6"; } return true;
                    //case Keys.D7: if (shift) { key = "&"; } else { key = "7"; } return true;
                    //case Keys.D8: if (shift) { key = "*"; } else { key = "8"; } return true;
                    //case Keys.D9: if (shift) { key = "("; } else { key = "9"; } return true;
                    case Keys.D0: key = "0"; return true;
                    case Keys.D1: key = "1"; return true;
                    case Keys.D2: key = "2"; return true;
                    case Keys.D3: key = "3"; return true;
                    case Keys.D4: key = "4"; return true;
                    case Keys.D5: key = "5"; return true;
                    case Keys.D6: key = "6"; return true;
                    case Keys.D7: key = "7"; return true;
                    case Keys.D8: key = "8"; return true;
                    case Keys.D9: key = "9"; return true;

                    //Decimal numpad keys
                    case Keys.NumPad0: key = "0"; return true;
                    case Keys.NumPad1: key = "1"; return true;
                    case Keys.NumPad2: key = "2"; return true;
                    case Keys.NumPad3: key = "3"; return true;
                    case Keys.NumPad4: key = "4"; return true;
                    case Keys.NumPad5: key = "5"; return true;
                    case Keys.NumPad6: key = "6"; return true;
                    case Keys.NumPad7: key = "7"; return true;
                    case Keys.NumPad8: key = "8"; return true;
                    case Keys.NumPad9: key = "9"; return true;

                    //Special keys
                    //case Keys.OemTilde: if (shift) { key = "~"; } else { key = "`"; } return true;
                    //case Keys.OemSemicolon: if (shift) { key = ":"; } else { key = ";"; } return true;
                    //case Keys.OemQuotes: if (shift) { key = "'"; } else { key = "\""; } return true;
                    //case Keys.OemQuestion: if (shift) { key = "?"; } else { key = "/"; } return true;
                    //case Keys.OemPlus: if (shift) { key = "+"; } else { key = "="; } return true;
                    //case Keys.OemPipe: if (shift) { key = "|"; } else { key = "\\"; } return true;
                    //case Keys.OemPeriod: if (shift) { key = ">"; } else { key = "."; } return true;
                    //case Keys.OemOpenBrackets: if (shift) { key = "{"; } else { key = "["; } return true;
                    //case Keys.OemCloseBrackets: if (shift) { key = "}"; } else { key = "]"; } return true;
                    //case Keys.OemMinus: if (shift) { key = "_"; } else { key = "-"; } return true;
                    //case Keys.OemComma: if (shift) { key = "<"; } else { key = ","; } return true;
                    case Keys.Space: key = " "; return true;

                    case Keys.Back: key = "backspace"; return true;
                }
            }

            key = "";
            return false;
        }
    }
}
