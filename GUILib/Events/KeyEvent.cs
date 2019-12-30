using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace GUILib.Events
{
    ///<summary>Class <c>KeyEvent</c> will be passed to every visible GUI Element every update, if one or more key events occured.</summary>
    class KeyEvent
    {
        private const char nullChar = '`';

        ///<summary>The list <c>down</c> contains every Key that is pressed at the creation of this KeyEvent.</summary>
        public List<Key> down;
        ///<summary>The list <c>pressed</c> contains every Key that is pressed at the creation of this KeyEvent, if it was not pressed on the previous update.</summary>
        public List<Key> pressed;
        ///<summary>The dictionary <c>keyCharsDown</c> contains the same values as keys as <c>down</c>. The value of each key is the character that corresponds to that key.</summary>
        public Dictionary<Key, char> keyCharsDown = new Dictionary<Key, char>();
        ///<summary>The dictionary <c>keyCharsPressed</c> contains the same values as keys as <c>pressed</c>. The value of each key is the character that corresponds to that key.</summary>
        public Dictionary<Key, char> keyCharsPressed = new Dictionary<Key, char>();

        public KeyEvent(List<Key> down, List<Key> pressed)
        {
            this.down = down;
            this.pressed = pressed;

            GenerateKeyChars(this);
        }

        private static void GenerateKeyChars(KeyEvent e)
        {
            foreach(Key k in e.down)
            {
                char kChar = GetKeyChar(k, e);
                if(kChar != nullChar)
                {
                    e.keyCharsDown.Add(k, kChar);
                }
            }

            foreach (Key k in e.pressed)
            {
                char kChar = GetKeyChar(k, e);
                if (kChar != nullChar)
                {
                    e.keyCharsPressed.Add(k, kChar);
                }
            }
        }

        private static char GetKeyChar(Key k, KeyEvent e)
        {
            if(e.down.Contains(Key.LShift) || e.down.Contains(Key.RShift))
            {
                switch (k)
                {
                    case Key.A: return 'A';
                    case Key.B: return 'B';
                    case Key.C: return 'C';
                    case Key.D: return 'D';
                    case Key.E: return 'E';
                    case Key.F: return 'F';
                    case Key.G: return 'G';
                    case Key.H: return 'H';
                    case Key.I: return 'I';
                    case Key.J: return 'J';
                    case Key.K: return 'K';
                    case Key.L: return 'L';
                    case Key.M: return 'M';
                    case Key.N: return 'N';
                    case Key.O: return 'O';
                    case Key.P: return 'P';
                    case Key.Q: return 'Q';
                    case Key.R: return 'R';
                    case Key.S: return 'S';
                    case Key.T: return 'T';
                    case Key.U: return 'U';
                    case Key.V: return 'V';
                    case Key.W: return 'W';
                    case Key.X: return 'X';
                    case Key.Y: return 'Z';
                    case Key.Z: return 'Y';
                    case Key.BracketLeft: return 'Ü';
                    case Key.Quote: return 'Ä';
                    case Key.Semicolon: return 'Ö';
                }
            }
            else
            {
                switch (k)
                {
                    case Key.A: return 'a';
                    case Key.B: return 'b';
                    case Key.C: return 'c';
                    case Key.D: return 'd';
                    case Key.E: return 'e';
                    case Key.F: return 'f';
                    case Key.G: return 'g';
                    case Key.H: return 'h';
                    case Key.I: return 'i';
                    case Key.J: return 'j';
                    case Key.K: return 'k';
                    case Key.L: return 'l';
                    case Key.M: return 'm';
                    case Key.N: return 'n';
                    case Key.O: return 'o';
                    case Key.P: return 'p';
                    case Key.Q: return 'q';
                    case Key.R: return 'r';
                    case Key.S: return 's';
                    case Key.T: return 't';
                    case Key.U: return 'u';
                    case Key.V: return 'v';
                    case Key.W: return 'w';
                    case Key.X: return 'x';
                    case Key.Y: return 'z';
                    case Key.Z: return 'y';
                    case Key.BracketLeft: return 'ü';
                    case Key.Quote: return 'ä';
                    case Key.Semicolon: return 'ö';
                }
            }

            switch (k)
            {
                case Key.Minus: return 'ß';
                case Key.Space: return ' ';
                case Key.Number0: return '0';
                case Key.Number1: return '1';
                case Key.Number2: return '2';
                case Key.Number3: return '3';
                case Key.Number4: return '4';
                case Key.Number5: return '5';
                case Key.Number6: return '6';
                case Key.Number7: return '7';
                case Key.Number8: return '8';
                case Key.Number9: return '9';
                case Key.Keypad0: return '0';
                case Key.Keypad1: return '1';
                case Key.Keypad2: return '2';
                case Key.Keypad3: return '3';
                case Key.Keypad4: return '4';
                case Key.Keypad5: return '5';
                case Key.Keypad6: return '6';
                case Key.Keypad7: return '7';
                case Key.Keypad8: return '8';
                case Key.Keypad9: return '9';
            }

            return nullChar;
        }
    }
}
