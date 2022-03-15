using OpenTK;
using GUILib.GUI.Render.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Events;
using GUILib.GUI.Animations;
using GUILib.GUI.Constraints;
using GUILib.GUI.PixelConstraints;

namespace GUILib.GUI.GuiElements
{
    public class TabContent : GuiElement
    {
        private List<GuiElement> dataChilds = new List<GuiElement>();

        public TabContent(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, float zIndex = 0, bool visible = false) : base(width, height, x, y, visible, zIndex)
        {
            
        }
        public void Activate()
        {
            visible = true;
        }
        public void Deactivate()
        {
            visible = false;
        }

        public override void AddChild(GuiElement element)
        {
            dataChilds.Add(element);
            base.AddChild(element);
        }
    }
}
