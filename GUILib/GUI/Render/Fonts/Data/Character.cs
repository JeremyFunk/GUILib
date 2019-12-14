using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI.Render.Fonts.Data
{
    class Character
    {
        public float xLow, yLow, width, height, xHigh, yHigh;
        public float xLowScreen, yLowScreen, widthScreen, heightScreen, xHighScreen, yHighScreen;
        public float xOffset, yOffset, xAdvance;
        public float xOffsetScreen, yOffsetScreen, xAdvanceScreen;

        public Character(int x, int y, int width, int height, int xOffset, int yOffset, int xAdvance, float imageWidth, float imageHeight)
        {
            this.xLow = x;
            this.yLow = y;
            this.width = width;
            this.height = height;
            this.xHigh = x + width;
            this.yHigh = y + height;


            xLowScreen = x / imageWidth;
            yLowScreen = y / imageHeight;
            widthScreen = width / imageWidth;
            heightScreen = height / imageHeight;
            xHighScreen = xLowScreen + widthScreen;
            yHighScreen = yLowScreen + heightScreen;

            this.xAdvance = xAdvance;
            this.xOffset = xOffset;
            this.yOffset = yOffset;

            this.xAdvanceScreen = xAdvance / imageWidth;
            this.xOffsetScreen = xOffset / imageWidth;
            this.yOffsetScreen = yOffset / imageHeight;
        }
    }
}
