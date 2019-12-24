using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GUILib.GUI
{
    class DarkTheme : Theme
    {
        private readonly int windowTopBarSize = 35, tabHeight = 35, tabWidth = 130, tickBoxEdgeSize = 1;
        private readonly int scrollBarWidth = 20;

        private readonly int initialDropDownPadding = 10;

        private readonly float cursorTickRate = 0.5f;

        private readonly Material 
            buttonFillMaterial = GetDefaultMaterial(new Vector4(0.3f, 0.3f, 0.3f, 1f), true), 
            buttonHoverMaterial = GetDefaultMaterial(new Vector4(0.4f, 0.4f, 0.4f, 1f), true), 
            buttonClickMaterial = GetDefaultMaterial(new Vector4(0.5f, 0.5f, 0.5f, 1f), true);

        private readonly Material 
            fieldFillMaterial = GetDefaultMaterial(new Vector4(0.3f, 0.3f, 0.3f, 1f), true);

        private readonly Material 
            tableFillMaterial = GetDefaultMaterial(new Vector4(0.3f, 0.3f, 0.3f, 1f), true), 
            tableEdgeMaterial = GetDefaultMaterial(new Vector4(0.6f, 0.6f, 0.6f, 1f));

        private readonly Material 
            windowBackgroundMaterial = GetDefaultMaterial(new Vector4(0.4f, 0.4f, 0.4f, 0.95f), true), 
            windowEdgeMaterial = GetDefaultMaterial(new Vector4(0.7f, 0.7f, 0.7f, 1f)),
            windowTopBarMaterial = GetDefaultMaterial(new Vector4(0.3f, 0.3f, 0.3f, 1f), true);

        private readonly Material 
            tabFillMaterial = GetDefaultMaterial(new Vector4(0.35f, 0.35f, 0.35f, 1f), true), 
            tabActiveMaterial = GetDefaultMaterial(new Vector4(0.65f, 0.65f, 0.65f, 1f), true), 
            tabHoverMaterial = GetDefaultMaterial(new Vector4(0.45f, 0.45f, 0.45f, 1f), true), 
            tabClickMaterial = GetDefaultMaterial(new Vector4(0.55f, 0.55f, 0.55f, 1f), true);

        private readonly Material 
            sliderMaterial = GetDefaultMaterial(new Vector4(0.55f, 0.55f, 0.55f, 1f)), 
            sliderQuadMaterial = GetDefaultMaterial(new Vector4(0.85f, 0.85f, 0.85f, 1f));

        private readonly Material
            mouseInfoFillMaterial = GetDefaultMaterial(new Vector4(0.3f, 0.3f, 0.3f, 1f), true);

        private readonly Material 
            scrollPaneFillMaterial = GetDefaultMaterial(new Vector4(0.4f, 0.4f, 0.4f, 1f)), 
            scrollPaneEdgeMaterial = GetDefaultMaterial(new Vector4(0.8f, 0.8f, 0.8f, 1f)), 
            scrollBarBackgroundFillMaterial = GetDefaultMaterial(new Vector4(0.3f, 0.3f, 0.3f, 1f)), 
            scrollBarFillMaterial = GetDefaultMaterial(new Vector4(0.5f, 0.5f, 0.5f, 1f));

        private readonly Material 
            tickBoxDefaultMaterial = GetDefaultMaterial(new Vector4(0.3f, 0.3f, 0.3f, 0.5f), true), 
            tickBoxHoverMaterial = GetDefaultMaterial(new Vector4(0.6f, 0.6f, 0.6f, 0.7f), true), 
            tickBoxClickMaterial = GetDefaultMaterial(new Vector4(0.8f, 0.8f, 0.8f, 0.9f), true),
            tickBoxClickedMaterial = new Material(new Texture("TickBoxClicked.png"));

        private readonly Material 
            leftRightSliderFillMaterial = GetDefaultMaterial(new Vector4(0.3f, 0.3f, 0.3f, 1f), true);

        private readonly Material leftArrowMaterial = new Material(new Texture("LeftArrow.png")), rightArrowMaterial = new Material(new Texture("RightArrow.png")), downArrowMaterial = new Material(new Texture("DownArrow.png")),
            upArrowMaterial = new Material(new Texture("UpArrow.png"));

        private readonly Material seperatorMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 1f));
        

        private static Material GetDefaultMaterial(Vector4 color, bool useBorder = false)
        {
            if(!useBorder)
                return new Material(color);
            return new Material(color, new Vector4(0.7f, 0.7f, 0.7f, 1f), 2);
        }


        public override Material GetButtonHoverMaterial()
        {
            return buttonHoverMaterial;
        }

        public override Material GetButtonClickMaterial()
        {
            return buttonClickMaterial;
        }

        public override Material GetButtonFillMaterial()
        {
            return buttonFillMaterial;
        }

        public override Material GetFieldFillMaterial()
        {
            return fieldFillMaterial;
        }

        public override Material GetPanelFillMaterial()
        {
            return scrollPaneFillMaterial;
        }

        public override Material GetSeperatorMaterial()
        {
            return seperatorMaterial;
        }

        public override Material GetWindowBackgroundMaterial()
        {
            return windowBackgroundMaterial;
        }

        public override Material GetWindowTopBarMaterial()
        {
            return windowTopBarMaterial;
        }

        public override int GetWindowTopBarSize()
        {
            return windowTopBarSize;
        }

        public override int GetTabHeight()
        {
            return tabHeight;
        }

        public override int GetTabWidth()
        {
            return tabWidth;
        }

        public override Material GetTabFillMaterial()
        {
            return tabFillMaterial;
        }

        public override Material GetTabActiveMaterial()
        {
            return tabActiveMaterial;
        }

        public override Material GetTabHoveredMaterial()
        {
            return tabHoverMaterial;
        }

        public override Material GetTabClickedMaterial()
        {
            return tabClickMaterial;
        }

        public override float GetButtonFontSize()
        {
            return 1.2f;
        }

        public override int GetInitialDropDownPadding()
        {
            return initialDropDownPadding;
        }

        public override float GetCursorTickRate()
        {
            return cursorTickRate;
        }

        public override Material GetTableFillMaterial()
        {
            return tableFillMaterial;
        }

        public override Material GetSliderMaterial()
        {
            return sliderMaterial;
        }

        public override Material GetSliderQuadMaterial()
        {
            return sliderQuadMaterial;
        }

        public override Material GetMouseInfoFillMaterial()
        {
            return mouseInfoFillMaterial;
        }

        public override Material GetScrollPaneFillMaterial()
        {
            return scrollPaneFillMaterial;
        }

        public override int GetScrollPaneScrollBarWidth()
        {
            return scrollBarWidth;
        }

        public override Material GetScrollPaneScrollBarBackgroundMaterial()
        {
            return scrollBarBackgroundFillMaterial;
        }

        public override Material GetScrollPaneScrollBarMaterial()
        {
            return scrollBarFillMaterial;
        }

        public override Material GetTickBoxHoverMaterial()
        {
            return tickBoxHoverMaterial;
        }

        public override Material GetTickBoxDefaultMaterial()
        {
            return tickBoxDefaultMaterial;
        }

        public override Material GetTickBoxClickedMaterial()
        {
            return tickBoxClickedMaterial;
        }

        public override Material GetTickBoxClickMaterial()
        {
            return tickBoxClickMaterial;
        }

        public override Material GetLeftRightTextSliderFillMaterial()
        {
            return leftRightSliderFillMaterial;
        }

        public override Material GetLeftArrowEdgeMaterial()
        {
            return leftArrowMaterial;
        }

        public override Material GetRightArrowEdgeMaterial()
        {
            return rightArrowMaterial;
        }

        public override Material GetUpArrowEdgeMaterial()
        {
            return upArrowMaterial;
        }

        public override Material GetDownArrowEdgeMaterial()
        {
            return downArrowMaterial;
        }

        public override Material GetScrollPaneBorderMaterial()
        {
            return scrollPaneEdgeMaterial;
        }

        public override Material GetTableSeperatorMaterial()
        {
            return tableEdgeMaterial;
        }

        public override Material GetWindowBorderMaterial()
        {
            return windowEdgeMaterial;
        }
    }
}
