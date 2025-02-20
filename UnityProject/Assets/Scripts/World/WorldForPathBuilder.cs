//this empty line for UTF-8 BOM header

using AlgorithmsDemo.DTS;
using UnityEngine;

namespace AlgorithmsDemo.World
{
    public class WorldForPathBuilder : MonoBehaviour
    {
        [SerializeField] private RectAreaInt worldSize;
        [SerializeField] private LayerMask walkableMask;

        internal RectAreaInt GetWorldSize()
        {
            return worldSize;
        }

        internal bool IsCellWalkable(Vector2Int position)
        {
            Ray ray = new Ray(new Vector3(position.x, 5f, position.y), Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 10f) == false)
            {
                return false;
            }

            int hitAsLayerMask = 1 << hitInfo.collider.gameObject.layer;

            return ((int)walkableMask & hitAsLayerMask) != 0;
        }
    }
}
