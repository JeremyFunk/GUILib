using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using GUILib.GUI.Render.Shader;
using GUILib.Events;
using GUILib.GUI.Constraints;
using GUILib.Util;
using GUILib.GUI.Animations;

namespace GUILib.GUI.GuiElements
{
    public enum AnimationRunType
    {
        Run, Swing
    }

    abstract class GuiElement
    {
        private GuiElement parent;


        public Animation animation;

        private List<GuiElement> childElements = new List<GuiElement>();

        public List<Constraint> xConstraints = new List<Constraint>();
        public List<Constraint> yConstraints = new List<Constraint>();
        public List<Constraint> widthConstraints = new List<Constraint>();
        public List<Constraint> heightConstraints = new List<Constraint>();
        public GeneralConstraint generalConstraint = null;

        public APixelConstraint width, height, x, y;
        public int curX, curY, curWidth, curHeight;
        public float opacity;
        protected float curOpacity;
        
        public bool visible;

        public int animationOffsetX, animationOffsetY, animationOffsetWidth, animationOffsetHeight;
        public float animationOffsetOpacity;

        public Action<MouseEvent, GuiElement> mouseButtonDownEvent;
        public Action<MouseEvent, GuiElement> mouseButtonPressedEvent;
        public Action<MouseEvent, GuiElement> mouseButtonReleasedEvent;
        public Action<MouseEvent, GuiElement> mouseButtonMissedEvent;
        public Action<MouseEvent, GuiElement> mouseButtonPressedMissedEvent;

        public bool hovered = false;

        public Action<MouseEvent, GuiElement> startHoverEvent;
        public Action<MouseEvent, GuiElement> hoverEvent;
        public Action<MouseEvent, GuiElement> endHoverEvent;

        private string startHoverAnimationName;
        private string endHoverAnimationName;
        private string leftMouseButtonReleasedAnimationName;
        private string leftMouseButtonDownAnimationName;

        private AnimationRunType startHoverAnimationType;
        private AnimationRunType endHoverAnimationType;
        private AnimationRunType leftMouseButtonReleasedAnimationType;
        private AnimationRunType leftMouseButtonDownAnimationType;

        public Material defaultMaterial;
        public Material hoverMaterial;
        public Material clickMaterial;

        protected Material curMaterial;


        //The bigger the zIndex, the later gets this element rendered.
        public float zIndex;

        public GuiElement(APixelConstraint width, APixelConstraint height, APixelConstraint x, APixelConstraint y, bool visible, float zIndex)
        {
            this.width = width;
            curWidth = GetCurWidth();
            this.height = height;
            curHeight = GetCurHeight();
            this.visible = visible;
            this.x = x;
            curX = GetCurX();
            this.y = y;
            curY = GetCurY();
            opacity = 1;
            curOpacity = opacity;
            this.zIndex = zIndex;
        }
        public void Render(GuiShader shader, Vector2 offset, float opacity)
        {
            Vector2 actualOffset = GetScreenOffset();
            actualOffset = new Vector2(actualOffset.X + offset.X, actualOffset.Y + offset.Y);

            RenderElement(shader, actualOffset, GetScreenScale(), opacity * curOpacity);

            foreach(GuiElement element in childElements)
            {
                if(element.visible)
                    element.Render(shader, actualOffset, opacity * curOpacity);
            }
        }

        protected virtual void RenderElement(GuiShader shader, Vector2 offset, Vector2 scale, float opacity) { }

        public virtual void UpdateElement(float delta) { }

        public void MouseEvent(MouseEvent e)
        {
            bool hoverResult = false;

            if (e.hit)
            {
                hoverResult = true;
                if (!hovered)
                {
                    if (startHoverAnimationName != null)
                    {
                        if (startHoverAnimationType == AnimationRunType.Run)
                            animation.RunAnimation(this, startHoverAnimationName);
                        else if (startHoverAnimationType == AnimationRunType.Swing)
                            animation.SwingAnimation(this, startHoverAnimationName);
                    }
                    startHoverEvent?.Invoke(e, this);
                    if (hoverMaterial != null)
                        curMaterial = hoverMaterial;
                }
                else
                {
                    hoverEvent?.Invoke(e, this);
                }

                if (e.mouseButtonType == MouseButtonType.Down)
                {
                    if (leftMouseButtonDownAnimationName != null && e.leftButtonDown) 
                    { 
                        if(leftMouseButtonDownAnimationType == AnimationRunType.Run)
                            animation.RunAnimation(this, leftMouseButtonDownAnimationName);
                        else if (leftMouseButtonDownAnimationType == AnimationRunType.Swing)
                            animation.SwingAnimation(this, leftMouseButtonDownAnimationName);
                    }
                    mouseButtonDownEvent?.Invoke(e, this);
                } else if (e.mouseButtonType == MouseButtonType.Pressed)
                {
                    mouseButtonPressedEvent?.Invoke(e, this);
                    if (clickMaterial != null)
                        curMaterial = clickMaterial;

                } else if (e.mouseButtonType == MouseButtonType.Released)
                {
                    if (leftMouseButtonReleasedAnimationName != null && e.leftButtonDown)
                    {
                        if (leftMouseButtonReleasedAnimationType == AnimationRunType.Run)
                            animation.RunAnimation(this, leftMouseButtonReleasedAnimationName);
                        else if (leftMouseButtonReleasedAnimationType == AnimationRunType.Swing)
                            animation.SwingAnimation(this, leftMouseButtonReleasedAnimationName);
                    }
                    mouseButtonReleasedEvent?.Invoke(e, this);

                    if (hoverMaterial != null)
                        curMaterial = hoverMaterial;
                }
            }
            else
            { 
                if (e.mouseButtonType == MouseButtonType.Released)
                {
                    mouseButtonMissedEvent?.Invoke(e, this);
                }else if(e.mouseButtonType == MouseButtonType.Pressed)
                {
                    mouseButtonPressedMissedEvent?.Invoke(e, this);
                }
            }

            if (hovered && !hoverResult)
            {
                if (endHoverAnimationName != null)
                {
                    if (endHoverAnimationType == AnimationRunType.Run)
                        animation.RunAnimation(this, endHoverAnimationName);
                    else if (endHoverAnimationType == AnimationRunType.Swing)
                        animation.SwingAnimation(this, endHoverAnimationName);
                }
                endHoverEvent?.Invoke(e, this);

                if (defaultMaterial != null)
                    curMaterial = defaultMaterial;
            }

            hovered = hoverResult;

            MouseEventElement(e);

            if (!e.covered)
            {
                foreach (GuiElement element in childElements)
                {
                    if (!element.visible)
                        continue;
                    MouseEvent newE = new MouseEvent(e);

                    if(!element.IsAnimationRunning())
                        newE.mousePositionLocal = new Vector2(e.mousePositionLocal.X - element.curX - element.animationOffsetX, e.mousePositionLocal.Y - element.curY - element.animationOffsetY);
                    else
                        newE.mousePositionLocal = new Vector2(e.mousePositionLocal.X - element.curX, e.mousePositionLocal.Y - element.curY);

                    if (MathsGeometry.IsInsideQuad(e.mousePositionLocal, element))
                    {
                        newE.hit = true;
                    }
                    else
                    {
                        newE.hit = false;
                    }


                    element.MouseEvent(newE);
                }
            }
            else
            {
                foreach (GuiElement element in childElements)
                {
                    if (!element.visible)
                        continue;
                    e.hit = false;
                    element.MouseEvent(e);
                }
            }
        }

        public virtual void MouseEventElement(MouseEvent e) 
        {

        }

        public virtual void KeyEvent(KeyEvent e) { }

        public void Update(int width, int height, float delta)
        {
            if(HandleGeneralConstraints(width, height))
            {

            }
            else
            {
                curWidth = HandleConstraintsW(widthConstraints, GetCurWidth(), width, height);
                curHeight = HandleConstraintsH(heightConstraints, GetCurHeight(), width, height);

                curX = HandleConstraintsX(xConstraints, GetCurX(), width, height);
                curY = HandleConstraintsY(yConstraints, GetCurY(), width, height);
            }

            curOpacity = animationOffsetOpacity + opacity;

            animation?.Update(delta, this);
            
            UpdateElement(delta);

            Vector2 realSize = GetScreenScale();

            foreach (GuiElement element in childElements)
                if(element.visible)
                    element.Update((int)realSize.X, (int)realSize.Y, delta);
        }

        private bool HandleGeneralConstraints(int width, int height)
        {
            if (generalConstraint == null)
                return false;
            int oX, oY, oW, oH;

            generalConstraint.ExecuteConstraint(this.x.GetPixelValue(width), this.y.GetPixelValue(height), this.width.GetPixelValue(width), this.height.GetPixelValue(height), width, height, out oX, out oY, out oW, out oH);

            curX = oX;
            curY = oY;
            curWidth = oW;
            curHeight = oH;

            return true;
        }

        private int HandleConstraintsX(List<Constraint> constraints, int pixelValue, int width, int height)
        {
            foreach(Constraint c in constraints)
            {
                Type cType = c.GetType();

                pixelValue = HandleGeneral(c, cType, pixelValue, width, height);

                if (cType == typeof(CenterConstraint))
                {
                    pixelValue = ((CenterConstraint)c).ExecuteConstraint(curWidth, width);
                }
                else if (cType == typeof(MarginConstraint))
                {
                    pixelValue = ((MarginConstraint)c).ExecuteConstraint(width, curWidth);
                }
            }

            return pixelValue;
        }

        private int HandleConstraintsY(List<Constraint> constraints, int pixelValue, int width, int height)
        {
            foreach (Constraint c in constraints)
            {
                Type cType = c.GetType();

                pixelValue = HandleGeneral(c, cType, pixelValue, width, height);

                if (cType == typeof(CenterConstraint))
                {
                    pixelValue = ((CenterConstraint)c).ExecuteConstraint(curHeight, height);
                }
                else if (cType == typeof(MarginConstraint))
                {
                    pixelValue = ((MarginConstraint)c).ExecuteConstraint(height, curHeight);
                }
            }

            return pixelValue;
        }


        private int HandleConstraintsW(List<Constraint> constraints, int pixelValue, int width, int height)
        {
            foreach (Constraint c in constraints)
            {
                Type cType = c.GetType();

                pixelValue = HandleGeneral(c, cType, pixelValue, width, height);
            }

            return pixelValue;
        }

        private int HandleConstraintsH(List<Constraint> constraints, int pixelValue, int width, int height)
        {
            foreach (Constraint c in constraints)
            {
                Type cType = c.GetType();

                pixelValue = HandleGeneral(c, cType, pixelValue, width, height);
            }

            return pixelValue;
        }

        private int HandleGeneral(Constraint c, Type cType, int pixelValue, int width, int height)
        {
            if (cType == typeof(MinConstraint))
            {
                pixelValue = ((MinConstraint)c).ExecuteConstraint(pixelValue);
            }
            else if (cType == typeof(MaxConstraint))
            {
                pixelValue = ((MaxConstraint)c).ExecuteConstraint(pixelValue);
            }
            else if (cType == typeof(FixConstraint))
            {
                pixelValue = ((FixConstraint)c).ExecuteConstraint(pixelValue);
            }
            else if (cType == typeof(AddConstraint))
            {
                pixelValue = ((AddConstraint)c).ExecuteConstraint(pixelValue);
            }
            else if (cType == typeof(SubtractConstraint))
            {
                pixelValue = ((SubtractConstraint)c).ExecuteConstraint(pixelValue);
            }


            return pixelValue;
        }

        public void StartAnimation(string animation)
        {
            this.animation?.RunAnimation(this, animation);
        }

        public bool IsAnimationRunning()
        {
            return animation != null ? animation.IsAnimationRunning(this) : false;
        }

        public void SetWidth(int width)
        {
            this.width = new PixelConstraint(width);
            curWidth = width;

        }

        public void SetHeight(int height)
        {
            this.height = new PixelConstraint(height);
            curHeight = height;
        }

        public void SetX(int x)
        {
            this.x = new PixelConstraint(x);
            curX = x;
        }

        public void SetY(int y)
        {
            this.y = new PixelConstraint(y);
            curY = y;
        }


        //The real visible position
        private Vector2 GetScreenOffset()
        {
            return new Vector2(curX + animationOffsetX, curY + animationOffsetY);
        }
        //The real visible scale
        private Vector2 GetScreenScale()
        {
            return new Vector2(curWidth + animationOffsetWidth, curHeight + animationOffsetHeight);
        }



        public void SetStartHoverAnimation(string animationName, AnimationRunType type)
        {
            startHoverAnimationType = type;
            startHoverAnimationName = animationName;
        }

        public void SetEndHoverAnimation(string animationName, AnimationRunType type)
        {
            endHoverAnimationType = type;
            endHoverAnimationName = animationName;
        }

        public void SetLeftMouseButtonReleasedAnimation(string animationName, AnimationRunType type)
        {
            leftMouseButtonReleasedAnimationType = type;
            leftMouseButtonReleasedAnimationName = animationName;
        }

        public void SetLeftMouseButtonDownAnimation(string animationName, AnimationRunType type)
        {
            leftMouseButtonDownAnimationType = type;
            leftMouseButtonDownAnimationName = animationName;
        }

        public virtual void AddChild(GuiElement child) 
        {
            childElements.Add(child);
            child.parent = this;
        }


        private int GetCurX()
        {
            if (parent == null)
                return x.GetPixelValue(GameSettings.Width);
            return x.GetPixelValue(parent.curX);
        }

        private int GetCurY()
        {
            if (parent == null)
                return y.GetPixelValue(GameSettings.Height);
            return y.GetPixelValue(parent.curY);
        }

        private int GetCurWidth()
        {
            if (parent == null)
                return width.GetPixelValue(GameSettings.Width);
            return width.GetPixelValue(parent.curWidth);
        }

        private int GetCurHeight()
        {
            if (parent == null)
                return height.GetPixelValue(GameSettings.Height);
            return height.GetPixelValue(parent.curHeight);
        }


        private static APixelConstraint GetPixelConstraintForThisElement(float value)
        {
            if (value > 1)
                return new PixelConstraint((int)value);
            else
                return new PixelSizeConstraint(value);
        }
    }
}
