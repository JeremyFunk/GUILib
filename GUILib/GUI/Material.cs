using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GUILib.GUI.Render.Shaders;
using GUILib.GUI.GuiElements;

namespace GUILib.GUI
{
    public enum RenderMode
    {
        Texture, Color, DistanceFieldFonts
    }

    public class GradientData
    {
        public bool left, up, right, down;
        public bool inverseGradient;
        public float gradientFalloff, gradientOpacity, gradientRadius;
        public Vector4 gradientColor;
        public bool usesColor = false;

        public GradientData(float gradientFalloff, float gradientOpacity, float gradientRadius = 0.5f, bool inverseGradient = false, bool left = true, bool up = true, bool right = true, bool down = true)
        {
            this.gradientFalloff = gradientFalloff;
            this.gradientOpacity = gradientOpacity;
            this.gradientRadius = gradientRadius;
            this.inverseGradient = inverseGradient;

            this.left = left;
            this.up = up;
            this.right = right;
            this.down = down;
        }

        public GradientData(Vector4 gradientColor, float gradientFalloff, float gradientOpacity, float gradientRadius = 0.5f, bool inverseGradient = false, bool left = true, bool up = true, bool right = true, bool down = true)
        {
            this.gradientColor = gradientColor;
            this.gradientFalloff = gradientFalloff;
            this.gradientOpacity = gradientOpacity;
            this.gradientRadius = gradientRadius;
            this.inverseGradient = inverseGradient;

            usesColor = true;

            this.left = left;
            this.up = up;
            this.right = right;
            this.down = down;
        }

        public GradientData(GradientData g)
        {
            this.left = g.left;
            this.up = g.up;
            this.right = g.right;
            this.down = g.down;
            this.inverseGradient = g.inverseGradient;
            this.gradientFalloff = g.gradientFalloff;
            this.gradientOpacity = g.gradientOpacity;
            this.gradientRadius = g.gradientRadius;
            this.gradientColor = g.gradientColor;
        }
    }

    public class BorderData
    {
        public Vector4 borderColor;
        public int borderSize;
        public bool roundEdges;
        public float radius;

        public BorderData(Vector4 borderColor, int borderSize = 2, bool roundEdges = false, float radius = 0)
        {
            this.borderColor = borderColor;
            this.borderSize = borderSize;
            this.roundEdges = roundEdges;
            this.radius = radius;
        }

        public BorderData(BorderData b)
        {
            this.borderColor = b.borderColor;
            this.borderSize = b.borderSize;
            this.roundEdges = b.roundEdges;
            this.radius = b.radius;
        }
    }

    public class Material
    {
        public Vector4 color;
        public Texture texture;
        public RenderMode renderMode;
        public bool usesBorder, usesGradient;
        public GradientData gradient;
        public BorderData border;

        public Material(Material m)
        {
            color = m.color;
            renderMode = m.renderMode;
            usesBorder = m.usesBorder;
            usesGradient = m.usesGradient;
            if(m.gradient != null)
                gradient = new GradientData(m.gradient);
            if(m.border != null)
                border = new BorderData(m.border);
            texture = m.texture;
        }

        public Material(Vector4 color, BorderData border = null, GradientData gradient = null)
        {
            usesBorder = border != null;
            usesGradient = gradient != null;
            this.border = border;
            this.gradient = gradient;

            this.color = color;
            this.renderMode = RenderMode.Color;
        }

        public Material(Texture texture, BorderData border = null, GradientData gradient = null)
        {
            usesBorder = border != null;
            usesGradient = gradient != null;
            this.border = border;
            this.gradient = gradient;
            color = new Vector4(1);

            this.texture = texture;
            this.renderMode = RenderMode.Texture;
        }

        public int GetBorderSize()
        {
            return border == null ? 0 : border.borderSize;
        }

        public void SetColor(Vector4 color)
        {
            this.color = color;
        }

        public void PrepareRender(DefaultShader shader, float opacity, Vector2 offset, Vector2 scale)
        {
            Vector4 color = new Vector4(this.color.X, this.color.Y, this.color.Z, this.color.W);

            shader.SetTransform(offset, scale);
            shader.SetBorderVisibility(usesBorder);
            shader.SetOpacity(opacity);

            if (usesBorder)
            {
                shader.SetBorderColor(border.borderColor);
                shader.SetUseRoundEdges(border.roundEdges);
                shader.SetBorderWidth(border.borderSize, scale);
                shader.SetEdgeRadius(border.radius);
            }
            else
            {
                shader.SetBorderColor(new Vector4());
                shader.SetUseRoundEdges(false);
                shader.SetBorderWidth(0, scale);
                shader.SetEdgeRadius(0);
            }

            shader.SetGradient(usesGradient);

            if (usesGradient)
            {
                shader.SetGradientFalloff(gradient.gradientFalloff);
                shader.SetGradientOpacity(gradient.gradientOpacity);
                shader.SetGradientRadius(gradient.gradientRadius * (scale.X / scale.Y));
                shader.SetGradientDirection(gradient.up, gradient.down, gradient.left, gradient.right);
                shader.SetGradientInverse(gradient.inverseGradient);
                shader.SetUseGradientColor(gradient.usesColor);
                if (gradient.usesColor)
                    shader.SetGradientColor(gradient.gradientColor);
            }else
            {
                shader.SetGradientFalloff(0);
                shader.SetGradientOpacity(0);
                shader.SetGradientRadius(0);
                shader.SetGradientDirection(false, false, false, false);
                shader.SetGradientInverse(false);
            }



            shader.SetRenderMode(renderMode);
            if (renderMode == RenderMode.Color)
            {
                shader.SetFillColor(color);
            }
            else if (renderMode == RenderMode.Texture)
            {
                shader.SetFillColor(color);

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
