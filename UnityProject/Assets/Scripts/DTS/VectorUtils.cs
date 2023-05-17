using UnityEngine;

namespace AlgorithmsDemo.DTS
{
    public static class VectorUtils
    {
        public static Vector2Int ToVector2Int(this Vector3 vector3)
        {
            return Vector2Int.RoundToInt(new Vector2(vector3.x, vector3.z));
        }

        public static Vector3 ToVector3(this Vector2Int vector2Int)
        {
            return new Vector3(vector2Int.x, 0f, vector2Int.y);
        }
    }
}
