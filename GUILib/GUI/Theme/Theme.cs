﻿using OpenTK;
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

        public abstract Material GetButtonFillMaterial();
        public abstract Material GetButtonHoverMaterial();
        public abstract Material GetButtonClickMaterial();

        public abstract Material GetLeftArrowMaterial();
        public abstract Material GetRightArrowMaterial();
        public abstract Material GetUpArrowMaterial();
        public abstract Material GetDownArrowMaterial();

        public abstract Material GetLeftRightTextSliderMaterial();

        public abstract Material GetFieldMaterial();
        public abstract float GetCursorTickRate();

        public abstract Material GetTextAreaMaterial();

        public abstract Material GetPanelMaterial();
        public abstract Material GetSeperatorMaterial();

        public abstract Material GetWindowBackgroundMaterial();
        public abstract Material GetWindowBorderMaterial();


        public abstract Material GetChoiceBoxMaterial();

        public abstract Material GetWindowTopBarMaterial();
        public abstract Material GetTabMaterial();

        public abstract Material GetTabActiveMaterial();
        public abstract Material GetTabHoveredMaterial();
        public abstract Material GetTabClickedMaterial();


        public abstract Material GetScrollPaneBorderMaterial();

        public abstract Material GetTableMaterial();
        public abstract Material GetSliderMaterial();
        public abstract Material GetSliderQuadMaterial();

        public abstract Material GetTableSeperatorMaterial();

        public abstract Material GetMouseInfoMaterial();
        public abstract Material GetScrollPaneMaterial();
        public abstract Material GetScrollPaneScrollBarBackgroundMaterial();
        public abstract Material GetScrollPaneScrollBarMaterial();

        public abstract Material GetTickBoxHoverMaterial();
        public abstract Material GetTickBoxDefaultMaterial();
        public abstract Material GetTickBoxClickedMaterial();
        public abstract Material GetTickBoxClickMaterial();

        public abstract int GetWindowTopBarSize();
        public abstract int GetTabHeight();
        public abstract int GetTabWidth();
        public abstract int GetScrollPaneScrollBarWidth();
        public abstract int GetInitialDropDownPadding();
        public abstract float GetButtonFontSize();
    }
}
