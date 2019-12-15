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
    class BordererdButton : GuiElement
    {
        private Text text;
        public BordererdButton(float x, float y, float width, float height, string text = "", float fontSize = -1, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, bool visible = true, int edgeSize = -1) : base(width, height, x, y, visible, zIndex)
        {
            if (fillMaterial == null)
               fillMaterial = Theme.defaultTheme.GetButtonFillMaterial();
            if (edgeMaterial == null)
                edgeMaterial = Theme.defaultTheme.GetButtonEdgeMaterial();
            if (edgeSize < 0)
                edgeSize = Theme.defaultTheme.GetButtonEdgeSize();
            if (fontSize < 0)
                fontSize = 1.2f;

            Border border = new Border(edgeMaterial, width, height, edgeSize);
            border.generalConstraint = new FillConstraintGeneral();

            Quad fillQuad = new Quad(fillMaterial);
            fillQuad.generalConstraint = new MarginConstraintGeneral(edgeSize);

            AddChild(fillQuad);
            AddChild(border);

            if (text != "")
            {
                this.text = new Text(0, 0, text, fontSize);
                this.text.xConstraints.Add(new CenterConstraint());
                this.text.yConstraints.Add(new CenterConstraint());
                AddChild(this.text);
            }
        }

        public override void MouseEventElement(MouseEvent events)
        {

        }

        public override void KeyEvent(KeyEvent events)
        {
        }

        protected override void RenderElement(GuiShader shader, Vector2 trans, Vector2 scale, float opacity)
        {
        }

        public override void UpdateElement(float delta)
        {}
    }
}
