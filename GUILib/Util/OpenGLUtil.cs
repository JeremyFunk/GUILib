using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace GUILib.Util
{
    class OpenGLUtil
    {
        public static void StartStencilDraw()
        {
            GL.Disable(EnableCap.ScissorTest);
            GL.Disable(EnableCap.StencilTest);
            GL.StencilMask(~0);
            GL.ClearStencil(0);
            GL.Enable(EnableCap.StencilTest);
            GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            GL.StencilMask(0xFF);
            GL.ColorMask(false, false, false, false);
        }

        public static void EndStencilDraw()
        {
            GL.ColorMask(true, true, true, true);
            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);
            GL.StencilMask(0x00);
        }


        public static void EndStencil()
        {
            GL.Disable(EnableCap.ScissorTest);
            GL.Disable(EnableCap.StencilTest);
            GL.StencilMask(~0);
            GL.ClearStencil(0);
        }
    }
}
