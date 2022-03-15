using OpenTK;
using GUILib.GUI.Render.Shaders;
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
    public class ChoiceBox : GuiElement
    {
        private int padding;
        private List<TextSelectable> elements = new List<TextSelectable>();

        private Text text;

        private TextSelectable topElement = null;
        
        private Quad quad;
        private Quad dropDown;

        private bool selected, newSelected;

        public ChoiceBox(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, int padding, string noSelection, float zIndex = 0, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            this.padding = padding;
            quad = new Quad(0, 0, 1f, height, Theme.defaultTheme.GetButtonFillMaterial());
            dropDown = new Quad(0, 0, 1f, 0, Theme.defaultTheme.GetChoiceBoxMaterial());
            dropDown.visible = false;

            quad.mouseButtonReleasedEvent = QuadClicked;
            quad.mouseButtonReleasedMissedEvent = ClickMissed;

            text = new Text(0, 0, noSelection, 0.8f, null);

            text.xConstraints.Add(new CenterConstraint());
            text.yConstraints.Add(new MarginConstraint(7));

            base.AddChild(quad);
            base.AddChild(dropDown);
            base.AddChild(text);
        }

        private void ClickMissed(MouseEvent e, GuiElement el)
        {
            newSelected = false;
        }

        private void QuadClicked(MouseEvent e, GuiElement el)
        {
            if (e.leftMouseButtonType != MouseButtonType.None)
            {
                newSelected = !selected;
            }
        }

        public override void UpdateElement(float delta)
        {
            if (newSelected != selected)
                SetSelected(newSelected);
        }

        public void SetSelected(bool selected)
        {
            this.selected = selected;

            foreach (GuiElement element in elements)
                element.visible = selected;
            dropDown.visible = selected;
        }

        public override void AddChild(GuiElement guiElement)
        {
            if(guiElement.GetType() != typeof(TextSelectable))
            {
                Logger.ALogger.defaultLogger.Log("Could not add Element to choice box because this element was not of the supported type \"TextSelectable\"!", Logger.LogLevel.Error);
                return;
            }

            TextSelectable element = (TextSelectable)guiElement;

            element.xConstraints.Add(new CenterConstraint());

            if(elements.Count == 0)
            {
                elements.Add(element);
                element.SetY(0);

                SetY(curY - element.curHeight - Theme.defaultTheme.GetInitialDropDownPadding());
                SetHeight(curHeight + element.curHeight + Theme.defaultTheme.GetInitialDropDownPadding()); 
                quad.SetY(quad.curY + element.curHeight + Theme.defaultTheme.GetInitialDropDownPadding());
                dropDown.SetHeight(dropDown.curHeight + element.curHeight + Theme.defaultTheme.GetInitialDropDownPadding());
            }
            else
            {
                foreach(GuiElement curElement in elements)
                {
                    curElement.SetY(curElement.curY + element.curHeight + padding);
                }

                element.SetY(0);
                elements.Add(element);

                SetY(curY - element.curHeight - padding);
                SetHeight(curHeight + element.curHeight + padding); 
                quad.SetY(quad.curY + element.curHeight + padding);
                dropDown.SetHeight(dropDown.curHeight + element.curHeight + padding);
            }

            base.AddChild(element);

            element.mouseButtonReleasedEvent = ElementClicked;

            element.visible = selected;
        }

        private void ElementClicked(MouseEvent e, GuiElement el)
        {
            if (e.leftMouseButtonType != MouseButtonType.None)
            {
                TextSelectable selectable = (TextSelectable)el;

                text.SetText(selectable.GetText());
            }
        }
    }
}