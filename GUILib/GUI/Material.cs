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
        private Vector4 color, borderColor;
        private RenderMode renderMode;
        private Texture texture;
        private bool usesBorder, roundEdges, usesGradient;
        private int borderSize;
        private float radius;
        private float gradientFalloff, gradientOpacity, gradientRadius;

        public Material(Vector4 color)
        {
            usesBorder = false;
            usesGradient = false;

            this.color = color;
            this.renderMode = RenderMode.Color;
        }

        public Material(Texture texture)
        {
            usesBorder = false;
            usesGradient = false;

            this.texture = texture;
            this.renderMode = RenderMode.Texture;
        }

        public Material(Vector4 color, Vector4 borderColor, int borderSize = 0, bool roundEdges = false, float radius = 0.3f, bool usesGradient = false, float gradientFalloff = 0.6f, float gradientOpacity = 0.3f, float gradientRadius = 0.3f)
        {
            this.color = color;
            this.borderSize = borderSize;
            this.roundEdges = roundEdges;
            this.radius = radius;
            this.usesGradient = usesGradient;
            this.gradientFalloff = gradientFalloff;
            this.gradientOpacity = gradientOpacity;
            this.gradientRadius = gradientRadius;
            this.usesBorder = true;
            this.borderColor = borderColor;

            this.renderMode = RenderMode.Color;
        }

        public Material(Texture texture, Vector4 borderColor, int borderSize = 0, bool roundEdges = false, float radius = 0.3f, bool usesGradient = false, float gradientFalloff = 0.6f, float gradientOpacity = 0.3f, float gradientRadius = 0.3f)
        {
            this.texture = texture;
            this.borderSize = borderSize;
            this.roundEdges = roundEdges;
            this.radius = radius;
            this.usesGradient = usesGradient;
            this.gradientFalloff = gradientFalloff;
            this.gradientOpacity = gradientOpacity;
            this.gradientRadius = gradientRadius;
            this.usesBorder = true;
            this.borderColor = borderColor;

            this.renderMode = RenderMode.Texture;
        }

        public int GetBorderSize()
        {
            return borderSize;
        }

        public void PrepareRender(GuiShader shader, float opacity, Vector2 offset, Vector2 scale)
        {
            Vector4 color = new Vector4(this.color.X, this.color.Y, this.color.Z, this.color.W * opacity);

            shader.SetTransform(offset, scale);

            shader.SetBorderVisibility(usesBorder);

            shader.SetBorderColor(borderColor);
            shader.SetUseRoundEdges(roundEdges);
            shader.SetBorderWidth(borderSize, scale);
            shader.SetEdgeRadius(radius);

            shader.SetGradient(usesGradient);
            shader.SetGradientFalloff(gradientFalloff);
            shader.SetGradientOpacity(gradientOpacity);
            shader.SetGradientRadius(gradientRadius);



            shader.SetRenderMode(renderMode);
            if (renderMode == RenderMode.Color)
            {
                shader.SetFillColor(color);
            }
            else if (renderMode == RenderMode.Texture)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture.textureID);
            }
            else if (renderMode == RenderMode.DistanceFieldFonts)
            {
                shader.SetFillColor(color);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture.textureID);
            }
        }
    }
}
