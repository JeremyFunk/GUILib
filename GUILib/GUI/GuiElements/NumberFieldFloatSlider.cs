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
    public class NumberFieldFloatSlider : GuiElement
    {
        private const float activationTime = 0.3f;

        public NumberFieldFloat field;
        private float numberChange;
        private float activationCounter;
        private bool leftActivated = false, rightActivated = false;

        public int roundToDigits = 3;

        public NumberFieldFloatSlider(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, float number, float numberChange = 1, Material fillMaterial = null, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.numberChange = numberChange;

            field = new NumberFieldFloat(0, 0, 1f, 1f, number, fillMaterial);
            AddChild(field);

            Quad left = new Quad(5, 5, 10, 10, Theme.defaultTheme.GetLeftArrowMaterial(), 1);
            left.yConstraints.Add(new CenterConstraint());
            left.canClickThrough = false;
            left.mouseButtonDownEvent = LeftDown;
            left.mouseButtonPressedEvent = LeftPressed;
            left.mouseButtonReleasedEvent = LeftReleased;
            AddChild(left);

            Quad right = new Quad(0, 5, 10, 10, Theme.defaultTheme.GetRightArrowMaterial(), 1);
            right.xConstraints.Add(new MarginConstraint(5));
            right.yConstraints.Add(new CenterConstraint());
            right.mouseButtonDownEvent = RightDown;
            right.mouseButtonPressedEvent = RightPressed;
            right.mouseButtonReleasedEvent = RightReleased;
            right.canClickThrough = false;
            AddChild(right);
        }

        public override void UpdateElement(float delta)
        {
            if (leftActivated)
            {
                activationCounter += delta;
                leftActivated = false;

                if(activationCounter >= activationTime)
                {
                    field.SetNumber((float)Math.Round(field.GetNumber() - numberChange * delta, roundToDigits), true);
                }
            }else if (rightActivated)
            {
                activationCounter += delta;
                rightActivated = false;

                if (activationCounter >= activationTime)
                {
                    field.SetNumber((float)Math.Round(field.GetNumber() + numberChange * delta, roundToDigits), true);
                }
            }
            else
            {
                activationCounter = 0;
            }
        }

        private void LeftDown(MouseEvent e, GuiElement el)
        {
            leftActivated = true;
        }

        private void RightDown(MouseEvent e, GuiElement el)
        {
            rightActivated = true;
        }

        private void LeftPressed(MouseEvent e, GuiElement el)
        {
            leftActivated = true;
        }

        private void RightPressed(MouseEvent e, GuiElement el)
        {
            rightActivated = true;
        }

        private void LeftReleased(MouseEvent e, GuiElement el)
        {
            if(activationCounter < activationTime)
                field.SetNumber(field.GetNumber() - numberChange, true);
        }

        private void RightReleased(MouseEvent e, GuiElement el)
        {
            if (activationCounter < activationTime)
                field.SetNumber(field.GetNumber() + numberChange, true);
        }
    }
}
