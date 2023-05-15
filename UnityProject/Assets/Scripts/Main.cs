using UnityEngine;

namespace AlgorithmsDemo
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabsToSpawnOnAwake;

        private void Awake()
        {
            foreach (GameObject prefab in prefabsToSpawnOnAwake)
            {
                Instantiate(prefab);
            }
        }
    }
}
