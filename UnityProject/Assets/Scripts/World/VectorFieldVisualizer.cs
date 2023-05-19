//this empty line for UTF-8 BOM header

using AlgorithmsDemo.DTS;
using UnityEngine;

namespace AlgorithmsDemo.World
{
    public class VectorFieldVisualizer : MonoBehaviour
    {
        [SerializeField] private VectorFieldSpawner vectorFieldSpawner;
        [SerializeField] private GameObject pathStepPrefab;
        [SerializeField] private float visualizeHeight;

        private void Awake()
        {
            vectorFieldSpawner.GetField().Done(vectorField =>
            {
                RectAreaInt workArea = vectorField.Area;

                int xMin = workArea.xMin;
                int xMax = workArea.xMax;
                int yMin = workArea.yMin;
                int yMax = workArea.yMax;

                for (int x = xMin; x <= xMax; x++)
                {
                    for (int y = yMin; y <= yMax; y++)
                    {
                        Vector2Int position = new Vector2Int(x, y);

                        Vector2Int direction = vectorField.GetDirectionToTarget(position);

                        if (direction != Vector2Int.zero)
                        {
                            VisualizeStep(position, position + direction);
                        }
                    }
                }
            });
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
        }
    }
}
