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
    class BorderedQuad : GuiElement
    {
        public BorderedQuad(float x, float y, float width, float height, Material fillMaterial, Material edgeMaterial, int edgeSize, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            Border border = new Border(edgeMaterial, width, height, edgeSize);
            border.generalConstraint = new FillConstraintGeneral();

            Quad fillQuad = new Quad(fillMaterial);
            fillQuad.generalConstraint = new MarginConstraintGeneral(edgeSize);

            AddChild(fillQuad);
            AddChild(border);
        }
    }
}
