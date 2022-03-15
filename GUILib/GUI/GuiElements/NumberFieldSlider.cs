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
    public class NumberFieldSlider : GuiElement
    {
        public NumberField field;
        private int numberChange;
        public NumberFieldSlider(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, int number, int numberChange = 1, Material fillMaterial = null, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.numberChange = numberChange;

            field = new NumberField(0, 0, 1f, 1f, number, fillMaterial);
            AddChild(field);

            Quad left = new Quad(5, 5, 10, 10, Theme.defaultTheme.GetLeftArrowMaterial(), 1);
            left.yConstraints.Add(new CenterConstraint());
            left.canClickThrough = false;
            left.mouseButtonReleasedEvent = Left;
            AddChild(left);

            Quad right = new Quad(0, 5, 10, 10, Theme.defaultTheme.GetRightArrowMaterial(), 1);
            right.xConstraints.Add(new MarginConstraint(5));
            right.yConstraints.Add(new CenterConstraint());
            right.mouseButtonReleasedEvent = Right;
            right.canClickThrough = false;
            AddChild(right);
        }

        private void Left(MouseEvent e, GuiElement el)
        {
            field.SetNumber(field.GetNumber() - numberChange, true);
        }

        private void Right(MouseEvent e, GuiElement el)
        {
            field.SetNumber(field.GetNumber() + numberChange, true);
        }
    }
}
