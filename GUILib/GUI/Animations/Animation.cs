using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.GUI.Constraints;
using GUILib.GUI.GuiElements;
using GUILib.Logger;
using GUILib.GUI.Animations.Transitions;

namespace GUILib.GUI.Animations
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

        public readonly string animationName;

        public float maxTime;

        public Transition transition = Transition.defaultTransition;

        public AnimationClass(AnimationType animationType, AnimationKeyframe[] keyframes, string animationName)
        {
            if (keyframes.Length < 2)
            {
                ALogger.defaultLogger.Log("An AnimationKeyframe array had the wrong size. The size of this array has to be two or greater but was " + keyframes.Length, LogLevel.Error);
            }

            this.animationKeyframes = keyframes;
            this.animationType = animationType;
            this.animationName = animationName;

            foreach (AnimationKeyframe keyframe in keyframes)
            {
                if (keyframe.keyframeTime > maxTime)
                    maxTime = keyframe.keyframeTime;
            }
        }

        public AnimationClass(AnimationType animationType, List<AnimationKeyframe> keyframes, string animationName)
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
            this.animationName = animationName;
        }
    }

    public class AnimationState
    {
        public AnimationStateEnum state;
        public float timer = 0;

        public AnimationClass animation;

        public int x, y, width, height;
        public float opacity;

        public AnimationState(AnimationStateEnum state, AnimationClass animation)
        {
            this.state = state;
            this.animation = animation;
        }

        internal void ResetValues()
        {
            x = y = width = height = 0;
            opacity = 0;
        }
    }

    public enum AnimationStateEnum
    {
        Run, Reverse, Pause
    }

    class Animation
    {
        private Dictionary<string, AnimationClass> animations;
        private Dictionary<GuiElement, List<AnimationState>> animationStates   = new Dictionary<GuiElement, List<AnimationState>>();

        
        public Animation()
        {
            animations = new Dictionary<string, AnimationClass>();
        }

        public Animation(Dictionary<string, AnimationClass> animations)
        {
            this.animations = animations;
        }

        public void AddAnimation(AnimationClass animation)
        {
            animations.Add(animation.animationName, animation);
        }


        private void PreUpdate(GuiElement element)
        {
            foreach(AnimationState state in animationStates[element]){
                if(state.state != AnimationStateEnum.Pause)
                    state.ResetValues();
                element.animationOffsetX = 0;
                element.animationOffsetY = 0;
                element.animationOffsetWidth = 0;
                element.animationOffsetHeight = 0;
                element.animationOffsetOpacity = 0;
            }
        }

        public void Update(float delta, GuiElement element)
        {
            if (!animationStates.ContainsKey(element))
                return;

            PreUpdate(element);

            List<AnimationState> endedAnimations = new List<AnimationState>();

            if (animationStates.ContainsKey(element))
            {
                List<AnimationState> animationStateClasses = animationStates[element];

                foreach(AnimationState animationStateClass in animationStateClasses)
                {
                    AnimationStateEnum animationState = animationStateClass.state;
                    AnimationClass animation = animationStateClass.animation;

                    if (animationState == AnimationStateEnum.Run)
                    {
                        animationStateClass.timer += delta;
                        if (animation.maxTime <= animationStateClass.timer)
                        {
                            if (animation.animationType == AnimationType.SwingBack)
                            {
                                animationStateClass.timer = animation.maxTime;
                                animationStateClass.state = AnimationStateEnum.Reverse;
                            }
                            else if (animation.animationType == AnimationType.PauseOnEnd)
                            {
                                animationStateClass.state = AnimationStateEnum.Pause;
                                animationStateClass.timer = animation.maxTime;
                            }
                            else
                            {
                                endedAnimations.Add(animationStateClass);
                                continue;
                            }
                        }
                    }
                    else if (animationState == AnimationStateEnum.Reverse)
                    {
                        animationStateClass.timer -= delta;

                        if (animationStateClass.timer <= 0)
                        {
                            endedAnimations.Add(animationStateClass);
                            continue;
                        }
                    }
                    else if (animationState == AnimationStateEnum.Pause)
                    {
                        continue;
                    }




                    bool lowerKeyframeFound = false;
                    AnimationKeyframe lowerKeyframe = GetLowerKeyframe(animation.animationKeyframes, animationStateClass.timer, out lowerKeyframeFound);


                    if (lowerKeyframeFound)
                    {
                        bool higherKeyframeFound = false;
                        AnimationKeyframe higherKeyframe = GetHigherKeyframe(animation.animationKeyframes, animationStateClass.timer, out higherKeyframeFound);

                        if (higherKeyframeFound)
                        {
                            SetObjectToKeyframes(lowerKeyframe, higherKeyframe, animationStateClass);
                        }
                    }
                    else
                    {
                        SetObjectToKeyframes(animationStateClass, animation.animationKeyframes[0]);
                    }
                }
            }

            foreach(AnimationState state in endedAnimations)
            {
                AnimationEnded(element, state);
            }

            PostUpdate(element);
        }

        public void PostUpdate(GuiElement element)
        {
            foreach (AnimationState state in animationStates[element])
            {
                element.animationOffsetX += state.x;
                element.animationOffsetY += state.y;
                element.animationOffsetWidth += state.width;
                element.animationOffsetHeight += state.height;
                element.animationOffsetOpacity += state.opacity;
            }
        }

        internal bool IsAnimationRunning(GuiElement element)
        {
            if (animationStates.ContainsKey(element))
            {
                if (animationStates.ContainsKey(element))
                {
                    foreach(AnimationState state in animationStates[element])
                        if(state.state != AnimationStateEnum.Pause)
                            return true;
                }    
            }
            return false;
        }

        public void RunAnimation(GuiElement element, string animationName)
        {
            if (!animations.ContainsKey(animationName))
            {
                ALogger.defaultLogger.Log("Could not run animation " + animationName + " because this animation does not exist!", LogLevel.Warning);
                return;
            }

            AnimationClass animation = animations[animationName];

            if (!animationStates.ContainsKey(element))
            {
                animationStates.Add(element, new List<AnimationState>());
            }
            else
            {
                //To avoid editting the list while looping
                Dictionary<GuiElement, AnimationState> toRemove = new Dictionary<GuiElement, AnimationState>();

                foreach(AnimationState state in animationStates[element])
                {
                    if(state.animation == animation && state.state != AnimationStateEnum.Pause)
                    {
                        toRemove.Add(element, state);
                    }
                }

                foreach (GuiElement curElement in toRemove.Keys)
                    StopAnimation(curElement, toRemove[curElement]);
            }

            animationStates[element].Add(new AnimationState(AnimationStateEnum.Run, animation));
        }

        public void StopAllAnimation(GuiElement element)
        {
            if (animationStates.ContainsKey(element))
            {
                animationStates[element].Clear();
            }
            else
            {
                ALogger.defaultLogger.Log("Could not stop all animations because the element does not have any animations running!", LogLevel.Info);
            }
        }

        public void StopAnimation(GuiElement element, AnimationState state)
        {
            if (animationStates.ContainsKey(element))
            {
                if (animationStates[element].Contains(state))
                    animationStates[element].Remove(state);
                else
                    ALogger.defaultLogger.Log("Could not stop animation because the element does not have any animations running!", LogLevel.Info);
            }
            else
            {
                ALogger.defaultLogger.Log("Could not stop animation because the element does not have any animations running!", LogLevel.Info);
            }
                
        }

        public void SwingAnimation(GuiElement element, string animationName)
        {
            if (!animationStates.ContainsKey(element))
            {
                ALogger.defaultLogger.Log("Could not swing animation \"" + animationName + "\" because the element does not have any animations running!", LogLevel.Info);
                return;
            }

            AnimationState state = (animationStates[element].Find(e => e.animation.animationName == animationName));
            if(state == null)
            {
                ALogger.defaultLogger.Log("Could not swing animation \"" + animationName + "\" because this animation is not running on this element!", LogLevel.Info);
                return;
            }

            state.state = AnimationStateEnum.Reverse;
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

        private void SetObjectToKeyframes(AnimationKeyframe keyframe1, AnimationKeyframe keyframe2, AnimationState state)
        {
            float factor = (state.timer - keyframe1.keyframeTime) / (keyframe2.keyframeTime - keyframe1.keyframeTime);

            state.x = Interpolate(state.animation.transition, keyframe1.x, keyframe2.x, factor);
            state.y = Interpolate(state.animation.transition, keyframe1.y, keyframe2.y, factor);
            state.width = Interpolate(state.animation.transition, keyframe1.width, keyframe2.width, factor);
            state.height = Interpolate(state.animation.transition, keyframe1.height, keyframe2.height, factor);
            state.opacity = Interpolate(state.animation.transition, keyframe1.opacity, keyframe2.opacity, factor);
        }

        private void SetObjectToKeyframes(AnimationState state, AnimationKeyframe keyframe)
        {
            state.x = keyframe.x;
            state.y = keyframe.y;
            state.width = keyframe.width;
            state.height = keyframe.height;
            state.opacity = keyframe.opacity;
        }

        private void AnimationEnded(GuiElement element, AnimationState animation)
        {
            animation.timer = 0;
            StopAnimation(element, animation);
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

