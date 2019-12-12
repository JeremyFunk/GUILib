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


using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

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
            
            GuiElement quad = new Quad(0.25f, 0.25f, 0.5f, 0.5f, new Material(new Vector4(1, 1, 1, 1)));
            
            scene.parents.Add(quad);

            AnimationStruct s = new AnimationStruct();


            quad.widthConstraints.Add(new MaxConstraint(1000)); //Minimale Skalierung
            quad.widthConstraints.Add(new MinConstraint(500)); //Minimale Skalierung
            quad.heightConstraints.Add(new FixConstraint(500)); //Minimale Skalierung
            quad.xConstraints.Add(new CenterConstraint());

            /*
            quad.constraints.Add(new MaxConstriant(new PixelCoordinate(40), new PixelCoordinate(40))); //Maximale Skalierung

            quad.constraints.Add(new ScaleConstriant()); //Keine Skalierung

            quad.constraints.Add(new HCenterConstraint()); //Immer Horizontale Mitte
            quad.constraints.Add(new VCenterConstraint()); //Immer Vertikale Mitte
            */


            GameInput.Initialize();

            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
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
