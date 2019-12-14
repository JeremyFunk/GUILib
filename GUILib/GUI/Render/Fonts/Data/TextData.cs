using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiLib.GUI.Render.Fonts.Data
{
    class TextData
    {
        public readonly int vaoID, vertexCount;

        public TextData(int vaoID, int vertexCount)
        {
            this.vaoID = vaoID;
            this.vertexCount = vertexCount; 
        }
    }
}
