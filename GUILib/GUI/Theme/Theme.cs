using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.GUI
{
    abstract class Theme
    {
        public static readonly Theme defaultTheme = new DarkTheme();

        public abstract Material GetButtonEdgeMaterial();
        public abstract Material GetButtonFillMaterial();
        public abstract Material GetButtonHoverMaterial();
        public abstract Material GetButtonClickMaterial();

        public abstract Material GetFieldEdgeMaterial();
        public abstract Material GetFieldFillMaterial();

        public abstract Material GetPanelEdgeMaterial();
        public abstract Material GetPanelFillMaterial();
        public abstract Material GetPanelSeperatorMaterial();

        public abstract Material GetBrightHighlightMaterial();

        public abstract Material GetWindowBackgroundMaterial();
        public abstract Material GetWindowEdgeMaterial();
        public abstract Material GetWindowTopBarMaterial();
        public abstract Material GetTabFillMaterial();
        public abstract Material GetTabEdgeMaterial();
        public abstract Material GetTabActiveMaterial();
        public abstract Material GetTabHoveredMaterial();
        public abstract Material GetTabClickedMaterial();
        public abstract int GetWindowEdgeSize();
        public abstract int GetWindowTopBarSize();
        public abstract int GetTabHeight();
        public abstract int GetTabWidth();
        public abstract int GetTabEdgeSize();


        public abstract int GetButtonEdgeSize();
        public abstract float GetButtonFontSize();
    }
}
