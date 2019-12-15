﻿using OpenTK;
using GUILib.GUI.Render.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using GUILib.Events;
using GUILib.GUI.Animations;

namespace GUILib.GUI.GuiElements
{
    class Quad : GuiElement
    {
        private Material material;
        public Quad(Material material, float x = 0, float y = 0, float width = 0, float height = 0, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.material = material;
        }

        public override void MouseEventElement(MouseEvent events)
        {

        }

        public override void KeyEvent(KeyEvent events)
        {
        }

        protected override void RenderElement(GuiShader shader, Vector2 trans, Vector2 scale, float opacity)
        {
            shader.ResetVAO();

            material.PrepareRender(shader, opacity);
            shader.SetTransform(trans, scale);

            GL.DrawArrays(PrimitiveType.Quads, 0, 4);
        }

        public override void UpdateElement(float delta)
        {
        }

        internal void SetMaterial(Material material)
        {
            this.material = material;
        }
    }
}
