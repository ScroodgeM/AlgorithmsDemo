using AlgorithmsDemo.Algoritms;
using TMPro;
using UnityEngine;

namespace AlgorithmsDemo.Visualizers
{
    public class AStarPathCell : MonoBehaviour
    {
        [SerializeField] private TMP_Text textLabel;

        public void Init(AStarPathBuilder.Cell cell)
        {
            textLabel.text = cell.isReady ? $"<color=green>{cell.distFromStart}</color>+<color=red>{cell.distToEnd}</color>\n={cell.PathLength}" : "?";
        }
    }
}
