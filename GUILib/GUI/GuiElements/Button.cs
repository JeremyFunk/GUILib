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
        public Button(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string text = "", bool defaultBehaviour = false, float fontSize = 1f, Material material = null, Material hoverMaterial = null, Material clickMaterial = null, float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            curMaterial = material == null ? Theme.defaultTheme.GetButtonFillMaterial() : material;

            quad = new Quad(0, 0, width, height, curMaterial);
            quad.generalConstraint = new FillConstraintGeneral();

            AddChild(quad);

            if (text != "")
            {
                this.text = new Text(0, 0, text, fontSize);
                this.text.xConstraints.Add(new CenterConstraint());
                this.text.yConstraints.Add(new CenterConstraint());
                AddChild(this.text);
            }

            if (defaultBehaviour)
            {
                startHoverEvent = HoverStart;
                endHoverEvent = HoverEnd;
                mouseButtonPressedEvent = Click;
                mouseButtonReleasedEvent = HoverStart;
            }

            defaultMaterial = curMaterial;
            this.clickMaterial = clickMaterial == null ? Theme.defaultTheme.GetButtonClickMaterial() : clickMaterial;
            this.hoverMaterial = hoverMaterial == null ? Theme.defaultTheme.GetButtonHoverMaterial() : hoverMaterial;
        }

        private void HoverStart(MouseEvent e, GuiElement el)
        {
            SetMaterial(hoverMaterial);
        }
        private void HoverEnd(MouseEvent e, GuiElement el)
        {
            SetMaterial(defaultMaterial);
        }
        private void Click(MouseEvent e, GuiElement el)
        {
            SetMaterial(clickMaterial);
        }

        public void SetMaterial(Material material)
        {
            quad.SetMaterial(material);
        }

        public override void UpdateElement(float delta)
        {
            quad.SetMaterial(curMaterial);
        }
    }
}
