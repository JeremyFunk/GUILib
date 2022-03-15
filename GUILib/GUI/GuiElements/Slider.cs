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
using GUILib.Logger;

namespace GUILib.GUI.GuiElements
{
    public class Slider : GuiElement
    {
        private Quad quad;
        private float number = 0, lowerValue, higherValue;
        private bool mouseInfo;

        public int decimalPlaces = 3;

        public Action<float> numberChangedEvent;

        public Slider(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, float lowerValue, float higherValue, float startValue, Material material = null, Material left = null, Material right = null, bool mouseInfo = true, float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.mouseInfo = mouseInfo;
            this.lowerValue = lowerValue;
            this.higherValue = higherValue;
            number = startValue;


            curMaterial = material == null ? Theme.defaultTheme.GetSliderMaterial() : material;

            Quad centerQuad = new Quad(0, 0, 1f, 4, curMaterial);
            centerQuad.yConstraints.Add(new CenterConstraint());

            Quad leftQuad = new Quad(0, 0, 4, 1f, left == null ? curMaterial : left);
            leftQuad.yConstraints.Add(new CenterConstraint());

            Quad rightQuad = new Quad(0, 0, 4, 1f, right == null ? curMaterial : right);
            rightQuad.yConstraints.Add(new CenterConstraint());
            rightQuad.xConstraints.Add(new MarginConstraint(0));

            float resultFactor = number == lowerValue ? 0 : (number - lowerValue) / (higherValue - lowerValue);

            quad = new Quad(resultFactor, 0, 4, 1f, Theme.defaultTheme.GetSliderQuadMaterial());

            AddChild(centerQuad);
            AddChild(leftQuad);
            AddChild(rightQuad);
            AddChild(quad);
        }

        public override void MouseEventElement(MouseEvent e)
        {
            if (e.hit)
            {
                if(mouseInfo)
                    MouseInfo.SetMouseInfo(Math.Round(number) + "");
                if (e.leftMouseButtonType != MouseButtonType.None)
                {
                    if(e.leftMouseButtonType == MouseButtonType.Pressed)
                    {
                        if (e.mousePositionLocal.X > curWidth - 3)
                            return;

                        quad.SetX((int)e.mousePositionLocal.X);

                        number = lowerValue + (higherValue * (e.mousePositionLocal.X / (curWidth - 3)));

                        numberChangedEvent?.Invoke(GetValue());
                    }
                }
            }
        }

        public float GetValue()
        {
            return (float)Math.Round(number, decimalPlaces);
        }

        public void SetValue(float val)
        {
            float resultFactor = val <= lowerValue ? 0f : val >= higherValue ? 1f : (val - lowerValue) / (higherValue - lowerValue);
            quad.SetX(resultFactor);
        }
    }
}
