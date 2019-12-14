using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiLib.Util;
using GuiLib.GUI.Render;
using GuiLib.GUI.GuiElements;
using GuiLib.GUI;
using GuiLib.GUI.Constraints;
using GuiLib.GUI.Animations;
using GuiLib.Events;

using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using GUILib.GUI.Animations.Transitions;

namespace GuiLib
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
            GuiElement quad = new Quad(50, 1080 / 2 + 200, 200, 60, new Material(new Vector4(1, 1, 1, 1)));
            GuiElement quad2 = new Quad(50, 1080 / 2 + 130, 200, 60, new Material(new Vector4(1, 1, 1, 1)));
            GuiElement quad3 = new Quad(50, 1080 / 2 + 60, 200, 60, new Material(new Vector4(1, 1, 1, 1)));
            GuiElement quad4 = new Quad(50, 1080 / 2 + -10, 200, 60, new Material(new Vector4(1, 1, 1, 1)));
            GuiElement quad5 = new Quad(50, 1080 / 2 + -80, 200, 60, new Material(new Vector4(1, 1, 1, 1)));


            quad.opacity = 0.4f;
            quad2.opacity = 0.4f;
            quad3.opacity = 0.4f;
            quad4.opacity = 0.4f;
            quad5.opacity = 0.4f;

            scene.parents.Add(quad);
            scene.parents.Add(quad2);
            scene.parents.Add(quad3);
            scene.parents.Add(quad4);
            scene.parents.Add(quad5);

            AnimationKeyframe k1 = new AnimationKeyframe(0);
            AnimationKeyframe k2 = new AnimationKeyframe(0.2f);

            k1.x = 0;
            k2.x = 30;
            k2.width = 10;
            k2.height = 5;
            k2.opacity = 0.5f;

            List<AnimationKeyframe> keyframes = new List<AnimationKeyframe>();
            keyframes.Add(k1);
            keyframes.Add(k2);

            AnimationClass animationStruct = new AnimationClass(AnimationType.PauseOnEnd, keyframes);
            animationStruct.transition = new CatmullRomSplineTransition(-15, 1);


            Animation a = new Animation();
            a.AddAnimation(animationStruct, "Fade");

            quad.animation = a;
            quad2.animation = a;
            quad3.animation = a;
            quad4.animation = a;
            quad5.animation = a;


            quad.hoverEvent = Hover;

            quad.startHoverEvent = StartHover;
            quad2.startHoverEvent = StartHover;
            quad3.startHoverEvent = StartHover;
            quad4.startHoverEvent = StartHover;
            quad5.startHoverEvent = StartHover;

            quad.endHoverEvent = EndHover;
            quad2.endHoverEvent = EndHover;
            quad3.endHoverEvent = EndHover;
            quad4.endHoverEvent = EndHover;
            quad5.endHoverEvent = EndHover;
        }


        protected override void OnResize(EventArgs e)
        {
            GameSettings.Width = Width;
            GameSettings.Height = Height;

            GL.Viewport(0, 0, Width, Height);

            base.OnResize(e);
        }


        private void StartHover(MouseEvent e, GuiElement el)
        {
            el.StartAnimation("Fade");
        }
        private void Hover(MouseEvent e, GuiElement el)
        {
            
        }
        private void EndHover(MouseEvent e, GuiElement el)
        {
            el.animation.SwingAnimation(el);
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
