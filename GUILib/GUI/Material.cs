using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GUILib.GUI.Render.Shader;
using GUILib.GUI.GuiElements;

namespace GUILib.GUI
{
    enum RenderMode
    {
        Texture, Color, DistanceFieldFonts
    }

    class Material
    {
        private Vector4 color;
        private RenderMode renderMode;
        private Texture texture;

        public Material(Vector4 color)
        {
            this.color = color;
            this.renderMode = RenderMode.Color;
        }

        public Material(Texture texture)
        {
            this.texture = texture;
            this.renderMode = RenderMode.Texture;
        }


        public void PrepareRender(GuiShader shader, float opacity)
        {
            Vector4 color = new Vector4(this.color.X, this.color.Y, this.color.Z, this.color.W * opacity);

            shader.SetRenderMode(renderMode);
            if (renderMode == RenderMode.Color)
            {
                shader.SetColor(color);
            }
            else if (renderMode == RenderMode.Texture)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture.textureID);
            }
            else if (renderMode == RenderMode.DistanceFieldFonts)
            {
                shader.SetColor(color);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture.textureID);
            }
        }
    }
}
