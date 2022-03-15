using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.GUI.GuiElements;
using GUILib.GUI.Render.Shaders;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GUILib.GUI.Render.Fonts.Data
{
    public class Font
    {
        public static Font defaultFont = FontFileParser.LoadFont("Candara");

        Dictionary<int, Character> characters = new Dictionary<int, Character>();
        Texture texture;

        public float xAdvance = 0.8f;

        public Font(Dictionary<int, Character> characters, Texture texture)
        {
            this.characters = characters;
            this.texture = texture;
        }

        public Character GetCharacter(char c)
        {
            if(characters.ContainsKey(c))
                return characters[c];
            return null;
        }

        public void Render(string text, DefaultShader shader, Vector4 color, Text textElement)
        {
            shader.SetRenderMode(RenderMode.DistanceFieldFonts);
            shader.SetFillColor(color);
            shader.SetRenderVAO(textElement.data.vaoID);

            GL.BindTexture(TextureTarget.Texture2D, texture.textureID);

            GL.DrawArrays(PrimitiveType.Quads, 0, textElement.data.vertexCount);
        }

        private TextData CreateText(string text, Text textElement, float maxSize, float fontSize, float xAdvanceMult)
        {
            float width;
            float height;
            TextData data = FontMeshCreator.CreateMesh(this, text, maxSize, fontSize, out width, out height, xAdvanceMult);

            textElement.SetWidth((int)Math.Round(width));
            textElement.SetHeight((int)Math.Round(height));

            return data;
        }

        public void Reconstruct(string text, Text textElement, float maxSize, float fontSize, float xAdvanceMult = 1f)
        {
            if (textElement.data != null)
                GL.DeleteVertexArray(textElement.data.vaoID);

            TextData data = CreateText(text, textElement, maxSize, fontSize, xAdvanceMult);
            textElement.data = data;
        }
    }
}
