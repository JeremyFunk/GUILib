using System;
using OpenTK;
using GUILib.GUI.Render.Shader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Events;
using GUILib.GUI.Animations;
using GUILib.GUI.Constraints;
using GUILib.GUI.PixelConstraints;
using GUILib.Logger;

namespace GUILib.GUI.GuiElements
{
    class Container : GuiElement
    {
        public Container(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex) 
        { 
            //Quad q = new Quad(new Material(new Vector4(1, 0, 0, 1)), 0, 0, 0, 0); q.generalConstraint = new FillConstraintGeneral(); AddChild(q);
        }
    }
}
