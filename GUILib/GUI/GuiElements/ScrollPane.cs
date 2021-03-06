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
using GUILib.Util;

namespace GUILib.GUI.GuiElements
{
    class ScrollPane : GuiElement
    {
        private Container elementContainer;
        private Quad scrollBar;

        float mouseDragY;
        bool drag = false;

        private float scroll = 0;
        private bool scrollable = false;

        private int yOffset = 0;
        public int overScroll = 30;

        public ScrollPane(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, Material fill = null, Material edge = null, int sliderWidth = -1, Material sliderBackground = null, Material sliderMaterial = null, int edgeSize = -1, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            useStencilBuffer = true;

            Quad q = new Quad(0, 0, 0, 0, fill == null ? Theme.defaultTheme.GetScrollPaneMaterial() : fill); 
            q.generalConstraint = new FillConstraintGeneral();
            base.AddChild(q);

            elementContainer = new Container(0, 0, 1f, 1f);
            elementContainer.debugIdentifier = "J";

            Border border = new Border(edge == null ? Theme.defaultTheme.GetScrollPaneBorderMaterial() : edge, 1f, 1f, 1);
            border.generalConstraint = new FillConstraintGeneral();

            Quad scrollBarBackground = new Quad(0, 0, sliderWidth == -1 ? Theme.defaultTheme.GetScrollPaneScrollBarWidth() : sliderWidth, 1f, sliderBackground == null ? Theme.defaultTheme.GetScrollPaneScrollBarBackgroundMaterial() : sliderBackground);
            scrollBarBackground.xConstraints.Add(new MarginConstraint(0));
            base.AddChild(scrollBarBackground);
            base.AddChild(elementContainer);
            base.AddChild(border);

            scrollBar = new Quad(0, 0, sliderWidth == -1 ? Theme.defaultTheme.GetScrollPaneScrollBarWidth() : sliderWidth, 1f, sliderMaterial == null ? Theme.defaultTheme.GetScrollPaneScrollBarMaterial() : sliderMaterial);
            scrollBar.xConstraints.Add(new MarginConstraint(0));

            scrollBar.mouseButtonPressedEvent = ScrollBarDragEvent;
            scrollBar.mouseButtonPressedMissedEvent = ScrollBarDragEvent;
            scrollBar.mouseButtonDownEvent = ScrollBarDownEvent;
            scrollBar.mouseButtonReleasedEvent = ScrollBarReleasedEvent;
            scrollBar.mouseButtonReleasedMissedEvent = ScrollBarReleaseMissedEvent;

            base.AddChild(scrollBar);
        }

        public override void UpdateElement(float delta)
        {
            UpdateContainer();
        }
        bool firstUpdate = true;
        int lastHeight = 0;

        private void UpdateContainer()
        {
            if(lastHeight != curHeight)
            {
                elementContainer.SetHeight(curHeight);
                lastHeight = curHeight;
            }


            int lowestY = int.MaxValue;

            foreach (GuiElement element in elementContainer.GetElements())
            {
                if (element.curY < lowestY)
                {
                    lowestY = element.curY;
                }
            }

            if (lowestY < 0)
            {
                scrollBar.SetHeight((int)(((float)curHeight / (curHeight - lowestY)) * curHeight));

                elementContainer.SetHeight(curHeight - lowestY);
                elementContainer.SetY(lowestY);
                scrollable = true;

                yOffset = lowestY;

                if (firstUpdate)
                {
                    firstUpdate = false;
                    scrollBar.SetY(curHeight - scrollBar.curHeight);
                }
            }
            else if(elementContainer.curHeight - lowestY < curHeight)
            {
                scrollable = false;
                scrollBar.SetHeight(curHeight);
                scrollBar.SetY(0);
                elementContainer.SetY(curHeight - elementContainer.curHeight);
            }
        }

        private void ScrollBarDownEvent(MouseEvent e, GuiElement el)
        {
            if (!scrollable)
                return;

            if (e.leftMouseButtonType != MouseButtonType.None)
            {
                mouseDragY = e.mousePositionWorld.Y;
                drag = true;
            }
        }

        private void ScrollBarReleaseMissedEvent(MouseEvent e, GuiElement el)
        {
            drag = false;
        }

        private void ScrollBarReleasedEvent(MouseEvent e, GuiElement el)
        {
            if (!scrollable)
                return;

            if (drag)
            {
                int newY = (int)(scrollBar.curY + (e.mousePositionWorld.Y - mouseDragY));

                if (newY > curHeight - scrollBar.curHeight)
                    newY = curHeight - scrollBar.curHeight;
                else if (newY < 0)
                    newY = 0;

                scroll = 1f - newY / ((float)curHeight - scrollBar.curHeight);

                elementContainer.SetY((int)((elementContainer.curHeight - curHeight + overScroll) * scroll) + yOffset);


                scrollBar.SetY(newY);
                drag = false;
            }
        }

        private void ScrollBarDragEvent(MouseEvent e, GuiElement el)
        {
            if (!scrollable)
                return;

            if (e.leftMouseButtonType != MouseButtonType.None)
            {
                if (drag)
                {
                    int newY = (int)(scrollBar.curY + (e.mousePositionWorld.Y - mouseDragY));

                    if (newY > curHeight - scrollBar.curHeight)
                        newY = curHeight - scrollBar.curHeight;
                    else if (newY < 0)
                        newY = 0;

                    scroll = 1f - newY / ((float)curHeight - scrollBar.curHeight);

                    elementContainer.SetY((int)((elementContainer.curHeight - curHeight + overScroll) * scroll) + yOffset);

                    scrollBar.SetY(newY);
                    mouseDragY = e.mousePositionWorld.Y;
                }
            }
        }

        public override void MouseEventElement(MouseEvent e)
        {
            if (!scrollable)
                return;
            if (e.hit)
            {
                if(e.mouseWheel != 0)
                {
                    int newY = (int)(scrollBar.curY + (e.mouseWheel * GameSettings.ScrollSpeed));

                    if (newY > curHeight - scrollBar.curHeight)
                        newY = curHeight - scrollBar.curHeight;
                    else if (newY < 0)
                        newY = 0;

                    scroll = 1f - newY / ((float)curHeight - scrollBar.curHeight);


                    elementContainer.SetY((int)((elementContainer.curHeight - curHeight + overScroll) * scroll) + yOffset);

                    scrollBar.SetY(newY);
                    mouseDragY = e.mousePositionWorld.Y;
                }
            }
        }

        public override void AddChild(GuiElement element)
        {
            elementContainer.AddChild(element);
        }
    }
}
