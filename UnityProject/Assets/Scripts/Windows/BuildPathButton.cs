//this empty line for UTF-8 BOM header

using System;
using System.Collections.Generic;
using AlgorithmsDemo.Algoritms;
using AlgorithmsDemo.DTS;
using AlgorithmsDemo.World;
using UnityEngine;
using UnityEngine.UI;

namespace AlgorithmsDemo.Windows
{
    public class BuildPathButton : MonoBehaviour
    {
        internal event Action<AStarPathBuilderResult> OnPathBuilt = result => { };

        [SerializeField] private Button button;
        [SerializeField] private StartEndPointProvider startEndPointProvider;
        [SerializeField] private GameObject worldRoot;

        private void Awake()
        {
            startEndPointProvider.StartPoint.OnValueChanged += point => UpdatePath();
            startEndPointProvider.EndPoint.OnValueChanged += point => UpdatePath();

            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick() => UpdatePath();

        private void UpdatePath()
        {
            WorldForPathBuilder worldForPathBuilder = worldRoot.GetComponentInChildren<WorldForPathBuilder>();

            if (worldForPathBuilder != null)
            {
                AStarPathBuilder pathBuilder = new AStarPathBuilder(worldForPathBuilder);

                List<Vector2Int> path = new List<Vector2Int>();

                pathBuilder.BuildPath(startEndPointProvider.StartPoint.Value, startEndPointProvider.EndPoint.Value, path);

                AStarPathBuilderResult pathBuilderResult;
                pathBuilderResult.path = path;
                OnPathBuilt(pathBuilderResult);

                foreach (Vector2Int point in path)
                {
                    Debug.Log(point);
                }
            }
            else
            {
                Debug.LogWarning("create map first");
            }
        }
    }
}
