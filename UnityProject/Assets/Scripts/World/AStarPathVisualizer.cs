using System.Collections.Generic;
using AlgorithmsDemo.DTS;
using AlgorithmsDemo.Windows;
using UnityEngine;

namespace AlgorithmsDemo.World
{
    public class AStarPathVisualizer : MonoBehaviour
    {
        [SerializeField] private BuildPathButton buildPathButton;
        [SerializeField] private GameObject pathStepPrefab;
        [SerializeField] private float visualizeHeight;

        private readonly List<GameObject> destroyBeforeVisualize = new List<GameObject>();

        private void Awake()
        {
            buildPathButton.OnPathBuilt += OnPathBuilt;
        }

        private void OnPathBuilt(AStarPathBuilderResult result)
        {
            foreach (GameObject victim in destroyBeforeVisualize)
            {
                Destroy(victim);
            }

            destroyBeforeVisualize.Clear();

            VisualizePath(result.path);
        }

        private void VisualizePath(List<Vector2Int> path)
        {
            for (int i = 1; i < path.Count; i++)
            {
                VisualizeStep(path[i - 1], path[i]);
            }
        }

        private void VisualizeStep(Vector2Int from, Vector2Int to)
        {
            GameObject pathStepInstance = Instantiate(pathStepPrefab, transform);

            Vector3 fromV3 = new Vector3(from.x, visualizeHeight, from.y);
            Vector3 toV3 = new Vector3(to.x, visualizeHeight, to.y);
            pathStepInstance.transform.position = (fromV3 + toV3) * 0.5f;
            
            Vector3 localScale = pathStepInstance.transform.localScale;
            localScale.z = (fromV3 - toV3).magnitude;
            pathStepInstance.transform.localScale = localScale;
            
            pathStepInstance.transform.rotation = Quaternion.LookRotation(toV3 - fromV3, Vector3.up);

            destroyBeforeVisualize.Add(pathStepInstance);
        }
    }
}
