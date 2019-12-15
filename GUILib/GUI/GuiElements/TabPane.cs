using OpenTK;
using GUILib.GUI.Render.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Events;
using GUILib.GUI.Animations;
using GUILib.GUI.Constraints;

namespace GUILib.GUI.GuiElements
{
    class TabPane : GuiElement
    {
        private Text text;

        private List<Tab> tabs = new List<Tab>();

        public TabPane(float x, float y, float width, float height, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, bool visible = true, int edgeSize = -1) : base(width, height, x, y, visible, zIndex)
        {
            if (fillMaterial == null)
               fillMaterial = Theme.defaultTheme.GetButtonFillMaterial();
            if (edgeMaterial == null)
                edgeMaterial = Theme.defaultTheme.GetButtonEdgeMaterial();
            if (edgeSize < 0)
                edgeSize = Theme.defaultTheme.GetButtonEdgeSize();

            BorderedQuad quad = new BorderedQuad(0, 0, width, curHeight - Theme.defaultTheme.GetTabHeight(), fillMaterial, edgeMaterial, edgeSize);

            AddChild(quad);

        }

        public void AddTab(Tab tab)
        {
            //AddChild(tab);
            tabs.Add(tab);

            BorderedQuad tabQuad = new BorderedQuad(0, 0, Theme.defaultTheme.GetTabWidth(), Theme.defaultTheme.GetTabHeight(), Theme.defaultTheme.GetTabFillMaterial(), Theme.defaultTheme.GetTabEdgeMaterial(), Theme.defaultTheme.GetTabEdgeSize());
            tabQuad.yConstraints.Add(new MarginConstraint(0));

            AddChild(tabQuad);
        }
    }
}
