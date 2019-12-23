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
        private static readonly string renderModeUniform = "u_renderMode", textureUniform = "u_texture", colorUniform = "u_color", positionUniform = "u_positionOffset", scaleUniform = "u_scale", absoluteScaleUniform = "u_absScale",
            roundEdgeUniform = "u_roundEdges", gradientUniform = "u_gradient", edgeWidthUniform = "u_edgeWidth";

        private int quadVao;

        public GuiShader(int quadVao) : base(@"Shaders\gui.vs", @"Shaders\gui.fs") { this.quadVao = quadVao; }

        public override void LoadUniforms()
        {
            CreateUniform(renderModeUniform);
            CreateUniform(textureUniform);
            CreateUniform(colorUniform);
            CreateUniform(positionUniform);
            CreateUniform(scaleUniform);
            CreateUniform(absoluteScaleUniform);
            CreateUniform(roundEdgeUniform);
            CreateUniform(gradientUniform);
            CreateUniform(edgeWidthUniform);

            Start();
            SetUniform(textureUniform, 0);
            Stop();
        }

        internal void SetUseRoundEdges(bool roundEdges)
        {
            SetUniform(roundEdgeUniform, roundEdges);
        }

        internal void SetEdgeWidth(int edgeWidth, Vector2 scale)
        {
            SetUniform(edgeWidthUniform, edgeWidth);
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

        public void SetColor(Vector4 color)
        {
            SetUniform(colorUniform, color);
        }

        public void SetTransform(Vector2 offset, Vector2 scale)
        {
            float absScaleX;

            float absScaleY;
            if (scale.Y < scale.X)
            {
                absScaleX = scale.X / scale.Y;
                absScaleY = 1;
            }
            else
            {
                absScaleX = 1;
                absScaleY = scale.Y / scale.X;
            }
            Vector2 absScale = new Vector2(absScaleX, absScaleY);
            scale = new Vector2(scale.X / GameSettings.Width, scale.Y / GameSettings.Height);
            offset = new Vector2(((offset.X / GameSettings.Width) * 2) - 1 + scale.X, ((offset.Y / GameSettings.Height) * 2) - 1 + scale.Y);
            SetUniform(positionUniform, offset);
            SetUniform(scaleUniform, scale);
            SetUniform(absoluteScaleUniform, absScale);

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
    }
}
