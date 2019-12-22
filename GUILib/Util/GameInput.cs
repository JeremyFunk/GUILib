
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace GUILib.Util
{
    static class GameSettings
    {
        public static int Width, Height;
        public const int UPS = 60;
        public const float AnimationFramesPerUpdate = 0.5f;
        public const float CursorTickSpeed = 0.5f;
        public const float ScrollSpeed = 20f;
    }

    static class GameInput
    {
        public static void Initialize()
        {
            InitKeys();
            InitMouse();
        }

        #region Key input
        private static Dictionary<Key, bool> keys = new Dictionary<Key, bool>();
        private static List<Key> keysPressed = new List<Key>(), keysBlocked = new List<Key>();

        public static bool isKeyDown(Key key)
        {
            lock (keys)
                return keys[key];
        }

        public static bool IsKeyPressed(Key key)
        {
            lock (keysPressed)
            {
                if (!keysPressed.Contains(key))
                    return false;

                keysPressed.Remove(key);

                lock (keysBlocked)
                    if (!keysBlocked.Contains(key))
                        keysBlocked.Add(key);

                return true;
            }
        }

        public static void UpdateKey(Key key, bool down)
        {
            lock (keys)
                keys[key] = down;

            lock (keysPressed)
            {
                if (down)
                {
                    if (!keysPressed.Contains(key) && !keysBlocked.Contains(key))
                        keysPressed.Add(key);
                }
                else
                {
                    if (keysPressed.Contains(key))
                        keysPressed.Remove(key);

                    if (keysBlocked.Contains(key))
                        keysBlocked.Remove(key);
                }
            }
        }

        private static void InitKeys()
        {
            lock (keys)
            {
                keys = new Dictionary<Key, bool>();

                var allKeys = Enum.GetValues(typeof(Key));

                foreach (Key key in allKeys)
                {
                    if (!keys.ContainsKey(key))
                        keys.Add(key, false);
                }
            }

            lock (keysPressed)
                keysPressed.Clear();

            lock (keysBlocked)
                keysBlocked.Clear();


        }

        public static List<Key> GetKeysPressed()
        {
            List<Key> keysPressed = new List<Key>();

            var allKeys = Enum.GetValues(typeof(Key));

            foreach (Key key in allKeys)
            {
                if (IsKeyPressed(key))
                    keysPressed.Add(key);
            }
            return keysPressed;
        }

        public static List<Key> GetKeysDown()
        {
            List<Key> returnKeys = new List<Key>();

            var allKeys = Enum.GetValues(typeof(Key));

            foreach (Key k in allKeys)
            {
                if (keys[k])
                {
                    returnKeys.Add(k);
                }
            }

            return returnKeys;
        }
        #endregion

        #region Mouse input
        public static int mouseX, mouseY, mouseWheel;
        public static float normalizedMouseX, normalizedMouseY, mouseWheelF;
        public static bool mouseInside, mouseGrabbed, mouseStateChanged;

        private static Dictionary<MouseButton, bool> mkeys = new Dictionary<MouseButton, bool>();
        private static List<MouseButton> mkeysPressed = new List<MouseButton>(), mkeysBlocked = new List<MouseButton>();
        private static List<MouseButton> mkeysReleased = new List<MouseButton>();

        public static bool IsMouseButtonDown(MouseButton key)
        {
            lock (mkeys)
                return mkeys[key];
        }

        public static bool IsMouseButtonReleased(MouseButton key)
        {
            lock (mkeysReleased)
                return mkeysReleased.Contains(key);
        }

        public static bool IsMouseButtonPressed(MouseButton key)
        {
            lock (mkeysPressed)
            {
                if (!mkeysPressed.Contains(key))
                    return false;

                mkeysPressed.Remove(key);

                lock (mkeysBlocked)
                    if (!mkeysBlocked.Contains(key))
                        mkeysBlocked.Add(key);

                return true;
            }
        }

        public static void UpdateMouseButton(MouseButton key, bool down)
        {
            lock (mkeys)
                mkeys[key] = down;

            lock (mkeysPressed)
            {
                if (down)
                {
                    if (!mkeysPressed.Contains(key) && !mkeysBlocked.Contains(key))
                        mkeysPressed.Add(key);
                }
                else
                {
                    mkeysReleased.Add(key);
                    if (mkeysPressed.Contains(key))
                        mkeysPressed.Remove(key);

                    if (mkeysBlocked.Contains(key))
                        mkeysBlocked.Remove(key);
                }
            }
        }

        private static void InitMouse()
        {
            lock (mkeys)
            {
                mkeys = new Dictionary<MouseButton, bool>();

                var allKeys = Enum.GetValues(typeof(MouseButton));

                foreach (MouseButton key in allKeys)
                {
                    if (!mkeys.ContainsKey(key))
                        mkeys.Add(key, false);
                }
            }

            lock (mkeysPressed)
                mkeysPressed.Clear();

            lock (mkeysBlocked)
                mkeysBlocked.Clear();
        }
        #endregion

        public static int mouseDx = 0, mouseDy = 0;
        public static void Update()
        {
            mouseDx = 0;
            mouseDy = 0;
            mouseWheel = 0;

            mkeysReleased.Clear();
        }
    }
}
