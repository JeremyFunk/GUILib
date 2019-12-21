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

namespace GUILib.GUI.GuiElements
{
    class TextArea : GuiElement
    {
        private Text text;
        public TextArea(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string text = "", float fontSize = -1, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, bool visible = true, int edgeSize = -1) : base(width, height, x, y, visible, zIndex)
        {
            if (fillMaterial == null)
                fillMaterial = Theme.defaultTheme.GetButtonFillMaterial();
            if (edgeMaterial == null)
                edgeMaterial = Theme.defaultTheme.GetButtonEdgeMaterial();
            if (edgeSize < 0)
                edgeSize = Theme.defaultTheme.GetButtonEdgeSize();
            if (fontSize < 0)
                fontSize = 1.2f;

            BorderedQuad quad = new BorderedQuad(0, 0, width, height, fillMaterial, edgeMaterial, edgeSize);
            //quad.generalConstraint = new FillConstraintGeneral();

            AddChild(quad);

            if (text != "")
            {
                this.text = new Text(6, 0, text, fontSize, null, 0, curWidth - 12);
                this.text.yConstraints.Add(new MarginConstraint(3));
                //this.text.xConstraints.Add(new CenterConstraint());
                //this.text.yConstraints.Add(new CenterConstraint());
                AddChild(this.text);
            }
        }
    }
}
