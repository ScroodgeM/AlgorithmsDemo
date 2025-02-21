using System.Collections.Generic;
using AlgorithmsDemo.DTS;
using UnityEngine;
using UnityEngine.Pool;

namespace AlgorithmsDemo.World
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject worldRoot;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private int obstacleCount;

        private void Awake()
        {
            WorldForPathBuilder worldForPathBuilder = worldRoot.GetComponentInChildren<WorldForPathBuilder>();

            using var _ = HashSetPool<Vector2Int>.Get(out HashSet<Vector2Int> usedPositions);

            usedPositions.Add(Vector2Int.zero); // reserved position

            RectAreaInt worldArea = worldForPathBuilder.GetWorldSize();

            int xMin = worldArea.xMin;
            int yMin = worldArea.yMin;
            int xMax = worldArea.xMax;
            int yMax = worldArea.yMax;

            for (int i = 0; i < obstacleCount; i++)
            {
                int x = Random.Range(xMin, xMax);
                int y = Random.Range(yMin, yMax);
                Vector2Int randomPosition = new Vector2Int(x, y);
                if (usedPositions.Add(randomPosition) == false)
                {
                    i--;
                    continue;
                }

                Vector3 randomPositionV3 = randomPosition.ToVector3();
                randomPositionV3.y = obstaclePrefab.transform.position.y;
                Instantiate(obstaclePrefab, transform).transform.position = randomPositionV3;
            }
        }
    }
}
