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
        private Quad quad;

        private Material fillMaterial, seperatorMaterial;

        private APixelConstraint[] tableRows, tableColumns;
        private int edgeSize;

        private List<Quad> rowBorders = new List<Quad>();
        private List<Quad> columnBorders = new List<Quad>();

        private Dictionary<Vector2i, Container> tableCells = new Dictionary<Vector2i, Container>();
        private Dictionary<Vector2i, Quad> disabledCells = new Dictionary<Vector2i, Quad>();

        public Table(APixelConstraint x, APixelConstraint y, APixelConstraint width, APixelConstraint height, Material pFillMaterial = null, Material pEdgeMaterial = null, float zIndex = 0, int pEdgeSize = -1, bool visible = true) : base(width, height, x, y, visible, zIndex)
        {
            fillMaterial = pFillMaterial == null ? Theme.defaultTheme.GetTableFillMaterial() : pFillMaterial;
            seperatorMaterial = pEdgeMaterial == null ? Theme.defaultTheme.GetTableSeperatorMaterial() : pEdgeMaterial;

            edgeSize = fillMaterial.GetBorderSize();

            seperatorMaterial = new Material(new Vector4(1));

            quad = new Quad(0, 0, 0, 0, fillMaterial);
            quad.generalConstraint = new FillConstraintGeneral();

            AddChild(quad);
        }

        public void SetCellHoverText(string text, int column, int row, float delay = 0)
        {
            Vector2i key = new Vector2i(row, column);
            if (tableCells.ContainsKey(key))
            {
                tableCells[key].SetHoverDelay(delay);
                tableCells[key].SetHoverText(text);
            }
            else
            {
                AddContainerToCell(column, row); 
                tableCells[key].SetHoverDelay(delay);
                tableCells[key].SetHoverText(text);
            }
        }

        public void SetCell(GuiElement element, int column, int row)
        {
            Vector2i key = new Vector2i(row, column);

            if (!tableCells.ContainsKey(key))
            {
                AddContainerToCell(column, row);
            }

            tableCells[key].AddChild(element);
        }

        private void AddContainerToCell(int column, int row)
        {
            Vector2i key = new Vector2i(row, column);

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

            if (column == 0)
                w = tableColumns[0];
            else if (column >= tableColumns.Length)
                w = 1f - tableColumns[tableColumns.Length - 1];
            else
                w = tableColumns[column] - tableColumns[column - 1];



            if (row == 0)
                y = tableRows[0];
            else if (row >= tableRows.Length)
                y = 0;
            else
                y = tableRows[row];


            if (row == 0)
                h = 1f - tableRows[0];
            else if (row >= tableRows.Length)
                h = tableRows[tableRows.Length - 1];
            else
                h = (tableRows[row - 1] - tableRows[row]);

            Container c = new Container(x, y, w, h);
            c.widthConstraints.Add(new SubtractConstraint(column == tableColumns.Length ? edgeSize * 2 : edgeSize));
            c.xConstraints.Add(new AddConstraint(edgeSize));
            c.yConstraints.Add(new AddConstraint(edgeSize));
            c.heightConstraints.Add(new SubtractConstraint(row == 0 ? edgeSize * 2 : edgeSize));

            tableCells.Add(key, c);
            AddChild(c);

            Quad q = new Quad(x, y, w, h, new Material(new Vector4(0.2f, 0.2f, 0.2f, 0.8f)));
            q.widthConstraints.Add(new SubtractConstraint(column == tableColumns.Length ? edgeSize * 2 : edgeSize));
            q.xConstraints.Add(new AddConstraint(edgeSize));
            q.yConstraints.Add(new AddConstraint(edgeSize));
            q.heightConstraints.Add(new SubtractConstraint(row == 0 ? edgeSize * 2 : edgeSize));
            q.visible = false;

            disabledCells.Add(key, q);
            AddChild(q);
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
                Quad quad = new Quad(c, 0, edgeSize, 1f, seperatorMaterial);
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

                Quad quad = new Quad(0, tableRows[counter], 1f, edgeSize, seperatorMaterial);
                AddChild(quad);
                rowBorders.Add(quad);

                counter++;
            }
        }

        internal void DisableCell(int column, int row)
        {
            Vector2i key = new Vector2i(row, column);

            if (!disabledCells.ContainsKey(key))
            {
                AddContainerToCell(column, row);
            }
            disabledCells[key].visible = true;
        }

        internal void EnableCell(int column, int row)
        {
            Vector2i key = new Vector2i(row, column);

            if (!disabledCells.ContainsKey(key))
            {
                AddContainerToCell(column, row);
            }
            disabledCells[key].visible = false;
        }
    }
}
