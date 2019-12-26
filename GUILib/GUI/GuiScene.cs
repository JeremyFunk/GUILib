using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using GUILib.GUI.GuiElements;
using GUILib.Util;
using GUILib.Events;

namespace GUILib.GUI
{
    class GuiScene
    {
        private List<GuiElement> parents;

        public List<GuiElement> GetParents()
        {
            return parents;
        }

        public void AddParent(GuiElement element)
        {
            parents.Add(element);
            parents = Utility.GetZIndexSorted(parents);
        }

        public GuiScene()
        {
            parents = new List<GuiElement>();
            MouseInfo.Init(this);
        }

        public void FirstUpdate(float delta)
        {
            foreach (GuiElement el in parents)
            {
                el.FirstUpdate(GameSettings.Width, GameSettings.Height, delta);
            }
        }

        public void Update(float delta)
        {
            MouseInfo.HideMouseInfo();
            MouseInfo.UpdatePosition(GameInput.mouseX, GameSettings.Height - GameInput.mouseY);

            foreach (GuiElement el in parents)
            {
                if (!el.visible)
                    continue;
                el.Update(GameSettings.Width, GameSettings.Height, delta);
            }

            bool leftMousePressed = GameInput.IsMouseButtonPressed(OpenTK.Input.MouseButton.Left);
            bool rightMousePressed = GameInput.IsMouseButtonPressed(OpenTK.Input.MouseButton.Right);

            int mouseWheel = GameInput.mouseWheel;

            KeyEvent e = new KeyEvent(GameInput.GetKeysDown(), GameInput.GetKeysPressed());

            float zIndex = float.MinValue;

            for (int i = parents.Count - 1; i >= 0; i--)
            {
                GuiElement parent = parents[i];

                if (!parent.visible)
                    continue;

                parent.KeyEvent(e);

                Vector2 mousePos = new Vector2(GameInput.mouseX, GameSettings.Height - GameInput.mouseY);

                Vector2 localPos;

                if(!parent.IsAnimationRunning())
                    localPos = new Vector2(GameInput.mouseX - parent.curX - parent.animationOffsetX, GameSettings.Height - GameInput.mouseY - parent.curY - parent.animationOffsetY);
                else
                    localPos = new Vector2(GameInput.mouseX - parent.curX, GameSettings.Height - GameInput.mouseY - parent.curY);

                bool covered = parent.ZIndex < zIndex;

                if (MathsGeometry.IsInsideQuad(mousePos, parent) && !covered)
                {
                    zIndex = parent.ZIndex;

                    if (leftMousePressed)
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Down, true, covered, mouseWheel));
                    }
                    else if (GameInput.IsMouseButtonDown(OpenTK.Input.MouseButton.Left))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Pressed, true, covered, mouseWheel));
                    }
                    else if (GameInput.IsMouseButtonReleased(OpenTK.Input.MouseButton.Left))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Released, true, covered, mouseWheel));
                    }
                    else if (rightMousePressed)
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Down, true, covered, mouseWheel));
                    }
                    else if (GameInput.IsMouseButtonDown(OpenTK.Input.MouseButton.Right))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Pressed, true, covered, mouseWheel));
                    }
                    else if (GameInput.IsMouseButtonReleased(OpenTK.Input.MouseButton.Right))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Released, true, covered, mouseWheel));
                    }
                    else
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Hover, mousePos, localPos, false, false, MouseButtonType.None, true, covered, mouseWheel));
                    }
                }
                else
                {
                    if (leftMousePressed)
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Down, false, covered, mouseWheel));
                    }
                    else if (GameInput.IsMouseButtonDown(OpenTK.Input.MouseButton.Left))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Pressed, false, covered, mouseWheel));
                    }
                    else if (GameInput.IsMouseButtonReleased(OpenTK.Input.MouseButton.Left))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Released, false, covered, mouseWheel));
                    }
                    else if (rightMousePressed)
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Down, false, covered, mouseWheel));
                    }
                    else if (GameInput.IsMouseButtonDown(OpenTK.Input.MouseButton.Right))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Pressed, false, covered, mouseWheel));
                    }
                    else if (GameInput.IsMouseButtonReleased(OpenTK.Input.MouseButton.Right))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Released, false, covered, mouseWheel));
                    }
                    else
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Hover, mousePos, localPos, false, false, MouseButtonType.None, false, covered, mouseWheel));
                    }
                }
            }
        }
    }
}
