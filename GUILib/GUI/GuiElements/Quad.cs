using OpenTK;
using GUILib.GUI.Render.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Events;
using GUILib.GUI.Animations;
using GUILib.GUI.Constraints;
using GUILib.GUI.PixelConstraints;

namespace GUILib.GUI.GuiElements
{
    class Quad : GuiElement
    {
        private Material material;
        public Quad(Material material, APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.material = material;
        }

        protected override void RenderElement(GuiShader shader, Vector2 trans, Vector2 scale, float opacity)
        {
            shader.ResetVAO();

            material.PrepareRender(shader, opacity);
            shader.SetTransform(trans, scale);

            GL.DrawArrays(PrimitiveType.Quads, 0, 4);
        }

        internal void SetMaterial(Material material)
        {
            this.material = material;
        }
    }
}
