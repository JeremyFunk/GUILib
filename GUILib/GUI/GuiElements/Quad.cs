using OpenTK;
using GuiLib.GUI.Render.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GuiLib.Events;
using GuiLib.GUI.Animations;

namespace GuiLib.GUI.GuiElements
{
    class Quad : GuiElement
    {
        private Material material;

        private Animation animation;

        public Quad(float x, float y, float width, float height, Material material, bool visible = true) : base(width, height, x, y, visible)
        {
            this.material = material;
            AnimationStruct animationStruct = new AnimationStruct(0.2f);
            animationStruct.endX = 200;
            animationStruct.endY = 10;
            animationStruct.startOpacity = 0;
            animationStruct.endOpacity = -0.7f;


            this.animation = new Animation(animationStruct);
            animation.reverseSweep = true;
        }

        public override void MouseEvent(MouseEvent events)
        {

        }

        public override void KeyEvent(KeyEvent events)
        {
            if(events.down.Contains(OpenTK.Input.Key.W))
            {
                animation.RunAnimation(this);
            }
        }

        protected override void RenderElement(GuiShader shader, Vector2 trans, Vector2 scale)
        {
            shader.ResetVAO();

            material.PrepareRender(shader, curOpacity);
            shader.SetTransform(trans, scale);

            GL.DrawArrays(PrimitiveType.Quads, 0, 4);
        }

        public override void UpdateElement(float delta)
        {
            animation.Update(delta, this);
        }
    }
}
