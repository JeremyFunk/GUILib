using OpenTK;
using GUILib.GUI.Render.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Events;
using GUILib.GUI.Constraints;
using GUILib.GUI.PixelConstraints;

namespace GUILib.GUI.GuiElements
{
    class Border : GuiElement
    {
        public Border(Material material, APixelConstraint width, APixelConstraint height, int borderSize, float zIndex = 0, bool visible = true) : base(width, height, 0, 0, visible, zIndex)
        {
            Quad left = new Quad(material, 0, 0, borderSize, 1f);
            Quad right = new Quad(material, 0, 0, borderSize, 1f);
            right.xConstraints.Add(new MarginConstraint(0));

            Quad bot = new Quad(material, borderSize, 0, 1f, borderSize);
            bot.widthConstraints.Add(new SubtractConstraint(borderSize * 2));

            Quad top = new Quad(material, borderSize, 1f, 1f, borderSize);
            top.yConstraints.Add(new MarginConstraint(0));
            top.widthConstraints.Add(new SubtractConstraint(borderSize * 2));

            AddChild(left);
            AddChild(right);
            AddChild(top);
            AddChild(bot);
        }
    }
}
