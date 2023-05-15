//this empty line for UTF-8 BOM header

using System;
using System.Collections.Generic;
using AlgorithmsDemo.Algoritms;
using AlgorithmsDemo.DTS;
using AlgorithmsDemo.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace AlgorithmsDemo.Windows
{
    public class BuildPathButton : MonoBehaviour
    {
        internal event Action<AStarPathBuilderResult> OnPathBuilt = result => { };

        [SerializeField] private Button button;
        [SerializeField] private GameObject worldContainer;
        [SerializeField] private GameObject startEndPointContainer;

        private IWorldForPathBuilder worldForPathBuilder;
        private IStartEndPointProvider startEndPointProvider;

        private void Awake()
        {
            worldForPathBuilder = worldContainer.GetComponentInChildren<IWorldForPathBuilder>();

            startEndPointProvider = startEndPointContainer.GetComponentInChildren<IStartEndPointProvider>();
            startEndPointProvider.StartPoint.OnValueChanged += point => UpdatePath();
            startEndPointProvider.EndPoint.OnValueChanged += point => UpdatePath();

            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick() => UpdatePath();

        private void UpdatePath()
        {
            AStarPathBuilder pathBuilder = new AStarPathBuilder(worldForPathBuilder);

            AStarPathBuilderResult pathBuilderResult;

            List<Vector2Int> path = new List<Vector2Int>();

            pathBuilder.BuildPath(startEndPointProvider.StartPoint.Value, startEndPointProvider.EndPoint.Value, path);

            OnPathBuilt(pathBuilderResult);
        }
    }
}
