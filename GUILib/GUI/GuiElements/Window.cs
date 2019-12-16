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

namespace GUILib.GUI.GuiElements
{
    class Window : GuiElement
    {
        float mouseDragX, mouseDragY;
        bool drag = false;

        public Window(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string title = "", float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            Quad background = new Quad(Theme.defaultTheme.GetWindowBackgroundMaterial(), 0, 0, 0, 0);
            background.generalConstraint = new MarginConstraintGeneral(Theme.defaultTheme.GetWindowEdgeSize());
            
            Quad topBar = new Quad(Theme.defaultTheme.GetWindowTopBarMaterial(), 0, 0, 1f, Theme.defaultTheme.GetWindowTopBarSize());
            topBar.yConstraints.Add(new MarginConstraint(0));
            topBar.mouseButtonPressedEvent = TopBarDragEvent;
            topBar.mouseButtonPressedMissedEvent = TopBarDragEvent;
            topBar.mouseButtonDownEvent = TopBarDownEvent;
            topBar.mouseButtonReleasedEvent = TopBarReleasedEvent;

            Border border = new Border(Theme.defaultTheme.GetWindowEdgeMaterial(), width, height, Theme.defaultTheme.GetWindowEdgeSize());
            border.generalConstraint = new FillConstraintGeneral();

            Material close = new Material(new Texture("Icons/Close.png"));
            Material closeHovered = new Material(new Texture("Icons/CloseHover.png"));
            Material closeClicked = new Material(new Texture("Icons/CloseClick.png"));

            Button button = new Button(0, 0, 28, 28, close);
            button.xConstraints.Add(new MarginConstraint(9));
            button.yConstraints.Add(new MarginConstraint(5));

            button.defaultMaterial = close;
            button.hoverMaterial = closeHovered;
            button.clickMaterial = closeClicked;

            AddChild(background);
            AddChild(topBar);
            AddChild(button);

            if (title != "")
            {
                Text text = new Text(10, 0, title, 0.8f);
                text.color = new Vector4(0.9f, 0.9f, 0.9f, 1f);

                text.yConstraints.Add(new MarginConstraint(6));
                AddChild(text);
            }


            AddChild(border);
        }

        private void TopBarDownEvent(MouseEvent e, GuiElement el)
        {
            if (e.leftButtonDown)
            {
                mouseDragX = e.mousePositionWorld.X;
                mouseDragY = e.mousePositionWorld.Y;
                drag = true;
            }
        }

        private void TopBarReleasedEvent(MouseEvent e, GuiElement el)
        {
            if (drag)
            {
                SetX((int)(curX + (e.mousePositionWorld.X - mouseDragX)));
                SetY((int)(curY + (e.mousePositionWorld.Y - mouseDragY)));
                drag = false;
            }
        }

        private void TopBarDragEvent(MouseEvent e, GuiElement el)
        {
            if (e.leftButtonDown)
            {
                if (drag)
                {
                    SetX((int)(curX + (e.mousePositionWorld.X - mouseDragX)));
                    SetY((int)(curY + (e.mousePositionWorld.Y - mouseDragY)));

                    mouseDragX = e.mousePositionWorld.X;
                    mouseDragY = e.mousePositionWorld.Y;
                }
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
        { }

        public int GetTopBarSize()
        {
            return Theme.defaultTheme.GetWindowTopBarSize();
        }
    }
}
