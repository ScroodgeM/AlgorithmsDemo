using AlgorithmsDemo.Algoritms;
using TMPro;
using UnityEngine;

namespace AlgorithmsDemo.Visualizers
{
    public class AStarPathCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text textLabel;
        [SerializeField] private GameObject finalPositionMarker;

        private void Awake()
        {
            textLabel.gameObject.SetActive(false);
            finalPositionMarker.SetActive(false);
        }

        public void Init(AStarPathBuilder.Cell cell)
        {
            textLabel.gameObject.SetActive(true);
            textLabel.text = cell.isReady ? $"<color=green>{cell.distFromStart}</color>+<color=red>{cell.distToEnd}</color>\n={cell.PathLength}" : "?";
        }

        public void MarkAsFinalPosition(bool isFinalPosition)
        {
            finalPositionMarker.SetActive(isFinalPosition == true);
        }
    }
}
