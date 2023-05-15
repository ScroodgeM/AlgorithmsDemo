//this empty line for UTF-8 BOM header

using AlgorithmsDemo.DTS;
using UnityEngine;

namespace AlgorithmsDemo.Interfaces
{
    public interface IWorldForPathBuilder
    {
        RectAreaInt GetWorldSize();
        bool IsCellWalkable(Vector2Int worldPosition);
    }
}
