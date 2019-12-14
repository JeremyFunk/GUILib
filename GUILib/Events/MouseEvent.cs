using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GUILib.Events
{
    enum MouseEventType
    {
        Hover, Click
    }

    enum MouseButtonType
    {
        Down, Pressed, Released, None
    }

    class MouseEvent
    {
        public MouseEventType type;
        public Vector2 mousePositionWorld, mousePositionLocal;
        public bool leftButtonDown, rightButtonDown;
        public MouseButtonType mouseButtonType;
        public bool hit;

        public MouseEvent(MouseEventType type, Vector2 mousePositionWorld, Vector2 mousePositionLocal, bool leftButtonDown, bool rightButtonDown, MouseButtonType mouseButtonType, bool hit)
        {
            this.type = type;
            this.mousePositionLocal = mousePositionLocal;
            this.mousePositionWorld = mousePositionWorld;
            this.leftButtonDown = leftButtonDown;
            this.rightButtonDown = rightButtonDown;
            this.hit = hit;
            this.mouseButtonType = mouseButtonType;
        }

        public MouseEvent(MouseEvent e){
            type = e.type;
            mousePositionLocal = new Vector2(e.mousePositionLocal.X, e.mousePositionLocal.Y);
            mousePositionWorld = new Vector2(e.mousePositionWorld.X, e.mousePositionWorld.Y);
            leftButtonDown = e.leftButtonDown;
            rightButtonDown = e.rightButtonDown;
            hit = e.hit;
            mouseButtonType = e.mouseButtonType;
        }
    }
}
