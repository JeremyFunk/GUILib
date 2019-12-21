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
using GUILib.GUI.Render.Fonts.Data;
using GUILib.GUI.Constraints;
using GUILib.GUI.PixelConstraints;

namespace GUILib.GUI.GuiElements
{
    class TextSelectable : GuiElement
    {
        private Material selectedMaterial, hoverMaterial, defaultMaterial;
        private Quad quad;
        private Text textElement;

        public TextSelectable(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string text, float fontSize, Material selectedMaterial, Material hoverMaterial, Font font = null, float zIndex = 0, float maxSize = 100000, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.selectedMaterial = selectedMaterial;
            this.hoverMaterial = hoverMaterial;
            this.defaultMaterial = new Material(new Vector4(0));
            
            quad = new Quad(defaultMaterial, 0, 0, width, height);

            quad.generalConstraint = new FillConstraintGeneral();

            quad.startHoverEvent = StartHover;
            quad.mouseButtonDownEvent = StartClick;
            quad.mouseButtonReleasedEvent = EndClick;
            quad.endHoverEvent = EndHover;

            textElement = new Text(x, y, text, fontSize, font, zIndex, maxSize, visible);
            textElement.xConstraints.Add(new CenterConstraint());

            quad.SetHeight(textElement.curHeight);
            
            this.SetHeight(textElement.curHeight);
            
            AddChild(textElement);
            AddChild(quad);
        }

        private void StartClick(MouseEvent e, GuiElement el)
        {
            quad.SetMaterial(hoverMaterial);
        }

        private void EndHover(MouseEvent e, GuiElement el)
        {
            quad.SetMaterial(defaultMaterial);
        }

        private void EndClick(MouseEvent e, GuiElement el)
        {
            quad.SetMaterial(hoverMaterial);
        }

        private void StartHover(MouseEvent e, GuiElement el)
        {
            quad.SetMaterial(hoverMaterial);
        }

        public string GetText()
        {
            return textElement.GetText();
        }
    }
}
