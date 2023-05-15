//this empty line for UTF-8 BOM header
using System;
using UnityEngine;

namespace AlgorithmsDemo.DTS
{
    [Serializable]
    public struct RectAreaInt
    {
        public int xMin;
        public int yMin;
        public int xMax;
        public int yMax;

        public int width => xMax - xMin + 1;
        public int height => yMax - yMin + 1;

        public RectAreaInt(int xMin, int yMin, int width, int height)
        {
            this.xMin = xMin;
            this.yMin = yMin;
            this.xMax = xMin + width - 1;
            this.yMax = yMin + height - 1;
        }

        public void Expand(Vector2Int position, int padding = 0)
        {
            xMin = Math.Min(xMin, position.x - padding);
            yMin = Math.Min(yMin, position.y - padding);

            xMax = Math.Max(xMax, position.x + padding);
            yMax = Math.Max(yMax, position.y + padding);
        }

        public void Clamp(RectAreaInt limits)
        {
            xMin = Math.Max(xMin, limits.xMin);
            yMin = Math.Max(yMin, limits.yMin);

            xMax = Math.Min(xMax, limits.xMax);
            yMax = Math.Min(yMax, limits.yMax);
        }

        public bool Belongs(Vector2Int pos) => pos.x >= xMin && pos.x <= xMax && pos.y >= yMin && pos.y <= yMax;

        public static bool operator ==(RectAreaInt a, RectAreaInt b) => a.xMin == b.xMin && a.xMax == b.xMax && a.yMin == b.yMin && a.yMax == b.yMax;
        public static bool operator !=(RectAreaInt a, RectAreaInt b) => (a == b) == false;
        public override int GetHashCode() => (xMin, xMax, yMin, yMax).GetHashCode();
        public override bool Equals(object other) => (other is RectAreaInt) && (this == ((RectAreaInt)other));

        public override string ToString()
        {
            return $"xMin={xMin} xMax={xMax} zMin={yMin} zMax={yMax} width={width} height={height}";
        }
    }
}
