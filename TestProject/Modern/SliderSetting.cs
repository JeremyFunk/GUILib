using OpenTK;
using GUILib.GUI.Render.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Events;
using GUILib.GUI.GuiElements;
using GUILib.GUI;
using GUILib.GUI.Animations;
using GUILib.GUI.Constraints;
using GUILib.GUI.PixelConstraints;

namespace GUILib.Modern
{
    class SliderSetting : GuiElement
    {
        private Quad quad;
        private Text text;
        private LeftRightTextSlider slider;

        public SliderSetting(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string text, Material background, Material backgroundHover, int yOffset = 0, int underlineHeight = 2, float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            quad = new Quad(0, 0, width, height, background);
            quad.startHoverEvent = StartHover;
            quad.endHoverEvent = EndHover;
            quad.defaultMaterial = background;
            quad.hoverMaterial = backgroundHover;
            AddChild(quad);

            this.text = new Text(10, 0, text, 0.8f);
            this.text.color = new Vector4(0.6f, 0.6f, 0.6f, 1f);
            this.text.yConstraints.Add(new CenterConstraint());

            List<string> options = new List<string>();
            options.Add("Low");
            options.Add("Medium");
            options.Add("High");
            options.Add("Very high");
            options.Add("Ultra");

            slider = new LeftRightTextSlider(5, 0, 300, curHeight - 10, options, -1, new Material(new Vector4(0)), new Material(new Vector4(0)));
            slider.xConstraints.Add(new MarginConstraint(30));
            slider.yConstraints.Add(new CenterConstraint());
            AddChild(slider);
            AddChild(this.text);
        }

        private void StartHover(MouseEvent e, GuiElement el)
        {
            quad.SetMaterial(quad.hoverMaterial);
        }
        private void EndHover(MouseEvent e, GuiElement el)
        {
            quad.SetMaterial(quad.defaultMaterial);
        }
    }
}
