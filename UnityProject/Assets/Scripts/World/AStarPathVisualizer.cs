using System.Collections.Generic;
using AlgorithmsDemo.DTS;
using UnityEngine;

namespace AlgorithmsDemo.World
{
    public class AStarPathVisualizer : MonoBehaviour
    {
        [SerializeField] private FollowPath followPath;
        [SerializeField] private GameObject pathStepPrefab;
        [SerializeField] private float visualizeHeight;

        private readonly List<GameObject> destroyBeforeVisualize = new List<GameObject>();

        private void Awake()
        {
            followPath.OnPathUpdated += OnPathUpdated;
        }

        private void OnPathUpdated(IEnumerable<Vector2Int> path)
        {
            foreach (GameObject victim in destroyBeforeVisualize)
            {
                Destroy(victim);
            }

            destroyBeforeVisualize.Clear();

            VisualizePath(path);
        }

        private void VisualizePath(IEnumerable<Vector2Int> path)
        {
            Vector2Int? previousPoint = null;

            foreach (Vector2Int pointOnPath in path)
            {
                if (previousPoint.HasValue == true)
                {
                    VisualizeStep(previousPoint.Value, pointOnPath);
                }

                previousPoint = pointOnPath;
            }
        }

        private void VisualizeStep(Vector2Int from, Vector2Int to)
        {
            GameObject pathStepInstance = Instantiate(pathStepPrefab, transform);

            Vector3 fromV3 = from.ToVector3() + new Vector3(0, visualizeHeight, 0);
            Vector3 toV3 = to.ToVector3() + new Vector3(0, visualizeHeight, 0);
            pathStepInstance.transform.position = (fromV3 + toV3) * 0.5f;

            Vector3 localScale = pathStepInstance.transform.localScale;
            localScale.z = (fromV3 - toV3).magnitude;
            pathStepInstance.transform.localScale = localScale;

            pathStepInstance.transform.rotation = Quaternion.LookRotation(toV3 - fromV3, Vector3.up);

            destroyBeforeVisualize.Add(pathStepInstance);
        }
    }
}
