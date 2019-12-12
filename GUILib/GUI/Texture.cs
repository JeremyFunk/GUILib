using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiLib.Util;

namespace GuiLib.GUI
{
    class Texture
    {
        public int textureID;

        public Texture(string path)
        {
            textureID = OpenGLLoader.LoadTexture(path, true, true);
        }
    }
}
