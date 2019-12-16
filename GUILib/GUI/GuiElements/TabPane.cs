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
    struct TabData
    {
        public APixelConstraint width;
        public string name;
        public Material fillMaterial, edgeMaterial;
        public Vector4 fontColor;

        public TabData(string name, APixelConstraint width = null, Material fillMaterial = null, Material edgeMaterial = null)
        {
            this.width = width;
            this.name = name;
            this.fillMaterial = fillMaterial;
            this.edgeMaterial = edgeMaterial;
            fontColor = new Vector4(1);
        }
    }

    class TabPane : GuiElement
    {
        private Dictionary<Tab, string> tabs = new Dictionary<Tab, string>();

        public TabPane(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, bool visible = true, int edgeSize = -1) : base(width, height, x, y, visible, zIndex)
        {
            if (fillMaterial == null)
               fillMaterial = Theme.defaultTheme.GetButtonFillMaterial();
            if (edgeMaterial == null)
                edgeMaterial = Theme.defaultTheme.GetButtonEdgeMaterial();
            if (edgeSize < 0)
                edgeSize = Theme.defaultTheme.GetButtonEdgeSize();


            BorderedQuad quad = new BorderedQuad(0, 0, width, height, fillMaterial, edgeMaterial, edgeSize);
            quad.heightConstraints.Add(new SubtractConstraint(Theme.defaultTheme.GetTabHeight() - Theme.defaultTheme.GetTabEdgeSize()));

            AddChild(quad);

        }

        public void AddTab(TabData tab)
        {
            Tab tabQuad = new Tab(tabs.Count * (Theme.defaultTheme.GetTabWidth() - Theme.defaultTheme.GetTabEdgeSize()) + 10, 0, tab.width == null ? Theme.defaultTheme.GetTabWidth() : tab.width, Theme.defaultTheme.GetTabHeight(), tab.name, -1, tab.fillMaterial, tab.edgeMaterial);
            tabQuad.yConstraints.Add(new MarginConstraint(0));
            tabQuad.SetTextColor(tab.fontColor);

            tabQuad.mouseButtonReleasedEvent = OnTabClicked;

            tabs.Add(tabQuad, tab.name);
            AddChild(tabQuad);
        }

        private void OnTabClicked(MouseEvent e, GuiElement el)
        {
            foreach(Tab tab in tabs.Keys)
            {
                if (tab == el)
                    Console.WriteLine(tabs[tab]);
            }
        }
    }
}
