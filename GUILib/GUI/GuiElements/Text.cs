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
using GUILib.GUI.Render.Fonts.Data;
using GUILib.GUI.Constraints;
using GUILib.GUI.PixelConstraints;

namespace GUILib.GUI.GuiElements
{
    public class Text : GuiElement
    {
        private string text;
        private Font font;
        public Vector4 color;
        private float fontSize;
        public TextData data;
        private float maxSize;
        public float fontWidth = 0.42f, fontEdge = 0.19f;
        private float xAdvanceMult = 1;

        public Text(APixelConstraint x, APixelConstraint y, string text, float fontSize, Font font = null, float zIndex = 0, float maxSize = 100000, bool visible = true) : base(0, 0, x, y, visible, zIndex)
        {
            if (font == null)
                font = Font.defaultFont;

            this.text = text;
            this.font = font;
            this.fontSize = fontSize;
            this.color = new Vector4(1);
            this.maxSize = maxSize;

            if(maxSize != float.MaxValue)
                font.Reconstruct(text, this, maxSize * 2, fontSize);
            else
                font.Reconstruct(text, this, maxSize, fontSize);
        }

        protected override void RenderElement(DefaultShader shader, Vector2 trans, Vector2 scale, float opacity)
        {
            shader.ResetVAO();

            shader.SetTransform(trans, new Vector2(1f));
            shader.SetFontWidth(fontWidth);
            shader.SetFontEdge(fontEdge);
            font.Render(text, shader, color, this);
        }
        public float GetFontSize()
        {
            return fontSize;
        }
        public void SetFontSize(float fontSize)
        {
            this.fontSize = fontSize;
            SetText(text);
        }
        public void SetText(string text)
        {
            if (maxSize != float.MaxValue)
                font.Reconstruct(text, this, maxSize * 2, fontSize, xAdvanceMult);
            else
                font.Reconstruct(text, this, maxSize, fontSize, xAdvanceMult);

            this.text = text;
        }
        public void SetCharacterSpaceMultiplyer(float xAdvanceMult)
        {
            this.xAdvanceMult = xAdvanceMult;
            if (maxSize != float.MaxValue)
                font.Reconstruct(text, this, maxSize * 2, fontSize, xAdvanceMult);
            else
                font.Reconstruct(text, this, maxSize, fontSize, xAdvanceMult);
        }
        public float GetCharacterSpaceMultiplyer()
        {
            return xAdvanceMult;
        }

        public string GetText()
        {
            return text;
        }

        public void SetMaxSize(float width)
        {
            this.maxSize = width;
            font.Reconstruct(text, this, maxSize * 2, fontSize, xAdvanceMult);
        }
        public float GetMaxSize()
        {
            return maxSize;
        }
    }
}
