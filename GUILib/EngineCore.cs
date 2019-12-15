using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.Util;
using GUILib.GUI.Render;
using GUILib.GUI.GuiElements;
using GUILib.GUI;
using GUILib.GUI.Constraints;
using GUILib.GUI.Animations;
using GUILib.Events;

using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using GUILib.GUI.Animations.Transitions;

namespace GUILib
{
    class EngineCore : GameWindow
    {
        private GuiRenderer guiRenderer;
        private GuiScene scene;

        public EngineCore(int widthP, int heightP, string title) : base(widthP, heightP, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8, 8, 8, 8), 24, 8, 4))
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            GameSettings.Width = Width;
            GameSettings.Height = Height;

            guiRenderer = new GuiRenderer();
            scene = new GuiScene();

            LoadGameMenuExample();

            GameInput.Initialize();

            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
        }
        private void LoadGameMenuExample()
        {
            VerticalList vList = new VerticalList(50, 0.5f, 100, 320, 5, 1);
            vList.yConstraints.Add(new CenterConstraint());

            GuiElement quad = new BordererdButton(0, 0, 250, 60, "New Game");
            GuiElement quad2 = new BordererdButton(0, 0, 250, 60, "Load Game");
            GuiElement quad3 = new BordererdButton(0, 0, 250, 60, "Options");
            GuiElement quad4 = new BordererdButton(0, 0, 250, 60, "Credits");
            GuiElement quad5 = new BordererdButton(0, 0, 250, 60, "Quit Game");

            GuiElement background = new Quad(new Material(new Texture("Background.jpg")));
            background.zIndex = -2;
            background.generalConstraint = new FillConstraintGeneral();

            Text text = new Text(0.5f, 0.5f, "Great Game", 2f);
            Text text2 = new Text(0.5f, 5, "Copyright Stuff", 0.7f);

            Window window = new Window(0.2f, 0.2f, 0.6f, 0.6f, "New Game...", -1);



            text.xConstraints.Add(new CenterConstraint());
            text.yConstraints.Add(new MarginConstraint(10));

            text2.xConstraints.Add(new MarginConstraint(10));

            quad.opacity = 0.6f;
            quad2.opacity = 0.6f;
            quad3.opacity = 0.6f;
            quad4.opacity = 0.6f;
            quad5.opacity = 0.6f;

            vList.AddElement(quad);
            vList.AddElement(quad2);
            vList.AddElement(quad3);
            vList.AddElement(quad4);
            vList.AddElement(quad5);

            scene.AddParent(background);
            scene.AddParent(text);
            scene.AddParent(vList);
            scene.AddParent(text2);
            scene.AddParent(window);

            AnimationKeyframe k1 = new AnimationKeyframe(0);
            AnimationKeyframe k2 = new AnimationKeyframe(0.2f);

            k1.x = 0;
            k2.x = 30;
            k2.width = 10;
            k2.height = 5;
            k2.opacity = 0.3f;

            AnimationKeyframe c1 = new AnimationKeyframe(0);
            AnimationKeyframe c2 = new AnimationKeyframe(0.07f);
;
            c2.width = 8;
            c2.height = 4;
            c2.x = -4;
            c2.y = -2;

            List<AnimationKeyframe> keyframes = new List<AnimationKeyframe>();
            keyframes.Add(k1);
            keyframes.Add(k2);

            List<AnimationKeyframe> cKeyframes = new List<AnimationKeyframe>();
            cKeyframes.Add(c1);
            cKeyframes.Add(c2);

            AnimationClass animationStruct = new AnimationClass(AnimationType.PauseOnEnd, keyframes, "Fade");
            animationStruct.transition = new CatmullRomSplineTransition(-15, 1);

            AnimationClass cAnimationStruct = new AnimationClass(AnimationType.SwingBack, cKeyframes, "Click");
            cAnimationStruct.transition = new SmoothstepTransition(1);

            Animation a = new Animation();
            a.AddAnimation(animationStruct);
            a.AddAnimation(cAnimationStruct);

            quad.animation = a;
            quad2.animation = a;
            quad3.animation = a;
            quad4.animation = a;
            quad5.animation = a;

            quad.SetStartHoverAnimation("Fade", AnimationRunType.Run);
            quad2.SetStartHoverAnimation("Fade", AnimationRunType.Run);
            quad3.SetStartHoverAnimation("Fade", AnimationRunType.Run);
            quad4.SetStartHoverAnimation("Fade", AnimationRunType.Run);
            quad5.SetStartHoverAnimation("Fade", AnimationRunType.Run);

            quad.SetEndHoverAnimation("Fade", AnimationRunType.Swing);
            quad2.SetEndHoverAnimation("Fade", AnimationRunType.Swing);
            quad3.SetEndHoverAnimation("Fade", AnimationRunType.Swing);
            quad4.SetEndHoverAnimation("Fade", AnimationRunType.Swing);
            quad5.SetEndHoverAnimation("Fade", AnimationRunType.Swing);

            quad.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);
            quad2.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);
            quad3.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);
            quad4.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);
            quad5.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);
        }

        protected override void OnResize(EventArgs e)
        {
            GameSettings.Width = Width;
            GameSettings.Height = Height;

            GL.Viewport(0, 0, Width, Height);

            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            scene.Update((float)e.Time);
            
            GameInput.Update();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            guiRenderer.PrepareRender();
            guiRenderer.Render(scene);

            this.SwapBuffers();
        }






        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            guiRenderer.CleanUp();
        }













        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            GameInput.UpdateKey(e.Key, false);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            GameInput.UpdateKey(e.Key, true);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            GameInput.mouseDx = GameInput.mouseX - e.X;
            GameInput.mouseDy = GameInput.mouseY - e.Y;

            GameInput.mouseX = e.X;
            GameInput.mouseY = e.Y;

            GameInput.normalizedMouseX = (2f * e.X) / Width - 1;
            GameInput.normalizedMouseY = -1 * ((2f * e.Y) / Height - 1);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            GameInput.mouseWheel = e.Delta;
            GameInput.mouseWheelF = e.DeltaPrecise;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            GameInput.mouseInside = false;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseLeave(e);
            GameInput.mouseInside = true;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            GameInput.UpdateMouseButton(e.Button, true);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            GameInput.UpdateMouseButton(e.Button, false);
        }
    }
}
