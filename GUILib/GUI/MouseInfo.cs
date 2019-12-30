using OpenTK;
using GUILib.GUI.Render.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Events;
using GUILib.GUI.Animations;
using GUILib.GUI.Constraints;
using GUILib.GUI.PixelConstraints;
using GUILib.GUI.GuiElements;
using GUILib.Util;

namespace GUILib.GUI
{
    /// <summary>
    /// This class can create hover mouse information.
    /// </summary>
    class MouseInfo
    {
        private static Quad quad;
        private static string newInfo = "";
        private static Text lastText;

        public static void Init(GuiScene scene)
        {
            quad = new Quad(0, 0, 0, 0, Theme.defaultTheme.GetMouseInfoMaterial(), float.MaxValue - 1);
            quad.debugIdentifier = "Z";
            quad.visible = false;
            scene.AddParent(quad);
        }

        public static void SetMouseInfo(string info)
        {
            quad.visible = true;
            newInfo = info;
        }

        public static void HideMouseInfo()
        {
            quad.visible = false;
        }
        public static void UpdatePosition(int x, int y)
        {
            if(newInfo != "")
            {
                Text text = new Text(0, 0, newInfo, 0.8f);
                if (lastText != null)
                    quad.RemoveChild(lastText);
                quad.AddChild(text);

                text.xConstraints.Add(new CenterConstraint());
                text.yConstraints.Add(new CenterConstraint());
                quad.SetWidth(text.curWidth + 20);
                quad.SetHeight(text.curHeight + 20);

                text.FirstUpdate(quad.curWidth, quad.curHeight, 0.00001f);

                lastText = text;

                newInfo = "";
            }

            quad.SetX(x + 10);
            quad.SetY(y - 10);
            quad.Update(GameSettings.Width, GameSettings.Height, 0.0001f, false);
        }
    }
}
