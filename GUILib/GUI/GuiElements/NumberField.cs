﻿using OpenTK;
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
    public class NumberField : GuiElement
    {
        private Quad quad;
        private Text defaultText, textElement, textElementCursor;
        private float timer;

        private bool selected, renderCursor;

        public Action<int> numberChangeEvent;

        public NumberField(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, int number, Material fillMaterial = null, float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
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
                    }
                }

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
                }else if (textElement.GetText() == "-")
                {
                    textElement.SetText("");
                    defaultText.visible = true;
                }
            }

            if(numberChangeEvent != null)
            {
                int number = 0;
                if (!int.TryParse(textElement.GetText(), out number))
                    number = 0;

                numberChangeEvent.Invoke(number);
            }
        }

        public int GetNumber()
        {
            int number = 0;
            if (!int.TryParse(textElement.GetText(), out number))
                number = 0;

            return number;
        }

        public void SetNumber(int number, bool triggerChangeEvent = false)
        {
            textElement.SetText(number + "");
            textElementCursor.SetText(textElement.GetText() + "|");

            defaultText.visible = false;
            textElement.visible = true;
            textElementCursor.visible = false;

            if (triggerChangeEvent)
                numberChangeEvent?.Invoke(number);
        }
    }
}
