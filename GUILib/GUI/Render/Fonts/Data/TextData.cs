using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Render.Fonts.Data
{
    public class TextData
    {
        public readonly int vaoID, vertexCount;

        public TextData(int vaoID, int vertexCount)
        {
            this.vaoID = vaoID;
            this.vertexCount = vertexCount; 
        }
    }
}
