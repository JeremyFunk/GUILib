using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GUILib
{
    class Program
    {
        const int SWP_NOZORDER = 0x4;
        const int SWP_NOACTIVATE = 0x10;

        [DllImport("kernel32")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int cx, int cy, int flags);

        static void Main(string[] args)
        {
            /*Bitmap map = new Bitmap(@"C:\Users\Jerem\Desktop\GUILib\GUILib\Resource\Textures\Background.jpg");
            Bitmap map1 = new Bitmap(192, 108);

            for(int x = 0; x < 192; x++)
            {
                for (int y = 0; y < 108; y++)
                {
                    map1.SetPixel(x, y, map.GetPixel(x * 10, y * 10));
                }
            }

            Bitmap newMap = new Bitmap(1920, 1080);

            for (int x = 0; x < 192; x++)
            {
                for (int y = 0; y < 108; y++)
                {
                    Color llC = map1.GetPixel(x, y);
                    Color hlC = map1.GetPixel(x == 191 ? x : x + 1, y);
                    Color lhC = map1.GetPixel(x, y == 107 ? y : y + 1);
                    Color hhC = map1.GetPixel(x == 191 ? x : x + 1, y == 107 ? y : y + 1);

                    for (int x2 = 0; x2 < 10; x2++)
                    {
                        for (int y2 = 0; y2 < 10; y2++)
                        {
                            Color resultColor = Color.FromArgb(Interpolate(llC.R, hhC.R, lhC.R, hlC.R, x2 / 10f, y2 / 10f), Interpolate(llC.G, hhC.G, lhC.G, hlC.G, x2 / 10f, y2 / 10f), Interpolate(llC.B, hhC.B, lhC.B, hlC.B, x2 / 10f, y2 / 10f));

                            newMap.SetPixel(x * 10 + x2, y * 10 + y2, resultColor);
                        }
                    }
                }
            }

            newMap.Save("Hallo.png");*/

            SetWindowPosition(0, 0, 500, 1000);

            EngineCore core = new EngineCore(1920, 1080, "Hallo Welt");

            core.Run(144, 144);
        }

        private static int Interpolate(int llC, int hhC, int lhC, int hlC, float xF, float yF)
        {
            return Clamp((int)(((llC + hlC * xF) + (lhC + hhC * xF) + (llC + lhC * yF) + (lhC + hhC * yF)) / 2));
        }

        private static int Clamp(int v)
        {
            return v < 0 ? 0 : v > 255 ? 255 : v;
        }

        /// <summary>
        /// Sets the console window location and size in pixels
        /// </summary>
        public static void SetWindowPosition(int x, int y, int width, int height)
        {
            SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
        }

        public static IntPtr Handle
        {
            get
            {
                //Initialize();
                return GetConsoleWindow();
            }
        }
    }
}
