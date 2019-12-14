using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using GuiLib.GUI.Render.Shader;
using GuiLib.Events;
using GuiLib.GUI.Constraints;
using GuiLib.Util;
using GuiLib.GUI.Animations;

namespace GuiLib.GUI.GuiElements
{
    abstract class GuiElement
    {
        public Animation animation;

        public List<Constraint> xConstraints = new List<Constraint>();
        public List<Constraint> yConstraints = new List<Constraint>();
        public List<Constraint> widthConstraints = new List<Constraint>();
        public List<Constraint> heightConstraints = new List<Constraint>();

        public APixelConstraint width, height, x, y;
        public int curX, curY, curWidth, curHeight;
        public float opacity;
        protected float curOpacity;
        
        public bool visible;

        public int animationOffsetX, animationOffsetY, animationOffsetWidth, animationOffsetHeight;
        public float animationOffsetOpacity;

        private Action<MouseEvent, GuiElement> clickEvent;
        private Action<MouseEvent, GuiElement> notClickedEvent;

        public bool hovered = false;

        public Action<MouseEvent, GuiElement> startHoverEvent;
        public Action<MouseEvent, GuiElement> hoverEvent;
        public Action<MouseEvent, GuiElement> endHoverEvent;

        public GuiElement(float width, float height, float x, float y, bool visible)
        {
            this.width = GetPixelConstraintForThisElement(width);
            this.height = GetPixelConstraintForThisElement(height);
            this.visible = visible;
            this.x = GetPixelConstraintForThisElement(x);
            this.y = GetPixelConstraintForThisElement(y);
            opacity = 1;
            curOpacity = opacity;
        }
        public void Render(GuiShader shader, Vector2 offset)
        {
            RenderElement(shader, new Vector2(offset.X + curX + animationOffsetX, offset.Y + curY + animationOffsetY), new Vector2(curWidth + animationOffsetWidth, curHeight + animationOffsetHeight));
        }

        protected abstract void RenderElement(GuiShader shader, Vector2 offset, Vector2 scale);

        public abstract void UpdateElement(float delta);

        public void MouseEvent(MouseEvent e)
        {
            bool hoverResult = false;

            if (e.hit)
            {
                hoverResult = true;
                if (e.type == MouseEventType.Hover || true) //Not sure if this makes sense yet
                {
                    if (!hovered)
                    {
                        startHoverEvent?.Invoke(e, this);
                    }
                    else
                    {
                        hoverEvent?.Invoke(e, this);
                    }
                }
            }

            if(hovered && !hoverResult)
            {
                endHoverEvent?.Invoke(e, this);
            }

            hovered = hoverResult;

            MouseEventElement(e);
        }

        public virtual void MouseEventElement(MouseEvent events) 
        {
        }

        public virtual void KeyEvent(KeyEvent events) { }

        public void Update(int width, int height, float delta)
        {
            curWidth = HandleConstraintsW(widthConstraints, this.width.GetPixelValue(width), width, height);
            curHeight = HandleConstraintsH(heightConstraints, this.height.GetPixelValue(height), width, height);

            curX = HandleConstraintsX(xConstraints, x.GetPixelValue(width), width, height);
            curY = HandleConstraintsY(yConstraints, y.GetPixelValue(height), width, height);

            curOpacity = animationOffsetOpacity + opacity;

            animation?.Update(delta, this);

            UpdateElement(delta);
        }

        public int HandleConstraintsX(List<Constraint> constraints, int pixelValue, int width, int height)
        {
            foreach(Constraint c in constraints)
            {
                Type cType = c.GetType();

                pixelValue = HandleGeneral(c, cType, pixelValue, width, height);

                if (cType == typeof(CenterConstraint))
                {
                    pixelValue = ((CenterConstraint)c).ExecuteConstraint(curWidth, width);
                }
            }

            return pixelValue;
        }

        public int HandleConstraintsY(List<Constraint> constraints, int pixelValue, int width, int height)
        {
            foreach (Constraint c in constraints)
            {
                Type cType = c.GetType();

                pixelValue = HandleGeneral(c, cType, pixelValue, width, height);

                if (cType == typeof(CenterConstraint))
                {
                    pixelValue = ((CenterConstraint)c).ExecuteConstraint(curHeight, height);
                }
            }

            return pixelValue;
        }


        public int HandleConstraintsW(List<Constraint> constraints, int pixelValue, int width, int height)
        {
            foreach (Constraint c in constraints)
            {
                Type cType = c.GetType();

                pixelValue = HandleGeneral(c, cType, pixelValue, width, height);
            }

            return pixelValue;
        }

        public int HandleConstraintsH(List<Constraint> constraints, int pixelValue, int width, int height)
        {
            foreach (Constraint c in constraints)
            {
                Type cType = c.GetType();

                pixelValue = HandleGeneral(c, cType, pixelValue, width, height);
            }

            return pixelValue;
        }

        public int HandleGeneral(Constraint c, Type cType, int pixelValue, int width, int height)
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

            return pixelValue;
        }

        public void StartAnimation(string animation)
        {
            this.animation.RunAnimation(this, animation);
        }

        public bool IsAnimationRunning()
        {
            return animation.IsAnimationRunning(this);
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
