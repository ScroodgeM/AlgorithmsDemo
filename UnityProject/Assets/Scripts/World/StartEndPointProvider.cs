using UnityEngine;
using UnityTools.Runtime.StatefulEvent;
using UnityTools.UnityRuntime.StatefulEvent;

namespace AlgorithmsDemo.World
{
    public class StartEndPointProvider
    {
        internal IStatefulEvent<Vector2Int> StartPoint => startPoint;
        internal IStatefulEvent<Vector2Int> EndPoint => endPoint;

        private readonly StatefulEventInt<Vector2Int> startPoint = StatefulEventForUnity.Create(Vector2Int.zero);
        private readonly StatefulEventInt<Vector2Int> endPoint = StatefulEventForUnity.Create(Vector2Int.zero);
    }
}
