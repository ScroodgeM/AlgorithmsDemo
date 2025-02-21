using AlgorithmsDemo.DTS;
using UnityEngine;

namespace AlgorithmsDemo.Visualizers
{
    public class VectorFieldCell : MonoBehaviour
    {
        [SerializeField] private Transform directionMarker;
        [SerializeField] private GameObject finalPositionMarker;

        private void Awake()
        {
            directionMarker.gameObject.SetActive(false);
            finalPositionMarker.SetActive(false);
        }

        public void Init(Vector2Int directionToTarget)
        {
            directionMarker.gameObject.SetActive(directionToTarget.sqrMagnitude > 0);
            directionMarker.rotation = Quaternion.LookRotation(directionToTarget.ToVector3(), Vector3.up);
        }

        public void MarkAsFinalPosition(bool isFinalPosition)
        {
            finalPositionMarker.SetActive(isFinalPosition == true);
        }
    }
}
