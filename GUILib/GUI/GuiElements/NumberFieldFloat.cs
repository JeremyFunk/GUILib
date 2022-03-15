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
using OpenTK.Input;

namespace GUILib.GUI.GuiElements
{
    public class NumberFieldFloat : GuiElement
    {
        private Quad quad;
        private Text defaultText, textElement, textElementCursor;
        private float timer;

        private bool selected, renderCursor;

        public Action<float> numberChangeEvent;

        private float number;

        public NumberFieldFloat(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, float number, Material fillMaterial = null, float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.number = number;

            quad = new Quad(0, 0, width, height, fillMaterial == null ? Theme.defaultTheme.GetFieldMaterial() : fillMaterial);
            quad.generalConstraint = new FillConstraintGeneral();

            quad.mouseButtonReleasedEvent = Click;
            quad.mouseButtonReleasedMissedEvent = ClickMissed;

            AddChild(quad);

            this.defaultText = new Text(5, 0, number + "", 0.8f);
            this.defaultText.color = new Vector4(1, 1, 1, 0.7f);
            //this.defaultText.yConstraints.Add(new CenterConstraint());

            textElement = new Text(5, 0, number + "", 0.8f);
            //textElement.yConstraints.Add(new CenterConstraint());
            textElement.visible = false;

            textElementCursor = new Text(5, 0, number + "|", 0.8f);
            //textElementCursor.yConstraints.Add(new CenterConstraint());
            textElementCursor.visible = false;

            defaultText.xConstraints.Add(new CenterConstraint());
            textElement.xConstraints.Add(new CenterConstraint());
            textElementCursor.xConstraints.Add(new CenterConstraint());

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
                    {
                        if (char.IsDigit(e.keyCharsPressed[key]))
                            textElement.SetText(textElement.GetText() + e.keyCharsPressed[key]);
                        
                    }else if (key == Key.Minus || key == Key.KeypadMinus)
                    {
                        if (textElement.GetText().Contains('-'))
                        {
                            textElement.SetText(textElement.GetText().Replace("-", ""));
                        }
                        else
                        {
                            textElement.SetText("-" + textElement.GetText());
                        }
                    }else if(key == Key.Period || key == Key.KeypadPeriod || key == Key.Comma)
                    {
                        if (!textElement.GetText().Contains("."))
                        {
                            textElement.SetText(textElement.GetText() + ".");
                        }
                    }
                }

                number = GetNumber();

                textElementCursor.SetText(textElement.GetText() + "|");
            }
        }

        private void Click(MouseEvent e, GuiElement el)
        {
            if (e.leftMouseButtonType == MouseButtonType.Released)
            {
                selected = true;
                defaultText.visible = false;
            }
        }

        private void ClickMissed(MouseEvent e, GuiElement el)
        {
            if (selected)
            {
                if (e.leftMouseButtonType == MouseButtonType.Released)
                {
                    selected = false;

                    textElementCursor.visible = false;
                    textElement.visible = true;
                    timer = 0;
                    renderCursor = false;

                    if (textElement.GetText() == "")
                    {
                        defaultText.visible = true;
                        number = 0;
                    }
                    else if (textElement.GetText() == "-" || textElement.GetText() == ".")
                    {
                        textElement.SetText("");
                        defaultText.visible = true;
                    }
                }

                number = GetNumber();
                numberChangeEvent?.Invoke(number);
            }
        }

        public float GetNumber()
        {
            if (textElement.GetText() == "" || textElement.GetText() == "-" || textElement.GetText() == ".")
            {
                return 0;
            }

            try
            {
                return float.Parse(textElement.GetText(), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return 0;
            }
        }

        public void SetNumber(float number, bool triggerChangeEvent = false)
        {
            textElement.SetText(number.ToString(System.Globalization.CultureInfo.InvariantCulture));
            textElementCursor.SetText(textElement.GetText() + "|");

            defaultText.visible = false;
            textElement.visible = true;
            textElementCursor.visible = false;

            if (triggerChangeEvent)
                numberChangeEvent?.Invoke(number);
        }
    }
}
