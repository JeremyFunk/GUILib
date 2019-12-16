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
        private readonly int buttonEdgeSize = 3, windowEdgeSize = 3, windowTopBarSize = 35, tabHeight = 35, tabWidth = 130, tabEdgeSize = 3;

        private readonly Material buttonFillMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 1f)), buttonEdgeMaterial = new Material(new Vector4(0.7f, 0.7f, 0.7f, 1f));
        private readonly Material windowBackgroundMaterial = new Material(new Vector4(0.4f, 0.4f, 0.4f, 0.95f)), windowEdgeMaterial = new Material(new Vector4(0.7f, 0.7f, 0.7f, 1f)),
            windowTopBarMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 1f));
        private readonly Material tabFillMaterial = new Material(new Vector4(0.35f, 0.35f, 0.35f, 1f)), tabEdgeMaterial = new Material(new Vector4(0.75f, 0.75f, 0.75f, 1f));

        public override Material GetBrightHighlightMaterial()
        {
            throw new NotImplementedException();
        }

        public override Material GetButtonEdgeMaterial()
        {
            return buttonEdgeMaterial;
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
            throw new NotImplementedException();
        }

        public override Material GetFieldFillMaterial()
        {
            throw new NotImplementedException();
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
    }
}
