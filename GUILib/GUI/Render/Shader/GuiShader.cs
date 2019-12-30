using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Util;
using OpenTK;

namespace GUILib.GUI.Render.Shader
{
    class GuiShader : ShaderProgram
    {
        /*
         * 
        uniform int u_renderMode;

        uniform sampler2D u_fillTexture;
        uniform vec4 u_fillColor;

        uniform sampler2D u_borderTexture;
        uniform vec4 u_borderColor;

        uniform vec2 u_absScale;
        uniform vec2 u_normScale;
        uniform vec2 u_windowScale;

        uniform bool u_roundEdges;
        uniform float u_edgeRadius;

        uniform bool u_gradient;
        uniform float u_gradientFalloff;

        uniform bool u_border;
        uniform float u_borderWidth;
         */
        private static readonly string
            renderModeUniform = "u_renderMode",

            fillTextureUniform = "u_fillTexture",
            fillColorUniform = "u_fillColor",

            borderColorUniform = "u_borderColor",

            normalizedScaleUniform = "u_normScale",
            windowScaleUniform = "u_windowScale",

            roundEdgesUniform = "u_roundEdges",
            edgeRadiusUniform = "u_edgeRadius",

            gradientUniform = "u_gradient",
            gradientFalloffUniform = "u_gradientFalloff",
            gradientRadiusUniform = "u_gradientRadius",
            gradientOpacityUniform = "u_gradientOpacity",
            inverseGradientUniform = "inverseGradient",
            upUniform = "u_up",
            downUniform = "u_down",
            leftUniform = "u_left",
            rightUniform = "u_right",

            borderUniform = "u_border",
            borderWidthUniform = "u_borderWidth",

            positionOffsetUniform = "u_positionOffset",

            opacityUniform = "u_opacity",

            widthUniform = "width";




        private int quadVao;

        public GuiShader(int quadVao) : base(@"Shaders\gui.vs", @"Shaders\gui.fs") { this.quadVao = quadVao; }

        public override void LoadUniforms()
        {
            CreateUniform(renderModeUniform);

            CreateUniform(fillTextureUniform);
            CreateUniform(fillColorUniform);
            
            CreateUniform(borderColorUniform);

            CreateUniform(normalizedScaleUniform);
            CreateUniform(windowScaleUniform);

            CreateUniform(roundEdgesUniform);
            CreateUniform(edgeRadiusUniform);

            CreateUniform(gradientUniform);
            CreateUniform(gradientFalloffUniform);
            CreateUniform(gradientRadiusUniform);
            CreateUniform(gradientOpacityUniform); 
            CreateUniform(inverseGradientUniform); 
            CreateUniform(upUniform);
            CreateUniform(downUniform);
            CreateUniform(leftUniform);
            CreateUniform(rightUniform);
            CreateUniform(widthUniform);

            CreateUniform(borderUniform);
            CreateUniform(borderWidthUniform);

            CreateUniform(positionOffsetUniform);

            CreateUniform(opacityUniform);


            Start();
            SetUniform(fillTextureUniform, 0);
            Stop();
        }

        internal void SetOpacity(float opacity)
        {
            SetUniform(opacityUniform, opacity);
        }

        public void SetFontWidth(float width)
        {
            SetUniform(widthUniform, width);
        }

        internal void SetUseRoundEdges(bool roundEdges)
        {
            SetUniform(roundEdgesUniform, roundEdges);
        }

        internal void SetEdgeRadius(float radius)
        {
            SetUniform(edgeRadiusUniform, radius);
        }

        internal void SetBorderVisibility(bool border)
        {
            SetUniform(borderUniform, border);
        }

        internal void SetGradient(bool gradient)
        {
            SetUniform(gradientUniform, gradient);
        }

        internal void SetGradientFalloff(float falloff)
        {
            SetUniform(gradientFalloffUniform, falloff);
        }

        internal void SetGradientRadius(float radius)
        {
            SetUniform(gradientRadiusUniform, radius);
        }

        internal void SetGradientInverse(bool inverse)
        {
            SetUniform(inverseGradientUniform, inverse);
        }

        internal void SetGradientDirection(bool up, bool down, bool left, bool right)
        {
            SetUniform(upUniform, up);
            SetUniform(downUniform, down);
            SetUniform(leftUniform, left);
            SetUniform(rightUniform, right);
        }

        //Influence of Gradient: The higher the opacity, the lower the alpha value in the center of the gradient effect.
        internal void SetGradientOpacity(float opacity)
        {
            SetUniform(gradientOpacityUniform, opacity);
        }

        internal void SetBorderWidth(int edgeWidth, Vector2 scale)
        {
            float scaler;
            if (scale[0] > scale[1])
            {
                scaler = scale[1];
            }
            else
            {
                scaler = scale[0];
            }
            SetUniform(borderWidthUniform, edgeWidth/scaler);
        }

        public void SetRenderMode(RenderMode renderMode)
        {
            if (renderMode == RenderMode.Texture)
            {
                SetUniform(renderModeUniform, 0);
            }
            else if (renderMode == RenderMode.Color)
            {
                SetUniform(renderModeUniform, 1);
            }
            else if (renderMode == RenderMode.DistanceFieldFonts)
            {
                SetUniform(renderModeUniform, 2);
            }
        }

        public void SetFillColor(Vector4 color)
        {
            SetUniform(fillColorUniform, color);
        }

        public void SetBorderColor(Vector4 color)
        {
            SetUniform(borderColorUniform, color);
        }
        
        public void SetTransform(Vector2 offset, Vector2 scale)
        {
            
            Vector2 normScale = NormalizeScale(scale);

            Vector2 winScale = WindowScale(scale);
            offset = new Vector2(((offset.X / GameSettings.Width) * 2) - 1 + winScale.X, ((offset.Y / GameSettings.Height) * 2) - 1 + winScale.Y);
            SetUniform(positionOffsetUniform, offset);
            SetUniform(windowScaleUniform, winScale);
            SetUniform(normalizedScaleUniform, normScale);

        }
        
       
        private int currentVaoID = 0;
        public void SetRenderVAO(int vaoID)
        {
            if (vaoID != currentVaoID)
            {
                currentVaoID = vaoID;
                GL.BindVertexArray(vaoID);
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
            }
        }

        public void ResetVAO()
        {
            if (quadVao != currentVaoID)
            {
                currentVaoID = quadVao;
                GL.BindVertexArray(quadVao);
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
            }
        }

        private Vector2 NormalizeScale(Vector2 scale)
        {
            float newScaleX;

            float newScaleY;
            if (scale.Y < scale.X)
            {
                newScaleX = scale.X / scale.Y;
                newScaleY = 1;
            }
            else
            {
                newScaleX = 1;
                newScaleY = scale.Y / scale.X;
            }
            return new Vector2(newScaleX, newScaleY);
        }

        private Vector2 WindowScale(Vector2 scale)
        {
            return new Vector2(scale.X / GameSettings.Width, scale.Y / GameSettings.Height);
        }

    }
}
