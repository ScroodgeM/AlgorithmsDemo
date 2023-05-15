//this empty line for UTF-8 BOM header

using UnityEngine;
using UnityTools.Runtime.StatefulEvent;

namespace AlgorithmsDemo.Interfaces
{
    public interface IStartEndPointProvider
    {
        IStatefulEvent<Vector2Int> StartPoint { get; }
        IStatefulEvent<Vector2Int> EndPoint { get; }
    }
}
