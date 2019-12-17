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
    class BorderedButton : GuiElement
    {
        private Text text;
        public bool defaultBehaviour = false;
        private BorderedQuad quad;
        public Material defaultMaterial;

        public BorderedButton(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string text = "", bool defaultBehaviour = false, float fontSize = -1, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, bool visible = true, int edgeSize = -1) : base(width, height, x, y, visible, zIndex)
        {
            if (fillMaterial == null)
               fillMaterial = Theme.defaultTheme.GetButtonFillMaterial();
            if (edgeMaterial == null)
                edgeMaterial = Theme.defaultTheme.GetButtonEdgeMaterial();
            if (edgeSize < 0)
                edgeSize = Theme.defaultTheme.GetButtonEdgeSize();
            if (fontSize < 0)
                fontSize = 1.2f;

            quad = new BorderedQuad(0, 0, width, height, fillMaterial, edgeMaterial, edgeSize);

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

            defaultMaterial = fillMaterial;
        }


        private static void HoverStart(MouseEvent e, GuiElement el)
        {
            ((BorderedButton)el).SetMaterial(Theme.defaultTheme.GetButtonHoverMaterial());
        }
        private static void HoverEnd(MouseEvent e, GuiElement el)
        {
            ((BorderedButton)el).SetMaterial(((BorderedButton)el).defaultMaterial);
        }
        private static void Click(MouseEvent e, GuiElement el)
        {
            ((BorderedButton)el).SetMaterial(Theme.defaultTheme.GetButtonClickMaterial());
        }

        public void SetMaterial(Material material)
        {
            quad.SetFillMaterial(material);
        }
    }
}
