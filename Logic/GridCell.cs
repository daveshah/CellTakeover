﻿namespace Logic
{
    public class GridCell
    {
        public int CellIndex { get; }
        public bool OutOfGrid { get; set; }
        public bool Empty { get; set; }
        public bool OrganicCell { get; set; }
        public bool Dead { get; set; }

        /// <summary>
        /// If the cell is going to be grown from an existing cell or if it was grown from an existing cell, this will be set
        /// </summary>
        public RelativePosition RelativePosition { get; set; }

        public GridCell(int cellIndex, bool outOfGrid, bool empty, bool organicCell, bool deadCell, 
            RelativePosition relativePosition = RelativePosition.NotApplicable)
        {
            CellIndex = cellIndex;
            OutOfGrid = outOfGrid;
            Empty = empty;
            OrganicCell = organicCell;
            Dead = deadCell;
            RelativePosition = relativePosition;
        }

        public static GridCell MakeOutOfGridCell(int cellIndex, RelativePosition relativePosition)
        {
            return new GridCell(cellIndex, true, false, false, false, relativePosition);
        }

        public static GridCell MakeEmptyCell(int cellIndex, RelativePosition relativePosition)
        {
            return new GridCell(cellIndex, false, true, false, false, relativePosition);
        }
    }
}