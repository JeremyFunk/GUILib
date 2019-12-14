using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiLib.GUI.Render.Shader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GuiLib.GUI.Render.Fonts.Data
{
    class Font
    {
        Dictionary<int, Character> characters = new Dictionary<int, Character>();
        Texture texture;

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

        /*public void Render(string text, GuiShader shader, Vector4 color, Text textElement)
        {
            shader.SetRenderMode(RenderMode.DistanceFieldFonts);
            shader.SetColor(color);
            shader.SetRenderVAO(textElement.data.vaoID);

            GL.BindTexture(TextureTarget.Texture2D, texture.textureID);

            GL.DrawArrays(PrimitiveType.Quads, 0, textElement.data.vertexCount);
        }

        private TextData CreateText(string text, Text textElement)
        {
            float width;
            float height;
            TextData data = FontMeshCreator.CreateMesh(this, text, out width, out height);

            textElement.width = (int)Math.Round(width * textElement.fontSize);
            textElement.height = (int)Math.Round(height * textElement.fontSize);

            return data;
        }

        internal void Reconstruct(string text, Text textElement)
        {
            if (textElement.data != null)
                GL.DeleteVertexArray(textElement.data.vaoID);

            TextData data = CreateText(text, textElement);
            textElement.data = data;
        }*/
    }
}
