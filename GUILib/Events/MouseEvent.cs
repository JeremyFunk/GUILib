using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GUILib.Events
{
    /// <summary>
    /// This enum has four possible values: <c>Down</c>, <c>Pressed</c>, <c>Released</c> and <c>None</c>.
    /// This enum is used in the class <c>GUILib.Events.MouseEvent</c>.
    /// </summary>
    enum MouseButtonType
    {
        Down, Pressed, Released, None
    }

    ///<summary>
    ///Struct <c>MouseEvent</c> will be passed to every visible GUI Element every update.
    ///<c>MouseEvent</c> contains information about clicked mouse buttons and the mouse position.
    ///</summary>
    struct MouseEvent
    {
        /// <summary>
        /// <c>mousePositionWorld</c> contains the mouse position in the window.
        /// </summary>
        public Vector2 mousePositionWorld;
        /// <summary>
        /// <c>mousePositionWorld</c> contains the mouse position on the current GUI Element.
        /// </summary>
        public Vector2 mousePositionLocal;
        /// <summary>
        /// <c>leftMouseButtonType</c> is set to one of four values: 
        /// None if the left mouse button is not pressed, 
        /// Released if the left mouse button was pressed last update, but is not pressed in this update, 
        /// Pressed if the left mouse button was pressed last update and is pressed this update and
        /// Down if the left mouse button was not pressed last update but is pressed this update.
        /// </summary>
        public MouseButtonType leftMouseButtonType;
        /// <summary>
        /// <c>rightMouseButtonType</c> is set to one of four values: 
        /// None if the right mouse button is not pressed, 
        /// Released if the right mouse button was pressed last update, but is not pressed in this update, 
        /// Pressed if the right mouse button was pressed last update and is pressed this update and
        /// Down if the right mouse button was not pressed last update but is pressed this update.
        /// </summary>
        public MouseButtonType rightMouseButtonType;
        /// <summary>
        /// Is set to true if the mouse is over the Gui Element this event will be passed to.
        /// </summary>
        public bool hit;
        /// <summary>
        /// Is set to true if some object with a higher zIndex got hit before this one.
        /// </summary>
        public bool covered;
        /// <summary>
        /// Contains the state of the mouse wheel.
        /// </summary>
        public int mouseWheel;
        /// <summary>
        /// Is set to false if the mouse position was outside of the bounds of the gui element this event will be passed to.
        /// </summary>
        public bool canHit;

        public MouseEvent(Vector2 mousePositionWorld, Vector2 mousePositionLocal, MouseButtonType leftMouseButtonType, MouseButtonType rightMouseButtonType, bool hit, bool covered, int mouseWheel, bool canHit = true)
        {
            this.mousePositionLocal = mousePositionLocal;
            this.mousePositionWorld = mousePositionWorld;
            this.hit = hit;
            this.leftMouseButtonType = leftMouseButtonType;
            this.rightMouseButtonType = rightMouseButtonType;
            this.covered = covered;
            this.mouseWheel = mouseWheel;
            this.canHit = canHit;
        }

        /// <summary>
        /// Clones a mouse event.
        /// </summary>
        /// <param name="e">The mouse event that will be cloned.</param>
        public MouseEvent(MouseEvent e){
            mousePositionLocal = new Vector2(e.mousePositionLocal.X, e.mousePositionLocal.Y);
            mousePositionWorld = new Vector2(e.mousePositionWorld.X, e.mousePositionWorld.Y);
            hit = e.hit;
            leftMouseButtonType = e.leftMouseButtonType;
            rightMouseButtonType = e.rightMouseButtonType;
            covered = e.covered;
            mouseWheel = e.mouseWheel;
            canHit = e.canHit;
        }
    }
}

