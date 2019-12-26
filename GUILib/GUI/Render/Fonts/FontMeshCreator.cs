using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.GUI.Render.Fonts.Data;
using GUILib.Util;

namespace GUILib.GUI.Render.Fonts
{
    class FontMeshCreator
    {
        public static TextData CreateMesh(Font font, string text, float maxSize, float fontSize, out float width, out float height)
        {
            maxSize = maxSize * (1 / fontSize);

            if(text == "")
            {
                width = 0;
                height = 0;

                int vaoIDL = OpenGLLoader.LoadObject(new float[] { }, new float[] { });

                return new TextData(vaoIDL, 0);
            }
            width = 0;
            height = 0;

            List<float> vertices = new List<float>();
            List<float> texCoords = new List<float>();

            float cursorPos = 0;
            float smallestY = float.PositiveInfinity;
            float yLineOffset = 0;

            List<int> lineBreaks = new List<int>();
            int charCounter = 0;

            string[] words = text.Split(' ');

            float spaceBarAdvance = font.GetCharacter(' ').xAdvance;

            
            foreach (string word in words)
            {
                float thisWordSize = 0;
                int charCounterAtBeginningOfWord = charCounter;

                foreach (char c in word)
                {
                    if(c == '\n')
                    {
                        lineBreaks.Add(charCounter);
                        thisWordSize = 0;
                        cursorPos = 0;
                        continue;
                    }

                    Character character = font.GetCharacter(c);
                    if(character != null)
                    {
                        thisWordSize += character.xAdvance * font.xAdvance;
                        charCounter++;
                    }
                }

                cursorPos += thisWordSize;

                if (cursorPos > maxSize)
                {
                    lineBreaks.Add(charCounterAtBeginningOfWord);
                    cursorPos = thisWordSize;
                }


                charCounter++;
                cursorPos += spaceBarAdvance * font.xAdvance;
            }

            text = text.Replace("\n", "");

            cursorPos = 0;
            charCounter = 0;
            foreach (char c in text)
            {
                Character character = font.GetCharacter(c);

                while (lineBreaks.Contains(charCounter))
                {
                    lineBreaks.Remove(charCounter);
                    yLineOffset += 77;
                    cursorPos = 0;
                }

                /*if(cursorPos + character.width + character.xOffset > maxSize)
                {
                    yLineOffset += 77;
                    cursorPos = 0;
                }*/

                if (character != null)
                {
                    vertices.Add((cursorPos + character.xOffset) * fontSize);
                    vertices.Add((-character.yOffset - yLineOffset) * fontSize);
                    vertices.Add(0);

                    vertices.Add((cursorPos + character.width + character.xOffset) * fontSize);
                    vertices.Add((-character.yOffset - yLineOffset) * fontSize);
                    vertices.Add(0);

                    vertices.Add((cursorPos + character.width + character.xOffset) * fontSize);
                    vertices.Add((-(character.height + character.yOffset) - yLineOffset) * fontSize);
                    vertices.Add(0);

                    vertices.Add((cursorPos + character.xOffsetScreen) * fontSize);
                    vertices.Add((-(character.height + character.yOffset) - yLineOffset) * fontSize);
                    vertices.Add(0);

                    if (-(character.height + character.yOffset) < smallestY)
                        smallestY = -(character.height + character.yOffset);

                    texCoords.Add(character.xLowScreen);
                    texCoords.Add(character.yLowScreen);

                    texCoords.Add(character.xHighScreen);
                    texCoords.Add(character.yLowScreen);

                    texCoords.Add(character.xHighScreen);
                    texCoords.Add(character.yHighScreen);

                    texCoords.Add(character.xLowScreen);
                    texCoords.Add(character.yHighScreen);

                    cursorPos += character.xAdvance * font.xAdvance;

                    charCounter++;
                }
            }

            height = float.NegativeInfinity;

            for (int i = 0; i < vertices.Count; i += 3)
            {
                vertices[i + 1] += 70f * fontSize;
                if (vertices[i + 1] / 2 > height)
                    height = vertices[i + 1] / 2;
            };
            width = vertices[vertices.Count - 6] / 2;

            int vaoID = OpenGLLoader.LoadObject(vertices.ToArray(), texCoords.ToArray());


            return new TextData(vaoID, vertices.Count / 3);
        }
    }
}
