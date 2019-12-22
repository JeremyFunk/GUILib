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
        private int lowestY;
        private Container elementContainer;
        private BorderedQuad scrollBar;

        float mouseDragY;
        bool drag = false;

        private float scroll = 0;
        private bool scrollable = true;

        public ScrollPane(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, Material fill = null, Material edge = null, int edgeSize = -1, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            useStencilBuffer = true;

            Quad q = new Quad(fill == null ? Theme.defaultTheme.GetScrollPaneFillMaterial() : fill, 0, 0, 0, 0); 
            q.generalConstraint = new FillConstraintGeneral();
            base.AddChild(q);

            elementContainer = new Container(0, 0, 1f, 1f);
            elementContainer.debugIdentifier = "S";

            edgeSize = edgeSize == -1 ? Theme.defaultTheme.GetScrollPaneEdgeSize() : edgeSize;

            Border border = new Border(edge == null ? Theme.defaultTheme.GetScrollPaneEdgeMaterial() : edge, 1f, 1f, edgeSize, 1);
            border.generalConstraint = new FillConstraintGeneral();

            BorderedQuad scrollBarBackground = new BorderedQuad(0, 0, Theme.defaultTheme.GetScrollPaneScrollBarWidth(), 1f, Theme.defaultTheme.GetScrollPaneScrollBarBackgroundMaterial(), edge == null ? Theme.defaultTheme.GetScrollPaneEdgeMaterial() : edge, edgeSize);
            scrollBarBackground.xConstraints.Add(new MarginConstraint(0));
            base.AddChild(scrollBarBackground);
            base.AddChild(elementContainer);
            base.AddChild(border);

            scrollBar = new BorderedQuad(0, 0, Theme.defaultTheme.GetScrollPaneScrollBarWidth(), 0.5f, Theme.defaultTheme.GetScrollPaneScrollBarMaterial(), Theme.defaultTheme.GetScrollPaneScrollBarEdgeMaterial(), edgeSize);
            scrollBar.xConstraints.Add(new MarginConstraint(0));

            scrollBar.mouseButtonPressedEvent = ScrollBarDragEvent;
            scrollBar.mouseButtonPressedMissedEvent = ScrollBarDragEvent;
            scrollBar.mouseButtonDownEvent = ScrollBarDownEvent;
            scrollBar.mouseButtonReleasedEvent = ScrollBarReleasedEvent;
            scrollBar.mouseButtonReleasedMissedEvent = ScrollBarReleaseMissedEvent;

            base.AddChild(scrollBar);
        }

        bool firstUpdate = true;

        public override void UpdateElement(float delta)
        {
            lowestY = 0;

            foreach (GuiElement element in elementContainer.GetElements())
                if (element.curY < lowestY)
                    lowestY = element.curY;

            if(lowestY < 0)
            {
                scrollBar.SetHeight((int)(((float)curHeight / (-lowestY + curHeight)) * curHeight));

                if (firstUpdate)
                {
                    scrollBar.SetY(curHeight - scrollBar.curHeight);
                    Console.WriteLine((int)(((float)curHeight / (-lowestY + curHeight)) * curHeight));
                    firstUpdate = false;
                }
            }
            else 
            {
                scrollBar.SetHeight(curHeight);
                scrollable = false;
            }
        }

        private void ScrollBarDownEvent(MouseEvent e, GuiElement el)
        {
            if (!scrollable)
                return;

            if (e.leftButtonDown)
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

                elementContainer.SetY((int)((-lowestY + 30) * scroll));


                scrollBar.SetY(newY);
                drag = false;
            }
        }

        private void ScrollBarDragEvent(MouseEvent e, GuiElement el)
        {
            if (!scrollable)
                return;

            if (e.leftButtonDown)
            {
                if (drag)
                {
                    int newY = (int)(scrollBar.curY + (e.mousePositionWorld.Y - mouseDragY));

                    if (newY > curHeight - scrollBar.curHeight)
                        newY = curHeight - scrollBar.curHeight;
                    else if (newY < 0)
                        newY = 0;

                    scroll = 1f - newY / ((float)curHeight - scrollBar.curHeight);

                    elementContainer.SetY((int)((-lowestY + 30) * scroll));

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

                    elementContainer.SetY((int)((-lowestY + 30) * scroll));

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
