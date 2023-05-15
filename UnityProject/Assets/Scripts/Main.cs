//this empty line for UTF-8 BOM header

using UnityEngine;

namespace AlgorithmsDemo
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private GameObject worldsPrefab;
        [SerializeField] private GameObject hudsPrefab;

        internal GameObject WorldsInstance { get; private set; }
        internal GameObject HudsInstance { get; private set; }

        private void Awake()
        {
            WorldsInstance = Instantiate(worldsPrefab, transform);
            HudsInstance = Instantiate(hudsPrefab, transform);
        }
    }
}
