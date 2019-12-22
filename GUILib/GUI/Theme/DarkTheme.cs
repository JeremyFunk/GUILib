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
        private readonly int buttonEdgeSize = 3, windowEdgeSize = 3, windowTopBarSize = 35, tabHeight = 35, tabWidth = 130, tabEdgeSize = 3, fieldEdgeSize = 3, tableEdgeSize = 3, mouseInfoEdgeSize = 3, scrollPaneEdgeSize = 3, tickBoxEdgeSize = 1;
        private readonly int scrollBarWidth = 20;

        private readonly int initialDropDownPadding = 10;

        private readonly float cursorTickRate = 0.5f;

        private readonly Material buttonFillMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 1f)), buttonEdgeMaterial = new Material(new Vector4(0.7f, 0.7f, 0.7f, 1f)),
            buttonHoverMaterial = new Material(new Vector4(0.4f, 0.4f, 0.4f, 1f)), buttonClickMaterial = new Material(new Vector4(0.5f, 0.5f, 0.5f, 1f));

        private readonly Material fieldFillMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 1f)), fieldEdgeMaterial = new Material(new Vector4(0.7f, 0.7f, 0.7f, 1f));

        private readonly Material tableFillMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 1f)), tableEdgeMaterial = new Material(new Vector4(0.7f, 0.7f, 0.7f, 1f));

        private readonly Material windowBackgroundMaterial = new Material(new Vector4(0.4f, 0.4f, 0.4f, 0.95f)), windowEdgeMaterial = new Material(new Vector4(0.7f, 0.7f, 0.7f, 1f)),
            windowTopBarMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 1f));

        private readonly Material tabFillMaterial = new Material(new Vector4(0.35f, 0.35f, 0.35f, 1f)), tabEdgeMaterial = new Material(new Vector4(0.75f, 0.75f, 0.75f, 1f)), 
            tabActiveMaterial = new Material(new Vector4(0.65f, 0.65f, 0.65f, 1f)), tabHoverMaterial = new Material(new Vector4(0.45f, 0.45f, 0.45f, 1f)), tabClickMaterial = new Material(new Vector4(0.55f, 0.55f, 0.55f, 1f));

        private readonly Material sliderMaterial = new Material(new Vector4(0.55f, 0.55f, 0.55f, 1f)), sliderQuadMaterial = new Material(new Vector4(0.85f, 0.85f, 0.85f, 1f));

        private readonly Material mouseInfoFillMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 1f)), mouseInfoEdgeMaterial = new Material(new Vector4(0.6f, 0.6f, 0.6f, 1f));
        private readonly Material scrollPaneFillMaterial = new Material(new Vector4(0.4f, 0.4f, 0.4f, 1f)), scrollPaneEdgeMaterial = new Material(new Vector4(0.8f, 0.8f, 0.8f, 1f)), 
            scrollBarBackgroundFillMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 1f)), scrollBarFillMaterial = new Material(new Vector4(0.5f, 0.5f, 0.5f, 1f)), scrollBarEdgeMaterial = new Material(new Vector4(0.8f, 0.8f, 0.8f, 1f));

        private readonly Material tickBoxDefaultMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 0.5f)), tickBoxHoverMaterial = new Material(new Vector4(0.6f, 0.6f, 0.6f, 0.7f)), tickBoxClickMaterial = new Material(new Vector4(0.8f, 0.8f, 0.8f, 0.9f)),
            tickBoxClickedMaterial = new Material(new Texture("TickBoxClicked.png")), tickBoxEdgeMaterial = new Material(new Vector4(0.8f, 0.8f, 0.8f, 1f));

        public override Material GetBrightHighlightMaterial()
        {
            throw new NotImplementedException();
        }

        public override Material GetButtonEdgeMaterial()
        {
            return buttonEdgeMaterial;
        }

        public override Material GetButtonHoverMaterial()
        {
            return buttonHoverMaterial;
        }

        public override Material GetButtonClickMaterial()
        {
            return buttonClickMaterial;
        }

        public override int GetButtonEdgeSize()
        {
            return buttonEdgeSize;
        }

        public override Material GetButtonFillMaterial()
        {
            return buttonFillMaterial;
        }

        public override Material GetFieldEdgeMaterial()
        {
            return fieldEdgeMaterial;
        }

        public override Material GetFieldFillMaterial()
        {
            return fieldFillMaterial;
        }

        public override Material GetPanelEdgeMaterial()
        {
            throw new NotImplementedException();
        }

        public override Material GetPanelFillMaterial()
        {
            throw new NotImplementedException();
        }

        public override Material GetPanelSeperatorMaterial()
        {
            throw new NotImplementedException();
        }

        public override Material GetWindowBackgroundMaterial()
        {
            return windowBackgroundMaterial;
        }

        public override int GetWindowEdgeSize()
        {
            return windowEdgeSize;
        }

        public override Material GetWindowEdgeMaterial()
        {
            return windowEdgeMaterial;
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

        public override Material GetTabEdgeMaterial()
        {
            return tabEdgeMaterial;
        }

        public override int GetTabEdgeSize()
        {
            return tabEdgeSize;
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

        public override int GetFieldEdgeSize()
        {
            return fieldEdgeSize;
        }

        public override float GetCursorTickRate()
        {
            return cursorTickRate;
        }

        public override Material GetTableFillMaterial()
        {
            return tableFillMaterial;
        }

        public override Material GetTableEdgeMaterial()
        {
            return tableEdgeMaterial;
        }

        public override int GetTableEdgeSize()
        {
            return tableEdgeSize;
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

        public override Material GetMouseInfoEdgeMaterial()
        {
            return mouseInfoEdgeMaterial;
        }

        public override int GetMouseInfoEdgeSize()
        {
            return mouseInfoEdgeSize;
        }

        public override Material GetScrollPaneFillMaterial()
        {
            return scrollPaneFillMaterial;
        }

        public override Material GetScrollPaneEdgeMaterial()
        {
            return scrollPaneEdgeMaterial;
        }

        public override int GetScrollPaneEdgeSize()
        {
            return scrollPaneEdgeSize;
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

        public override Material GetScrollPaneScrollBarEdgeMaterial()
        {
            return scrollBarEdgeMaterial;
        }

        public override Material GetTickBoxEdgeMaterial()
        {
            return tickBoxEdgeMaterial;
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

        public override int GetTickBoxEdgeSize()
        {
            return tickBoxEdgeSize;
        }
    }
}
