﻿using OpenTK;
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
using GUILib.GUI.PixelConstraints;
using GUILib.Logger;

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
        private Dictionary<Tab, TabContent> tabContent = new Dictionary<Tab, TabContent>();
        private Tab activeTab = null;
        private int borderSize = 0;

        public TabPane(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, Material fillMaterial = null, Material edgeMaterial = null, float zIndex = 0, bool visible = true, int edgeSize = -1) : base(width, height, x, y, visible, zIndex)
        {
            if (fillMaterial == null)
               fillMaterial = Theme.defaultTheme.GetButtonFillMaterial();

            borderSize = fillMaterial.GetBorderSize();

            Quad quad = new Quad(0, 0, width, height, fillMaterial);
            quad.heightConstraints.Add(new SubtractConstraint(Theme.defaultTheme.GetTabHeight() - borderSize));

            base.AddChild(quad);
        }

        public void AddTab(TabData tab)
        {
            Tab tabQuad = new Tab(tabs.Count * (Theme.defaultTheme.GetTabWidth() - borderSize) + 10, 0, tab.width == null ? Theme.defaultTheme.GetTabWidth() : tab.width, Theme.defaultTheme.GetTabHeight(), tab.name, -1, tab.fillMaterial, tab.edgeMaterial);
            tabQuad.yConstraints.Add(new MarginConstraint(0));
            tabQuad.SetTextColor(tab.fontColor);
            tabQuad.mouseButtonReleasedEvent = TabClicked;

            tabs.Add(tabQuad, tab.name);

            TabContent content = new TabContent(0, 0, width, height);
            content.heightConstraints.Add(new SubtractConstraint(Theme.defaultTheme.GetTabHeight() - borderSize));
            tabContent.Add(tabQuad, content);

            AddChild(content);
            AddChild(tabQuad);

            if (activeTab == null)
            {
                content.Activate();
                activeTab = tabQuad;
                activeTab.Activate();
            }
        }

        public void AddElementToTab(GuiElement element, string tabName)
        {
            foreach (Tab tab in tabs.Keys)
                if (tabs[tab] == tabName)
                {
                    AddElementToTab(element, tab);
                    return;
                }

            ALogger.defaultLogger.Log("Could not add element to the tab \'" + tabName + "\', because this tab does not exist.", LogLevel.Warning);
        }

        public void AddElementToTab(GuiElement element, TabData data)
        {
            foreach (Tab tab in tabs.Keys)
                if (tabs[tab] == data.name)
                {
                    AddElementToTab(element, tab);
                    return;
                }

            ALogger.defaultLogger.Log("Could not add element to the tab \'" + data.name + "\', because this tab does not exist.", LogLevel.Warning);
        }

        private void AddElementToTab(GuiElement element, Tab tab)
        {
            tabContent[tab].AddChild(element);
        }

        private void TabClicked(MouseEvent e, GuiElement el)
        {
            if (e.leftMouseButtonType == MouseButtonType.Released)
            {
                activeTab.Deactivate();
                tabContent[activeTab].Deactivate();
                activeTab = (Tab)el;
                activeTab.Activate();
                tabContent[activeTab].Activate();
            }
        }
    }
}
