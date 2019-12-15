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
        Material close, closeHovered, closeClicked;

        public Window(float x, float y, float width, float height, string title = "", float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            Quad background = new Quad(Theme.defaultTheme.GetWindowBackgroundMaterial());
            background.generalConstraint = new MarginConstraintGeneral(Theme.defaultTheme.GetWindowEdgeSize());

            Quad topBar = new Quad(Theme.defaultTheme.GetWindowTopBarMaterial(), 0, 0, 1f, Theme.defaultTheme.GetWindowTopBarSize());
            topBar.yConstraints.Add(new MarginConstraint(0));

            Border border = new Border(Theme.defaultTheme.GetWindowEdgeMaterial(), width, height, Theme.defaultTheme.GetWindowEdgeSize());
            border.generalConstraint = new FillConstraintGeneral();

            close = new Material(new Texture("Icons/Close.png"));
            closeHovered = new Material(new Texture("Icons/CloseHover.png"));
            closeClicked = new Material(new Texture("Icons/CloseClick.png"));

            Button button = new Button(0, 0, 28, 28, close);
            button.xConstraints.Add(new MarginConstraint(9));
            button.yConstraints.Add(new MarginConstraint(5));

            button.mouseButtonPressedEvent = ButtonPressed;
            button.mouseButtonReleasedEvent = ButtonReleased;
            button.startHoverEvent = ButtonHovered;
            button.endHoverEvent = ButtonEndHovered;

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

        private void ButtonHovered(MouseEvent e, GuiElement el)
        {
            ((Button)el).SetMaterial(closeHovered);
        }
        private void ButtonEndHovered(MouseEvent e, GuiElement el)
        {
            ((Button)el).SetMaterial(close);
        }

        private void ButtonPressed(MouseEvent e, GuiElement el)
        {
            if (e.leftButtonDown)
            {
                ((Button)el).SetMaterial(closeClicked);
            }
        }

        private void ButtonReleased(MouseEvent e, GuiElement el)
        {
            ((Button)el).SetMaterial(closeHovered);
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
    }
}
