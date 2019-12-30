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

            MouseButtonType leftMouseButtonType = MouseButtonType.None;
            if (GameInput.IsMouseButtonPressed(OpenTK.Input.MouseButton.Left))
            {
                leftMouseButtonType = MouseButtonType.Down;
            }
            else if (GameInput.IsMouseButtonDown(OpenTK.Input.MouseButton.Left))
            {
                leftMouseButtonType = MouseButtonType.Pressed;
            }
            else if (GameInput.IsMouseButtonReleased(OpenTK.Input.MouseButton.Left))
            {
                leftMouseButtonType = MouseButtonType.Released;
            }

            MouseButtonType rightMouseButtonType = MouseButtonType.None;
            if (GameInput.IsMouseButtonPressed(OpenTK.Input.MouseButton.Right))
            {
                rightMouseButtonType = MouseButtonType.Down;
            }
            else if (GameInput.IsMouseButtonDown(OpenTK.Input.MouseButton.Right))
            {
                rightMouseButtonType = MouseButtonType.Pressed;
            }
            else if (GameInput.IsMouseButtonReleased(OpenTK.Input.MouseButton.Right))
            {
                rightMouseButtonType = MouseButtonType.Released;
            }

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

                    parent.MouseEvent(new MouseEvent(mousePos, localPos, leftMouseButtonType, rightMouseButtonType, true, covered, mouseWheel));
                }
                else
                {
                    parent.MouseEvent(new MouseEvent(mousePos, localPos, leftMouseButtonType, rightMouseButtonType, false, covered, mouseWheel));
                }
            }
        }
    }
}
