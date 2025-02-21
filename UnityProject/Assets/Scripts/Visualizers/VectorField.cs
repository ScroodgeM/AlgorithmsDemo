//this empty line for UTF-8 BOM header

using System.Collections.Generic;
using AlgorithmsDemo.DTS;
using AlgorithmsDemo.World;
using UnityEngine;

namespace AlgorithmsDemo.Visualizers
{
    public class VectorField : MonoBehaviour
    {
        [SerializeField] private VectorFieldSpawner vectorFieldSpawner;
        [SerializeField] private VectorFieldCell pathCellPrefab;
        [SerializeField] private float visualizeHeight;

        private readonly List<GameObject> destroyBeforeVisualize = new List<GameObject>();

        private void Awake()
        {
            vectorFieldSpawner.GetField().Done(vectorField =>
            {
                foreach (GameObject victim in destroyBeforeVisualize)
                {
                    Destroy(victim);
                }

                destroyBeforeVisualize.Clear();

                VisualizeGrid(vectorField);
            });
        }

        private void VisualizeGrid(AlgorithmsDemo.Algoritms.VectorField vectorField)
        {
            RectAreaInt area = vectorField.Area;
            for (int x = area.xMin; x <= area.xMax; x++)
            {
                for (int y = area.yMin; y <= area.yMax; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    Vector2Int directionToTarget = vectorField.GetDirectionToTarget(position);
                    bool isFinalPosition = position == vectorField.GetDestinationTarget();
                    if (directionToTarget.sqrMagnitude > 0 || isFinalPosition)
                    {
                        VectorFieldCell pathCellInstance = Instantiate(pathCellPrefab, transform);
                        Vector3 positionV3 = position.ToVector3() + new Vector3(0, visualizeHeight, 0);
                        pathCellInstance.transform.position = positionV3;
                        pathCellInstance.Init(directionToTarget);
                        pathCellInstance.MarkAsFinalPosition(isFinalPosition);
                        destroyBeforeVisualize.Add(pathCellInstance.gameObject);
                    }
                }
            }
        }
    }
}
