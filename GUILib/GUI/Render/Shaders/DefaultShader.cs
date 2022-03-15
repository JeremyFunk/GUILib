using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Render.Shaders
{
    public abstract class DefaultShader : Shader
    {
        public DefaultShader(string vertexShaderPath, string fragmentShaderPath) : base(vertexShaderPath, fragmentShaderPath) { }

        public abstract void SetOpacity(float opacity);

        public abstract void SetFontWidth(float width);
        public abstract void SetFontEdge(float edge);

        public abstract void SetUseRoundEdges(bool roundEdges);

        public abstract void SetEdgeRadius(float radius);

        public abstract void SetBorderVisibility(bool border);
        public abstract void SetGradient(bool gradient);

        public abstract void SetGradientFalloff(float falloff);

        public abstract void SetGradientRadius(float radius);

        public abstract void SetGradientInverse(bool inverse);

        public abstract void SetGradientDirection(bool up, bool down, bool left, bool right);

        //Influence of Gradient: The higher the opacity, the lower the alpha value in the center of the gradient effect.
        public abstract void SetGradientOpacity(float opacity);

        public abstract void SetGradientColor(Vector4 color);

        public abstract void SetUseGradientColor(bool uses);

        public abstract void SetBorderWidth(int edgeWidth, Vector2 scale);

        public abstract void SetRenderMode(RenderMode renderMode);

        public abstract void SetFillColor(Vector4 color);

        public abstract void SetBorderColor(Vector4 color);

        public abstract void SetTransform(Vector2 offset, Vector2 scale);

        public abstract void SetRenderVAO(int vaoID);

        public abstract void SetAbsoluteScale(float scale);
        public abstract void ResetVAO();
    }
}
