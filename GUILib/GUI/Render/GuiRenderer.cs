using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiLib.GUI.Render.Shader;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GuiLib.Util;
using GuiLib.GUI.GuiElements;

namespace GuiLib.GUI.Render
{
    class GuiRenderer
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

            vaoID = OpenGLLoader.LoadObject(vertices, textureCoords);

            shader = new GuiShader(vaoID);
        }

        public void Render(GuiScene scene)
        {
            shader.Start();

            GL.BindVertexArray(vaoID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            foreach (GuiElement parent in scene.parents)
            {
                parent.Render(shader, new Vector2());
            }

            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);

            shader.Stop();
        }

        public GuiShader GetShader()
        {
            return shader;
        }

        internal void CleanUp()
        {
            shader.CleanUp();
        }

        internal void PrepareRender()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0, 0, 0, 1f);
            GL.Disable(EnableCap.DepthTest);
        }
    }
}
