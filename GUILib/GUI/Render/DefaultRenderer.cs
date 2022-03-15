using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.GUI.Render.Shaders;

namespace GUILib.GUI.Render
{
    public abstract class DefaultRenderer
    {
        public abstract void Render(GuiScene scene);
        public abstract DefaultShader GetShader();
        public abstract void CleanUp();
        public abstract void PrepareRender();
    }
}
