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
    class UnderlinedButton : GuiElement
    {
        private Quad quad;
        private Quad underline;
        private Text text;

        public Vector4 defaultTextColor = new Vector4(1);
        public Vector4 hoverTextColor = new Vector4(1);
        public UnderlinedButton(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string text = "", bool defaultBehaviour = false, float fontSize = 1f, Material material = null, Material hoverMaterial = null, Material clickMaterial = null, Material underlineMaterial = null, Material underlineHoverMaterial = null, Material underlineClickMaterial = null, int underlineHeight = 2, float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            curMaterial = material == null ? Theme.defaultTheme.GetButtonFillMaterial() : material;

            quad = new Quad(0, 0, width, height, curMaterial);

            underline = new Quad(0, 0, width, underlineHeight, curMaterial);
            AddChild(underline);

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

            underline.defaultMaterial = underlineMaterial == null ? defaultMaterial : underlineMaterial;
            underline.clickMaterial = underlineClickMaterial == null ? this.clickMaterial : underlineClickMaterial;
            underline.hoverMaterial = underlineHoverMaterial == null ? this.hoverMaterial : underlineHoverMaterial;

            underline.SetMaterial(underline.defaultMaterial);
        }

        private void HoverStart(MouseEvent e, GuiElement el)
        {
            SetMaterial(hoverMaterial);
            underline.SetMaterial(underline.hoverMaterial);
            text.color = hoverTextColor;
        }
        private void HoverEnd(MouseEvent e, GuiElement el)
        {
            SetMaterial(defaultMaterial);
            underline.SetMaterial(underline.defaultMaterial);
            text.color = defaultTextColor;
        }
        private void Click(MouseEvent e, GuiElement el)
        {
            SetMaterial(clickMaterial);
            underline.SetMaterial(underline.clickMaterial);
        }

        public void SetMaterial(Material material)
        {
            quad.SetMaterial(material);
        }

        public override void UpdateElement(float delta)
        {
            quad.SetMaterial(curMaterial);
        }

        internal void SetTextColor(Vector4 color)
        {
            text.color = color;
        }
    }
}
