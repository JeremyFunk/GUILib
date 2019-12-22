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
    class TickBox : GuiElement
    {
        private Material fillMaterialClick, fillMaterialHover, fillMaterialDefault;

        private Quad clickQuad, quad;

        private bool clicked = false;

        public TickBox(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, bool ticked = false, Material fillMaterialClick = null, Material fillMaterialHover = null, Material fillMaterialDefault = null, Material fillMaterialClicked = null, Material edgeMaterial = null, int edgeSize = -1, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            clicked = ticked;

            this.fillMaterialClick = fillMaterialClick == null ? Theme.defaultTheme.GetTickBoxClickMaterial() : fillMaterialClick;
            this.fillMaterialHover = fillMaterialHover == null ? Theme.defaultTheme.GetTickBoxHoverMaterial() : fillMaterialHover;
            this.fillMaterialDefault = fillMaterialDefault == null ? Theme.defaultTheme.GetTickBoxDefaultMaterial() : fillMaterialDefault;
            fillMaterialClicked = fillMaterialClicked == null ? Theme.defaultTheme.GetTickBoxClickedMaterial() : fillMaterialClicked;
            edgeMaterial = edgeMaterial == null ? Theme.defaultTheme.GetTickBoxEdgeMaterial() : edgeMaterial;
            edgeSize = edgeSize == -1 ? Theme.defaultTheme.GetTickBoxEdgeSize() : edgeSize;

            quad = new Quad(this.fillMaterialDefault, 0, 0, 0, 0);
            quad.generalConstraint = new FillConstraintGeneral();

            clickQuad = new Quad(fillMaterialClicked, 0, 0, 1f, 1f, 1);
            quad.generalConstraint = new FillConstraintGeneral();
            clickQuad.visible = ticked;

            Border border = new Border(edgeMaterial, 0, 0, edgeSize, 2);
            border.generalConstraint = new FillConstraintGeneral();

            AddChild(quad);
            AddChild(clickQuad);
            AddChild(border);
        }

        public override void MouseEventElement(MouseEvent e)
        {
            if (e.hit)
            {
                if (e.leftButtonDown)
                {
                    if (e.mouseButtonType == MouseButtonType.Released)
                    {
                        clicked = !clicked;

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
