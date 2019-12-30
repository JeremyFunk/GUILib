using OpenTK;
using GUILib.GUI.Render.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Events;
using GUILib.GUI.GuiElements;
using GUILib.GUI;
using GUILib.GUI.Animations;
using GUILib.GUI.Constraints;
using GUILib.GUI.PixelConstraints;

namespace GUILib.Modern
{
    class UnderlinedText : GuiElement
    {
        private Quad underline;
        private Text text;

        public UnderlinedText(APixelConstraint x, APixelConstraint y, APixelConstraint width, Text text, Material underlineMaterial, int yOffset = 0, int underlineHeight = 2, float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, text.curHeight, x, y, visible, zIndex)
        {
            underline = new Quad(0, -yOffset, width, underlineHeight, underlineMaterial);
            AddChild(underline);

            this.text = text;
            AddChild(this.text);
        }
    }
}
