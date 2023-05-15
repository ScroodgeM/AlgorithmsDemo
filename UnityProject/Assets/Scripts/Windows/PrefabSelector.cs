//this empty line for UTF-8 BOM header

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AlgorithmsDemo.Windows
{
    public class PrefabSelector : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabsArray;
        [SerializeField] private Transform instanceParent;
        [SerializeField] private TMP_Dropdown prefabsDropdown;

        private GameObject selectedPrefabInstance;

        private void Awake()
        {
            prefabsDropdown.ClearOptions();
            prefabsDropdown.AddOptions(new List<string>(GetOptions()));

            prefabsDropdown.onValueChanged.AddListener(OnPrefabsDropdownValueChanged);

            IEnumerable<string> GetOptions()
            {
                yield return "NONE";

                foreach (GameObject prefab in prefabsArray)
                {
                    yield return prefab.name;
                }
            }
        }

        private void OnDestroy()
        {
            SpawnNewPrefab(null);
        }

        private void OnPrefabsDropdownValueChanged(int optionIndex)
        {
            SpawnNewPrefab(optionIndex > 0 ? prefabsArray[optionIndex - 1] : null);
        }

        private void SpawnNewPrefab(GameObject newPrefab)
        {
            if (selectedPrefabInstance != null)
            {
                Destroy(selectedPrefabInstance);
            }

            if (newPrefab != null)
            {
                selectedPrefabInstance = Instantiate(newPrefab, instanceParent);
            }
        }
    }
}
