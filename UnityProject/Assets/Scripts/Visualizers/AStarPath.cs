//this empty line for UTF-8 BOM header

using System.Collections.Generic;
using AlgorithmsDemo.Algoritms;
using AlgorithmsDemo.World;
using AlgorithmsDemo.DTS;
using TMPro;
using UnityEngine;

namespace AlgorithmsDemo.Visualizers
{
    public class AStarPath : MonoBehaviour
    {
        [SerializeField] private FollowPath followPath;
        [SerializeField] private TMP_Text globalInfoLabel;
        [SerializeField] private GameObject pathStepPrefab;
        [SerializeField] private AStarPathCell pathCellPrefab;
        [SerializeField] private float visualizeHeight;

        private readonly List<GameObject> destroyBeforeVisualize = new List<GameObject>();

        private void Awake()
        {
            followPath.OnPathUpdated += OnPathUpdated;
        }

        private void OnPathUpdated()
        {
            foreach (GameObject victim in destroyBeforeVisualize)
            {
                Destroy(victim);
            }

            destroyBeforeVisualize.Clear();

            globalInfoLabel.text = $"Expected path length: {followPath.GetPathBuilder().GetExpectedPathLength()}";
            VisualizePath(followPath.GetPathBuilder());
            VisualizeGrid(followPath.GetPathBuilder());
        }

        private void VisualizeGrid(AStarPathBuilder pathBuilder)
        {
            RectAreaInt area = pathBuilder.GetArea();
            for (int x = area.xMin; x <= area.xMax; x++)
            {
                for (int y = area.yMin; y <= area.yMax; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    AStarPathBuilder.Cell cell = pathBuilder.GetCell(position);
                    if (cell.isReady == true)
                    {
                        AStarPathCell pathCellInstance = Instantiate(pathCellPrefab, transform);
                        Vector3 positionV3 = position.ToVector3() + new Vector3(0, visualizeHeight, 0);
                        pathCellInstance.transform.position = positionV3;
                        pathCellInstance.Init(cell);
                        destroyBeforeVisualize.Add(pathCellInstance.gameObject);
                    }
                }
            }
        }

        private void VisualizePath(AStarPathBuilder pathBuilder)
        {
            Vector2Int? previousPoint = null;

            foreach (Vector2Int pointOnPath in pathBuilder.GetPath())
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
