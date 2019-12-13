using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiLib.GUI.Constraints;
using GuiLib.GUI.GuiElements;
using GuiLib.Logger;
using GUILib.GUI.Animations.Transitions;

namespace GuiLib.GUI.Animations
{
    public enum AnimationType
    {
        //swingBack = when animation stops it takes the exact same amount of time to return back to the start state. 
        SwingBack, Normal, PauseOnEnd
    }

    public struct AnimationKeyframe
    {
        public int x, y, width, height;
        public float opacity;
        public float keyframeTime;

        public AnimationKeyframe(float keyframeTime)
        {
            this.keyframeTime = keyframeTime;

            x = y = width = height = 0;
            opacity = 0;
        }
    }

    public class AnimationClass
    {
        public AnimationKeyframe[] animationKeyframes;

        public AnimationType animationType;

        public float maxTime;

        public Transition transition = Transition.defaultTransition;

        public AnimationClass(AnimationType animationType, AnimationKeyframe[] keyframes)
        {
            if (keyframes.Length < 2)
            {
                ALogger.defaultLogger.Log("An AnimationKeyframe array had the wrong size. The size of this array has to be two or greater but was " + keyframes.Length, LogLevel.Error);
            }

            this.animationKeyframes = keyframes;
            this.animationType = animationType;

            foreach(AnimationKeyframe keyframe in keyframes)
            {
                if (keyframe.keyframeTime > maxTime)
                    maxTime = keyframe.keyframeTime;
            }
        }

        public AnimationClass(AnimationType animationType, List<AnimationKeyframe> keyframes)
        {
            if (keyframes.Count < 2)
            {
                ALogger.defaultLogger.Log("An AnimationKeyframe list had the wrong size. The size of this list has to be two or greater, but was " + keyframes.Count, LogLevel.Error);
            }

            AnimationKeyframe[] keyframeArr = new AnimationKeyframe[keyframes.Count];

            for (int i = 0; i < keyframes.Count; i++)
            {
                keyframeArr[i] = keyframes[i];
            }

            foreach (AnimationKeyframe keyframe in keyframes)
            {
                if (keyframe.keyframeTime > maxTime)
                    maxTime = keyframe.keyframeTime;
            }

            this.animationKeyframes = keyframeArr;
            this.animationType = animationType;
        }
    }

    public class AnimationState
    {
        public AnimationStateEnum state;
        public float timer = 0;

        public AnimationState(AnimationStateEnum state)
        {
            this.state = state;
        }
    }

    public enum AnimationStateEnum
    {
        Run, Stop, Reverse, Pause
    }

    class Animation
    {
        private Dictionary<string, AnimationClass> animations;
        private Dictionary<GuiElement, AnimationClass> runningAnimations = new Dictionary<GuiElement, AnimationClass>();
        private Dictionary<GuiElement, AnimationState> animationStates = new Dictionary<GuiElement, AnimationState>();

        
        public Animation()
        {
            animations = new Dictionary<string, AnimationClass>();
        }

        public Animation(Dictionary<string, AnimationClass> animations)
        {
            this.animations = animations;
        }

        public void AddAnimation(AnimationClass animation, string animationName)
        {
            animations.Add(animationName, animation);
        }


        public void Update(float delta, GuiElement element)
        {
            if (runningAnimations.ContainsKey(element))
            {
                AnimationClass animation = runningAnimations[element];

                AnimationState animationStateClass = animationStates[element];
                AnimationStateEnum animationState = animationStateClass.state;

                if (animationState == AnimationStateEnum.Run)
                {
                    animationStateClass.timer += delta;
                    if(animation.maxTime <= animationStateClass.timer)
                    {
                        Console.WriteLine(animation.maxTime);

                        if(animation.animationType == AnimationType.SwingBack)
                        {
                            animationStates[element].state = AnimationStateEnum.Reverse;
                        }else if (animation.animationType == AnimationType.PauseOnEnd)
                        {
                            animationStates[element].state = AnimationStateEnum.Pause;
                            animationStateClass.timer = animation.maxTime;
                        }
                        else
                        {
                            AnimationEnded(element, animationStateClass);
                            return;
                        }
                    }
                }else if(animationState == AnimationStateEnum.Reverse)
                {
                    animationStateClass.timer -= delta;

                    if(animationStateClass.timer <= 0)
                    {
                        AnimationEnded(element, animationStateClass);
                        return;
                    }
                }else if(animationState == AnimationStateEnum.Pause)
                {
                    return;
                }
                    



                bool lowerKeyframeFound = false;
                AnimationKeyframe lowerKeyframe = GetLowerKeyframe(animation.animationKeyframes, animationStateClass.timer, out lowerKeyframeFound);


                if (lowerKeyframeFound)
                {
                    bool higherKeyframeFound = false;
                    AnimationKeyframe higherKeyframe = GetHigherKeyframe(animation.animationKeyframes, animationStateClass.timer, out higherKeyframeFound);

                    if (higherKeyframeFound)
                    {
                        SetObjectToKeyframes(element, lowerKeyframe, higherKeyframe, animationStateClass.timer, animation.transition);
                    }
                    else
                    {
                        ResetObject(element);
                    }
                }
                else
                {
                    SetObjectToKeyframes(element, animation.animationKeyframes[0]);
                }
            }
        }

        public void RunAnimation(GuiElement element, string animationName)
        {
            AnimationClass animation = animations[animationName];

            if (runningAnimations.ContainsKey(element))
                StopAnimation(element);
            runningAnimations.Add(element, animation);
            animationStates.Add(element, new AnimationState(AnimationStateEnum.Run));
        }

        public void StopAnimation(GuiElement element)
        {
            runningAnimations.Remove(element);
            animationStates.Remove(element);
        }

        public void SwingAnimation(GuiElement element)
        {
            animationStates[element].state = AnimationStateEnum.Reverse;
        }

        private AnimationKeyframe GetLowerKeyframe(AnimationKeyframe[] animationKeyframes, float timer, out bool lowerKeyframeFound)
        {
            lowerKeyframeFound = false;
            AnimationKeyframe lowerKeyframe = new AnimationKeyframe();
            float timeDelta = float.MaxValue;

            foreach (AnimationKeyframe keyframe in animationKeyframes)
            {
                if (keyframe.keyframeTime < timer && (keyframe.keyframeTime - timer) < timeDelta)
                {
                    lowerKeyframe = keyframe;
                    lowerKeyframeFound = true;
                    timeDelta = (keyframe.keyframeTime - timer);
                }
            }

            return lowerKeyframe;
        }

        private AnimationKeyframe GetHigherKeyframe(AnimationKeyframe[] animationKeyframes, float timer, out bool higherKeyframeFound)
        {
            higherKeyframeFound = false;
            AnimationKeyframe higherKeyframe = new AnimationKeyframe();
            float timeDelta = float.MaxValue;

            foreach (AnimationKeyframe keyframe in animationKeyframes)
            {

                if (keyframe.keyframeTime >= timer && (timer - keyframe.keyframeTime) < timeDelta)
                {
                    higherKeyframe = keyframe;
                    higherKeyframeFound = true;
                    timeDelta = (keyframe.keyframeTime - timer);
                }
            }

            return higherKeyframe;
        }

        private void SetObjectToKeyframes(GuiElement element, AnimationKeyframe keyframe1, AnimationKeyframe keyframe2, float timer, Transition transition)
        {

            float factor = (timer - keyframe1.keyframeTime) / (keyframe2.keyframeTime - keyframe1.keyframeTime);

            element.animationOffsetX = Interpolate(transition, keyframe1.x, keyframe2.x, factor);
            element.animationOffsetY = Interpolate(transition, keyframe1.y, keyframe2.y, factor);
            element.animationOffsetWidth = Interpolate(transition, keyframe1.width, keyframe2.width, factor);
            element.animationOffsetHeight = Interpolate(transition, keyframe1.height, keyframe2.height, factor);
            element.animationOffsetOpacity = Interpolate(transition, keyframe1.opacity, keyframe2.opacity, factor);
        }

        private void SetObjectToKeyframes(GuiElement element, AnimationKeyframe keyframe)
        {
            element.animationOffsetX = keyframe.x;
            element.animationOffsetY = keyframe.y;
            element.animationOffsetOpacity = keyframe.opacity;
        }

        private void ResetObject(GuiElement element)
        {
            element.animationOffsetOpacity = 0;
            element.animationOffsetX = 0;
            element.animationOffsetY = 0;
        }

        private void AnimationEnded(GuiElement element, AnimationState animation)
        {
            animation.timer = 0;
            ResetObject(element);

            StopAnimation(element);
        }

        private int Interpolate(Transition transition, int v1, int v2, float factor)
        {
            float actualFactor = transition.GetCalculatedResult(factor);

            return (int)(v1 + (v2 - v1) * actualFactor);
        }

        private float Interpolate(Transition transition, float v1, float v2, float factor)
        {
            float actualFactor = transition.GetCalculatedResult(factor);

            return (v1 + (v2 - v1) * actualFactor);
        }
    }
}

