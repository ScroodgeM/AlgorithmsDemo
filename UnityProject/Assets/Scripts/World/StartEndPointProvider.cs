//this empty line for UTF-8 BOM header

using UnityEngine;
using UnityTools.Runtime.StatefulEvent;
using UnityTools.UnityRuntime.StatefulEvent;

namespace AlgorithmsDemo.World
{
    public class StartEndPointProvider : MonoBehaviour
    {
        internal IStatefulEvent<Vector2Int> StartPoint => startPoint;
        internal IStatefulEvent<Vector2Int> EndPoint => endPoint;

        [SerializeField] private Transform startPointTransform;
        [SerializeField] private Transform endPointTransform;

        private readonly StatefulEventInt<Vector2Int> startPoint = StatefulEventForUnity.Create(Vector2Int.zero);
        private readonly StatefulEventInt<Vector2Int> endPoint = StatefulEventForUnity.Create(Vector2Int.zero);

        private void Update()
        {
            startPoint.Set(ToVector2Int(startPointTransform.position));
            endPoint.Set(ToVector2Int(endPointTransform.position));
        }

        private Vector2Int ToVector2Int(Vector3 vector3) => Vector2Int.RoundToInt(new Vector2(vector3.x, vector3.z));
    }
}
