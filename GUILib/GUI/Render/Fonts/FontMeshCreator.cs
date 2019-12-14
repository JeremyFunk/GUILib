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
        public static TextData CreateMesh(Font font, string text, out float width, out float height)
        {
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

            foreach (char c in text)
            {
                Character character = font.GetCharacter(c);

                if (character != null)
                {
                    vertices.Add(cursorPos + character.xOffset);
                    vertices.Add(-character.yOffset);
                    vertices.Add(0);

                    vertices.Add(cursorPos + character.width + character.xOffset);
                    vertices.Add(-character.yOffset);
                    vertices.Add(0);

                    vertices.Add(cursorPos + character.width + character.xOffset);
                    vertices.Add(-(character.height + character.yOffset));
                    vertices.Add(0);

                    vertices.Add(cursorPos + character.xOffsetScreen);
                    vertices.Add(-(character.height + character.yOffset));
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

                    cursorPos += character.xAdvance * 0.8f;
                }
            }

            height = float.NegativeInfinity;

            for (int i = 0; i < vertices.Count; i += 3)
            {
                vertices[i + 1] += 70f;
                if (vertices[i + 1] / 2 > height)
                    height = vertices[i + 1] / 2;
            };
            width = vertices[vertices.Count - 6] / 2;

            int vaoID = OpenGLLoader.LoadObject(vertices.ToArray(), texCoords.ToArray());


            return new TextData(vaoID, vertices.Count / 3);
        }
    }
}
