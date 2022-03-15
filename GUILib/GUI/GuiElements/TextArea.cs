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
    public class TextArea : GuiElement
    {
        private Text text;

        private int lastCurWidth = 0;

        public TextArea(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, string text = "", float fontSize = -1, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, bool visible = true, int edgeSize = -1) : base(width, height, x, y, visible, zIndex)
        {
            if (fillMaterial == null)
                fillMaterial = Theme.defaultTheme.GetTextAreaMaterial();
            if (fontSize < 0)
                fontSize = 1.2f;

            Quad quad = new Quad(0, 0, width, height, fillMaterial);

            AddChild(quad);

            this.text = new Text(6, 0, text, fontSize, null, 0, curWidth - 12);

            lastCurWidth = curWidth;

            this.text.yConstraints.Add(new MarginConstraint(3));
            AddChild(this.text);
        }

        public void SetText(string text)
        {
            this.text.SetText(text);
        }

        public override void UpdateElement(float delta)
        {
            if(lastCurWidth != curWidth)
            {
                this.text.SetMaxSize(curWidth - 12);
                lastCurWidth = curWidth;
            }
        }
    }
}
