using UnityEngine;

namespace AlgorithmsDemo.World
{
    public class SetCameraSizeOnAwake : MonoBehaviour
    {
        [SerializeField] private float orthographicSize;

        private void Awake()
        {
            Camera.main.orthographicSize = orthographicSize;
        }
    }
}
