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
using OpenTK.Input;

namespace GUILib.GUI.GuiElements
{
    class TextField : GuiElement
    {
        private Quad quad;
        private Text defaultText, textElement, textElementCursor;
        private float timer;

        private bool selected, renderCursor;

        public TextField(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string text, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {

            quad = new Quad(0, 0, width, height, fillMaterial == null ? Theme.defaultTheme.GetFieldFillMaterial() : fillMaterial);
            quad.generalConstraint = new FillConstraintGeneral();

            quad.mouseButtonReleasedEvent = Click;
            quad.mouseButtonReleasedMissedEvent = ClickMissed;

            AddChild(quad);

            this.defaultText = new Text(5, 0, text, 0.8f);
            this.defaultText.color = new Vector4(1, 1, 1, 0.7f);
            this.defaultText.yConstraints.Add(new CenterConstraint());

            textElement = new Text(5, 0, "", 0.8f);
            textElement.yConstraints.Add(new CenterConstraint());

            textElementCursor = new Text(5, 0, "|", 0.8f);
            textElementCursor.yConstraints.Add(new CenterConstraint());
            textElementCursor.visible = false;

            AddChild(this.defaultText);
            AddChild(textElement);
            AddChild(textElementCursor);
        }

        public override void UpdateElement(float delta)
        {
            if (selected)
            {
                timer += delta;

                if (timer >= Theme.defaultTheme.GetCursorTickRate())
                {
                    renderCursor = !renderCursor;
                    timer -= Theme.defaultTheme.GetCursorTickRate();

                    textElement.visible = !renderCursor;
                    textElementCursor.visible = renderCursor;
                }
            }
        }

        public override void KeyEventElement(KeyEvent e)
        {
            if (selected)
            {
                if (e.pressed.Contains(Key.BackSpace))
                    if (textElement.GetText().Length > 0)
                        textElement.SetText(textElement.GetText().Remove(textElement.GetText().Length - 1, 1));

                foreach (Key key in e.pressed)
                {
                    if (e.keyCharsPressed.ContainsKey(key))
                        textElement.SetText(textElement.GetText() + e.keyCharsPressed[key]);
                }

                textElementCursor.SetText(textElement.GetText() + "|");
            }
        }

        private void Click(MouseEvent e, GuiElement el)
        {
            if (e.leftButtonDown)
            {
                selected = true;
                defaultText.visible = false;
            }
        }

        private void ClickMissed(MouseEvent e, GuiElement el)
        {
            if (e.leftButtonDown)
            {
                selected = false;

                textElementCursor.visible = false;
                textElement.visible = true;
                timer = 0;
                renderCursor = false;

                if (textElement.GetText() == "")
                {
                    defaultText.visible = true;
                }
            }
        }
    }
}
