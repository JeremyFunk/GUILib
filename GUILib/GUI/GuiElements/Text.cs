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

namespace GUILib.GUI.GuiElements
{
    class Text : GuiElement
    {
        public string text;
        public Font font;
        public Vector4 color;
        public float fontSize;
        public TextData data;

        public Text(float x, float y, string text, float fontSize, Font font = null, float zIndex = 0, bool visible = true) : base(0, 0, x, y, visible, zIndex)
        {
            if (font == null)
                font = Font.defaultFont;

            this.text = text;
            this.font = font;
            this.fontSize = fontSize;
            this.color = new Vector4(1);
            font.Reconstruct(text, this);
        }

        public override void MouseEventElement(MouseEvent events)
        {

        }

        public override void KeyEvent(KeyEvent events)
        {
        }

        protected override void RenderElement(GuiShader shader, Vector2 trans, Vector2 scale, float opacity)
        {
            shader.ResetVAO();

            shader.SetTransform(trans, new Vector2(fontSize));
            font.Render(text, shader, color, this);
        }

        public override void UpdateElement(float delta)
        {
        }

        public void RunAnimation(string animationName)
        {
        }
    }
}
