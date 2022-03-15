using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.Logger;
using GUILib.Util;

namespace GUILib.GUI.Render.Fonts.Data
{
    public class FontFileParser
    {
        public static Font LoadFont(string fontName)
        {
            ALogger.defaultLogger.Log("Loading font: " + fontName, LogLevel.Info);

            string[] fileContent = Loader.ReadFileLines(@"Fonts\" + fontName + ".fnt");

            Dictionary<int, Character> characters = new Dictionary<int, Character>();

            float scaleW = 512;
            float scaleH = 512;

            foreach(string line in fileContent)
            {
                if(line.StartsWith("char id=")){
                    string[] lineSplitted = line.Split('=');
                    int charID = int.Parse(lineSplitted[1].Substring(0, 5));
                    int x = int.Parse(lineSplitted[2].Substring(0, 4));
                    int y = int.Parse(lineSplitted[3].Substring(0, 4));
                    int width = int.Parse(lineSplitted[4].Substring(0, 4));
                    int height = int.Parse(lineSplitted[5].Substring(0, 4));
                    int xOffset = int.Parse(lineSplitted[6].Substring(0, 4));
                    int yOffset = int.Parse(lineSplitted[7].Substring(0, 4));
                    int xAdvance = int.Parse(lineSplitted[8].Substring(0, 4));

                    characters.Add(charID, new Character(x, y, width, height, xOffset, yOffset, xAdvance, scaleW, scaleH));
                }
            }

            return new Font(characters, new Texture(@"Fonts\" + fontName + ".png", true));
        }
    }
}
