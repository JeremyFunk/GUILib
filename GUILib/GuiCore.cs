using GUILib.GUI;
using GUILib.GUI.Render;
using GUILib.Util;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib
{
    public class GuiCore
    {
        public static DefaultRenderer guiRenderer = null;
        public static GuiScene guiScene = null;

        public static void InitializeGuiLib(int Width, int Height)
        {
            GameSettings.Width = Width;
            GameSettings.Height = Height;

            guiRenderer = guiRenderer == null ? new GuiRenderer() : guiRenderer;
            guiScene = guiScene == null ? new GuiScene() : guiScene;
        }

        public static void Update(float delta)
        {
            guiScene.Update(delta);
        }

        public static void Render()
        {
            guiRenderer.PrepareRender();
            guiRenderer.Render(guiScene);
        }

        public static void CleanUp()
        {
            guiRenderer.CleanUp();
        }

        public static void Resize(int Width, int Height)
        {
            GameSettings.Width = Width;
            GameSettings.Height = Height;
        }


        public static void KeyUp(KeyboardKeyEventArgs e)
        {
            GameInput.UpdateKey(e.Key, false);
        }

        public static void KeyDown(KeyboardKeyEventArgs e)
        {
            GameInput.UpdateKey(e.Key, true);
        }

        public static void MouseMove(MouseMoveEventArgs e)
        {
            GameInput.mouseDx = GameInput.mouseX - e.X;
            GameInput.mouseDy = GameInput.mouseY - e.Y;

            GameInput.mouseX = e.X;
            GameInput.mouseY = e.Y;

            GameInput.normalizedMouseX = (2f * e.X) / GameSettings.Width - 1;
            GameInput.normalizedMouseY = -1 * ((2f * e.Y) / GameSettings.Height - 1);
        }

        public static void MouseWheel(MouseWheelEventArgs e)
        {
            GameInput.mouseWheel = e.Delta;
            GameInput.mouseWheelF = e.DeltaPrecise;
        }

        public static void MouseLeave()
        {
            GameInput.mouseInside = false;
        }

        public static void MouseEnter()
        {
            GameInput.mouseInside = true;
        }

        public static void MouseDown(MouseButtonEventArgs e)
        {
            GameInput.UpdateMouseButton(e.Button, true);
        }

        public static void MouseUp(MouseButtonEventArgs e)
        {
            GameInput.UpdateMouseButton(e.Button, false);
        }
    }
}
