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
using GUILib.GUI.Constraints;

namespace GUILib.GUI.GuiElements
{
    class Button : GuiElement
    {
        private Quad quad;
        private Text text;
        public Button(float x, float y, float width, float height, Material material, string text = "", float zIndex = 0, int edgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            quad = new Quad(material, 0, 0, width, height);
            quad.generalConstraint = new FillConstraintGeneral();

            AddChild(quad);

            if (text != "")
            {
                this.text = new Text(0, 0, text, 1.2f);
                this.text.xConstraints.Add(new CenterConstraint());
                this.text.yConstraints.Add(new CenterConstraint());
                AddChild(this.text);
            }
        }

        public override void MouseEventElement(MouseEvent events)
        {}

        public override void KeyEvent(KeyEvent events)
        {}

        protected override void RenderElement(GuiShader shader, Vector2 trans, Vector2 scale, float opacity)
        {}

        public override void UpdateElement(float delta)
        {}

        internal void SetMaterial(Material material)
        {
            quad.SetMaterial(material);
        }
    }
}
