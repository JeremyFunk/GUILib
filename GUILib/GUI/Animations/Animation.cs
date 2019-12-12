using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiLib.GUI.Constraints;
using GuiLib.GUI.GuiElements;

namespace GuiLib.GUI.Animations
{
    public struct AnimationStruct
    {
        public int startX, startY, endX, endY;

        public float startOpacity, endOpacity;

        public float animationTime;

        public AnimationStruct(float animationTime)
        {
            this.animationTime = animationTime;

            startX = startY = endX = endY = 0;
            startOpacity = endOpacity = 0;
        }
    }

    public enum AnimationState
    {
        Run, Stop, Reverse
    }

    class Animation
    {
        private AnimationStruct animation;

        public bool loopAnimation;
        public bool reverseSweep = false;
        private AnimationState animationState = AnimationState.Stop;
        private float timer = 0;

        public Animation(AnimationStruct animation, bool loopAnimation = false)
        {
            this.animation = animation;
            this.loopAnimation = false;
        }

        public void Update(float delta, GuiElement element)
        {
            if (animationState == AnimationState.Run)
            {
                timer += delta;

                float progression = timer / animation.animationTime;

                if(progression >= 1)
                {
                    element.animationOffsetX = 0;
                    element.animationOffsetY = 0;

                    if (reverseSweep)
                    {
                        progression -= (progression % 1) * 2;

                        element.animationOffsetX = (int)Math.Round(animation.startX + (animation.endX - animation.startX) * progression);
                        element.animationOffsetY = (int)Math.Round(animation.startY + (animation.endY - animation.startY) * progression);
                        element.animationOffsetOpacity = (animation.startOpacity + (animation.endOpacity - animation.startOpacity) * progression);

                        animationState = AnimationState.Reverse;
                        timer -= animation.animationTime;
                    }
                    else
                    {
                        StopAnimation(element);

                        if (loopAnimation)
                        {
                            RunAnimation(element);
                        }
                    }
                }
                else
                {
                    element.animationOffsetX = (int)Math.Round(animation.startX + (animation.endX - animation.startX) * progression);
                    element.animationOffsetY = (int)Math.Round(animation.startY + (animation.endY - animation.startY) * progression);
                    element.animationOffsetOpacity = (animation.startOpacity + (animation.endOpacity - animation.startOpacity) * progression);
                }
            }else if(animationState == AnimationState.Reverse)
            {
                timer += delta;

                float progression = timer / animation.animationTime;

                if (progression >= 1)
                {
                    StopAnimation(element);

                    if (loopAnimation)
                    {
                        RunAnimation(element);
                    }
                }
                else
                {
                    element.animationOffsetX = (int)Math.Round(animation.endX - (animation.endX - animation.startX) * progression);
                    element.animationOffsetY = (int)Math.Round(animation.endY - (animation.endY - animation.startY) * progression);
                    element.animationOffsetOpacity = (animation.endOpacity - (animation.endOpacity - animation.startOpacity) * progression);
                }
            }
        }

        public void RunAnimation(GuiElement element)
        {
            animationState = AnimationState.Run;

            timer = 0;

            element.animationOffsetX = animation.startX;
            element.animationOffsetY = animation.startY;
        }

        public void StopAnimation(GuiElement element)
        {
            animationState = AnimationState.Stop;

            timer = 0;

            element.animationOffsetX = animation.startX;
            element.animationOffsetY = animation.startY;
        }
    }
}

