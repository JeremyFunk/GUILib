using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.GUI.Render.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GUILib.Util;
using GUILib.GUI.GuiElements;
using GUILib.Logger;

namespace GUILib.GUI.Render
{
    public class GuiRenderer : DefaultRenderer
    {
        private GuiShader shader;
        private int vaoID;

        public GuiRenderer()
        {
            float[] vertices = new float[]{
                 -1,  1, 0,
                  1,  1, 0,
                  1, -1, 0,
                 -1, -1, 0
            };

            float[] textureCoords = new float[]
            {
                0, 0,
                1, 0,
                1, 1,
                0, 1
            };

            ALogger.defaultLogger.Log("Loading quad vertices.", LogLevel.Info);
            vaoID = OpenGLLoader.LoadObject(vertices, textureCoords);

            shader = new GuiShader(vaoID);
        }

        public override void Render(GuiScene scene)
        {
            shader.Start();

            GL.BindVertexArray(vaoID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            foreach (GuiElement parent in scene.GetParents())
            {
                if (!parent.visible)
                    continue;
                parent.Render(shader, new Vector2(), 1f);
            }

            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);

            shader.Stop();
        }

        public override DefaultShader GetShader()
        {
            return shader;
        }

        public override void CleanUp()
        {
            shader.CleanUp();
        }

        public override void PrepareRender()
        {
            GL.Disable(EnableCap.ScissorTest);
            GL.Disable(EnableCap.StencilTest);
            GL.StencilMask(~0);
            GL.ClearStencil(0);
            GL.ClearColor(0, 0, 0, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.StencilBufferBit);
        }
    }
}
