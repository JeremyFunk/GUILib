using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.Util;

namespace GUILib.GUI
{
    public class Texture
    {
        public int textureID;

        public Texture(string path, bool texturePathInternal = false)
        {
            if(!texturePathInternal)
                textureID = OpenGLLoader.LoadTexture(path, true, true);
            else
                textureID = OpenGLLoader.LoadTextureInternal(path, true, true);
        }
    }
}
