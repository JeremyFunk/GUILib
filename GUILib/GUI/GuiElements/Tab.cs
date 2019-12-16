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

namespace GUILib.GUI.GuiElements
{
    class Tab : GuiElement
    {
        Text textElement;

        public Tab(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string text = "", float fontSize = -1, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            if (fillMaterial == null)
                fillMaterial = Theme.defaultTheme.GetTabFillMaterial();
            if (edgeMaterial == null)
                edgeMaterial = Theme.defaultTheme.GetTabEdgeMaterial();
            if (fontSize < 0)
                fontSize = 0.8f;

            BorderedQuad tabQuad = new BorderedQuad(0, 0, width, height, fillMaterial, edgeMaterial, Theme.defaultTheme.GetTabEdgeSize());
            AddChild(tabQuad);
            if (text != "")
            {
                textElement = new Text(0, 0, text, fontSize);
                textElement.xConstraints.Add(new CenterConstraint());
                textElement.yConstraints.Add(new CenterConstraint());
                AddChild(textElement);
            }
        }


        public void SetTextColor(Vector4 color)
        {
            textElement.color = color;
        }
    }
}
