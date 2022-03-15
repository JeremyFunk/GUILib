using OpenTK;
using GUILib.GUI.Render.Shaders;
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
    public class TickBox : GuiElement
    {
        private Material fillMaterialClick, fillMaterialHover, fillMaterialDefault;

        public Action<TickBox> tickBoxChangedEvent;

        private Quad clickQuad, quad;

        private bool clicked = false;

        public TickBox(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, bool ticked = false, Action<TickBox> tickBoxChangedEvent = null, Material fillMaterialClick = null, Material fillMaterialHover = null, Material fillMaterialDefault = null, Material fillMaterialClicked = null, Material edgeMaterial = null, int edgeSize = -1, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            clicked = ticked;

            this.tickBoxChangedEvent = tickBoxChangedEvent;

            this.fillMaterialClick = fillMaterialClick == null ? Theme.defaultTheme.GetTickBoxClickMaterial() : fillMaterialClick;
            this.fillMaterialHover = fillMaterialHover == null ? Theme.defaultTheme.GetTickBoxHoverMaterial() : fillMaterialHover;
            this.fillMaterialDefault = fillMaterialDefault == null ? Theme.defaultTheme.GetTickBoxDefaultMaterial() : fillMaterialDefault;
            fillMaterialClicked = fillMaterialClicked == null ? Theme.defaultTheme.GetTickBoxClickedMaterial() : fillMaterialClicked;

            int borderWidth = this.fillMaterialDefault.GetBorderSize();

            quad = new Quad(0, 0, 0, 0, this.fillMaterialDefault);
            quad.generalConstraint = new FillConstraintGeneral();

            clickQuad = new Quad(borderWidth, borderWidth, 1f, 1f, fillMaterialClicked, 1);
            clickQuad.widthConstraints.Add(new SubtractConstraint(borderWidth * 2));
            clickQuad.heightConstraints.Add(new SubtractConstraint(borderWidth * 2));

            clickQuad.visible = ticked;

            AddChild(quad);
            AddChild(clickQuad);
        }

        public bool IsClicked()
        {
            return clicked;
        }

        public void SetClicked(bool clicked)
        {
            this.clicked = clicked;
            clickQuad.visible = clicked;
        }

        public override void MouseEventElement(MouseEvent e)
        {
            if (e.hit)
            {
                if (e.leftMouseButtonType != MouseButtonType.None)
                {
                    if (e.leftMouseButtonType == MouseButtonType.Released)
                    {
                        clicked = !clicked;

                        tickBoxChangedEvent?.Invoke(this);

                        clickQuad.visible = clicked;
                        quad.SetMaterial(fillMaterialHover);
                    }
                    else
                    {
                        quad.SetMaterial(fillMaterialClick);
                    }
                }
                else
                {
                    quad.SetMaterial(fillMaterialHover);
                }
            }
            else
            {
                quad.SetMaterial(fillMaterialDefault);
            }
        }
    }
}
