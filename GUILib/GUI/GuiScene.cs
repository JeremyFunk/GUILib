using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using GuiLib.GUI.GuiElements;
using GuiLib.Util;
using GuiLib.Events;

namespace GuiLib.GUI
{
    class GuiScene
    {
        public List<GuiElement> parents;

        public GuiScene()
        {
            parents = new List<GuiElement>();
        }

        public void Update(float delta)
        {
            foreach (GuiElement el in parents)
            {
                if (!el.visible)
                    continue;
                el.Update(GameSettings.Width, GameSettings.Height, delta);
            }

            bool leftMousePressed = GameInput.IsMouseButtonPressed(OpenTK.Input.MouseButton.Left);
            bool rightMousePressed = GameInput.IsMouseButtonPressed(OpenTK.Input.MouseButton.Right);

            KeyEvent e = new KeyEvent(GameInput.GetKeysDown(), GameInput.GetKeysPressed());

            foreach (GuiElement parent in parents)
            {
                parent.KeyEvent(e);

                Vector2 mousePos = new Vector2(GameInput.mouseX, GameSettings.Height - GameInput.mouseY);
                Vector2 localPos = new Vector2(GameInput.mouseX - parent.x.GetPixelValue(GameSettings.Width), GameSettings.Height - GameInput.mouseY - parent.y.GetPixelValue(GameSettings.Height));

                if (MathsGeometry.IsInsideQuad(mousePos, parent))
                {
                    if (leftMousePressed)
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Down, true));
                    }
                    else if (GameInput.IsMouseButtonDown(OpenTK.Input.MouseButton.Left))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Pressed, true));
                    }
                    else if (GameInput.IsMouseButtonReleased(OpenTK.Input.MouseButton.Left))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Released, true));
                    }
                    else if (rightMousePressed)
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Down, true));
                    }
                    else if (GameInput.IsMouseButtonDown(OpenTK.Input.MouseButton.Right))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Pressed, true));
                    }
                    else if (GameInput.IsMouseButtonReleased(OpenTK.Input.MouseButton.Right))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Released, true));
                    }
                    else
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Hover, mousePos, localPos, false, false, MouseButtonType.None, true));
                    }
                }
                else
                {
                    if (leftMousePressed)
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Down, false));
                    }
                    else if (GameInput.IsMouseButtonDown(OpenTK.Input.MouseButton.Left))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Pressed, false));
                    }
                    else if (GameInput.IsMouseButtonReleased(OpenTK.Input.MouseButton.Left))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, true, false, MouseButtonType.Released, false));
                    }
                    else if (rightMousePressed)
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Down, false));
                    }
                    else if (GameInput.IsMouseButtonDown(OpenTK.Input.MouseButton.Right))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Pressed, false));
                    }
                    else if (GameInput.IsMouseButtonReleased(OpenTK.Input.MouseButton.Right))
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Click, mousePos, localPos, false, true, MouseButtonType.Released, false));
                    }
                    else
                    {
                        parent.MouseEvent(new MouseEvent(MouseEventType.Hover, mousePos, localPos, false, false, MouseButtonType.None, false));
                    }
                }
            }
        }
    }
}
