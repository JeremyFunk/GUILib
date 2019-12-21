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
using GUILib.Logger;

namespace GUILib.GUI.GuiElements
{
    struct Vector2i
    {
        int x, y;

        public Vector2i(int val)
        {
            x = y = val;
        }
        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Table : GuiElement
    {
        private BorderedButton quad;

        private Material fillMaterial, edgeMaterial;

        private APixelConstraint[] tableRows, tableColumns;
        private int edgeSize;

        private List<Quad> rowBorders = new List<Quad>();
        private List<Quad> columnBorders = new List<Quad>();

        private Dictionary<Vector2i, Container> tableCells = new Dictionary<Vector2i, Container>();

        public Table(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, Material pFillMaterial = null, Material pEdgeMaterial = null, float zIndex = 0, int pEdgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            fillMaterial = pFillMaterial == null ? Theme.defaultTheme.GetTableFillMaterial() : pFillMaterial;
            edgeMaterial = pEdgeMaterial == null ? Theme.defaultTheme.GetTableEdgeMaterial() : pEdgeMaterial;
            edgeSize = pEdgeSize == -1 ? Theme.defaultTheme.GetTableEdgeSize() : pEdgeSize;

            quad = new BorderedButton(0, 0, 0, 0, "", false, -1, fillMaterial, edgeMaterial, 0, true, edgeSize);
            quad.generalConstraint = new FillConstraintGeneral();

            AddChild(quad);
        }

        public void SetCell(GuiElement element, int column, int row)
        {
            Vector2i key = new Vector2i(row, column);

            if (!tableCells.ContainsKey(key))
            {
                APixelConstraint x = null;
                APixelConstraint y = null;
                APixelConstraint w = null;
                APixelConstraint h = null;

                if (column == 0)
                    x = 0;
                else if (column >= tableColumns.Length)
                    x = tableColumns[tableColumns.Length - 1];
                else
                    x = tableColumns[column - 1];

                if (row == 0)
                    y = tableRows[0];
                else if (row >= tableRows.Length)
                    y = 0;
                else
                    y = tableRows[row - 1];

                if (column == 0)
                    w = tableColumns[0];
                else if (column >= tableColumns.Length)
                    w = 1f - tableColumns[tableColumns.Length - 1];
                else
                    w = tableColumns[column] - tableColumns[column - 1];

                if (row == 0)
                    h = 1f - tableRows[0];
                else if(row >= tableRows.Length)
                    h = tableRows[tableRows.Length - 1];
                else
                    h = (tableRows[row] - tableRows[row - 1]);

                Container c = new Container(x, y, w, h);

                tableCells.Add(key, c);
                AddChild(c);
            }


            tableCells[key].AddChild(element);
        }

        /*
         * Columns contains the position of each column. There will be one more column generated than specified that fills out the remaining space.
         */
        public void SetColumnCount(params APixelConstraint[] columns)
        {
            foreach (Quad quad in columnBorders)
                RemoveChild(quad);
            columnBorders.Clear();

            tableColumns = columns;

            foreach(APixelConstraint c in columns)
            {
                Quad quad = new Quad(edgeMaterial, c, 0, edgeSize, 1f);
                AddChild(quad);
                columnBorders.Add(quad);
            }
        }

        public void SetRowCount(params APixelConstraint[] rows)
        {
            foreach (Quad quad in rowBorders)
                RemoveChild(quad);
            rowBorders.Clear();

            tableRows = new APixelConstraint[rows.Length];

            int counter = 0;
            foreach (APixelConstraint c in rows)
            {
                tableRows[counter] = 1f - c;

                Quad quad = new Quad(edgeMaterial, 0, tableRows[counter], 1f, edgeSize);
                AddChild(quad);
                rowBorders.Add(quad);

                counter++;
            }
        }
    }
}
