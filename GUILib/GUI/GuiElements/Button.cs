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
    class Button : GuiElement
    {
        private Quad quad;
        private Text text;
        public Button(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, Material material, string text = "", float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            curMaterial = material;

            quad = new Quad(material, 0, 0, width, height);
            quad.generalConstraint = new FillConstraintGeneral();

            AddChild(quad);

            if (text != "")
            {
                this.text = new Text(0, 0, text, 1.2f);
                this.text.xConstraints.Add(new CenterConstraint());
                this.text.yConstraints.Add(new CenterConstraint());
                AddChild(this.text);
            }
        }

        public override void UpdateElement(float delta)
        {
            quad.SetMaterial(curMaterial);
        }
    }
}
