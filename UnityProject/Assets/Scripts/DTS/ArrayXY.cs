//this empty line for UTF-8 BOM header

using UnityEngine;

namespace AlgorithmsDemo.DTS
{
    public class ArrayXY<T>
    {
        public delegate T GetOutOfRangeCell(Vector2Int position);

        private readonly RectAreaInt area;
        private readonly T[,] elements;
        private readonly GetOutOfRangeCell getOutOfRangeCell;

        public ArrayXY(RectAreaInt area, T defaultValue, GetOutOfRangeCell getOutOfRangeCell)
        {
            this.area = area;
            this.elements = new T[area.width, area.height];
            this.getOutOfRangeCell = getOutOfRangeCell;

            ResetToValue(defaultValue);
        }

        public void ResetToValue(T value)
        {
            for (int x = 0; x < area.width; x++)
            {
                for (int y = 0; y < area.height; y++)
                {
                    elements[x, y] = value;
                }
            }
        }

        public T this[Vector2Int position]
        {
            get
            {
                if (ValidatePosition(position, out int indexX, out int indexY) == true)
                    return elements[indexX, indexY];
                else
                    return getOutOfRangeCell(position);
            }
            set
            {
                if (ValidatePosition(position, out int indexX, out int indexY) == true)
                    elements[indexX, indexY] = value;
            }
        }

        private bool ValidatePosition(Vector2Int position, out int indexX, out int indexY)
        {
            indexX = position.x - area.xMin;
            indexY = position.y - area.yMin;
            if (position.x < area.xMin) { return false; }
            if (position.x > area.xMax) { return false; }
            if (position.y < area.yMin) { return false; }
            if (position.y > area.yMax) { return false; }
            return true;
        }

        public bool IsPositionValid(Vector2Int position) => ValidatePosition(position, out _, out _);
    }
}
