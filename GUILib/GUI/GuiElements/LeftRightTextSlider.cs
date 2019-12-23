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
    class LeftRightTextSlider : GuiElement
    {
        private Text text;
        public bool defaultBehaviour = false;
        private BorderedQuad quad;

        private List<string> options = new List<string>();
        private string selectedText = "";

        private bool hovered = false;

        public LeftRightTextSlider(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, List<string> options, float fontSize = -1, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, bool visible = true, int edgeSize = -1) : base(width, height, x, y, visible, zIndex)
        {
            this.options = options; 

            if (fillMaterial == null)
                fillMaterial = Theme.defaultTheme.GetLeftRightTextSliderFillMaterial();
            if (edgeMaterial == null)
                edgeMaterial = Theme.defaultTheme.GetLeftRightTextSliderEdgeMaterial();
            if (edgeSize < 0)
                edgeSize = Theme.defaultTheme.GetButtonEdgeSize();
            if (fontSize < 0)
                fontSize = 0.8f;

            quad = new BorderedQuad(0, 0, width, height, fillMaterial, edgeMaterial, edgeSize);
            quad.generalConstraint = new FillConstraintGeneral();

            quad.startHoverEvent = StartHover;
            quad.endHoverEvent = EndHover;

            AddChild(quad);

            selectedText = options[0];

            this.text = new Text(0, 0, selectedText, fontSize);
            this.text.xConstraints.Add(new CenterConstraint());
            this.text.yConstraints.Add(new CenterConstraint());
            AddChild(this.text);

            Quad leftQuad = new Quad(Theme.defaultTheme.GetLeftArrowEdgeMaterial(), 10, 0, 20, 20);
            leftQuad.yConstraints.Add(new CenterConstraint());
            leftQuad.mouseButtonReleasedEvent = LeftClick;

            Quad rightQuad = new Quad(Theme.defaultTheme.GetRightArrowEdgeMaterial(), 10, 0, 20, 20);
            rightQuad.yConstraints.Add(new CenterConstraint());
            rightQuad.xConstraints.Add(new MarginConstraint(10));
            rightQuad.mouseButtonReleasedEvent = RightClick;

            AddChild(leftQuad);
            AddChild(rightQuad);

            defaultMaterial = fillMaterial;
        }

        private void EndHover(MouseEvent e, GuiElement el)
        {
            hovered = false;
        }

        private void StartHover(MouseEvent e, GuiElement el)
        {
            hovered = true;
        }

        public override void KeyEventElement(KeyEvent e)
        {
            if (hovered)
            {
                if (e.pressed.Contains(OpenTK.Input.Key.Left))
                {
                    NextLeft();
                }
                else if (e.pressed.Contains(OpenTK.Input.Key.Right))
                {
                    NextRight();
                }
            }
        }

        private void LeftClick(MouseEvent e, GuiElement el)
        {
            NextLeft();
        }

        private void NextLeft()
        {
            int curPos = options.IndexOf(selectedText) - 1;

            if (curPos < 0)
            {
                curPos = options.Count - 1;
            }

            selectedText = options[curPos];
            text.SetText(selectedText);
        }

        private void RightClick(MouseEvent e, GuiElement el)
        {
            NextRight();
        }

        private void NextRight()
        {
            int curPos = options.IndexOf(selectedText) + 1;

            if (curPos >= options.Count)
            {
                curPos = 0;
            }

            selectedText = options[curPos];
            text.SetText(selectedText);
        }
    }
}
