//this empty line for UTF-8 BOM header

using System;
using System.Collections.Generic;
using AlgorithmsDemo.Algoritms;
using AlgorithmsDemo.DTS;
using AlgorithmsDemo.Interfaces;
using AlgorithmsDemo.World;
using UnityEngine;
using UnityEngine.UI;

namespace AlgorithmsDemo.Windows
{
    public class BuildPathButton : MonoBehaviour
    {
        internal event Action<AStarPathBuilderResult> OnPathBuilt = result => { };

        [SerializeField] private Button button;

        private Main main;
        private IStartEndPointProvider startEndPointProvider;

        private void Awake()
        {
            main = GetComponentInParent<Main>();
            
            startEndPointProvider = main.WorldsInstance.GetComponentInChildren<IStartEndPointProvider>();
            startEndPointProvider.StartPoint.OnValueChanged += point => UpdatePath();
            startEndPointProvider.EndPoint.OnValueChanged += point => UpdatePath();

            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick() => UpdatePath();

        private void UpdatePath()
        {
            WorldForPathBuilder worldForPathBuilder = main.WorldsInstance.GetComponentInChildren<WorldForPathBuilder>();

            AStarPathBuilder pathBuilder = new AStarPathBuilder(worldForPathBuilder);

            AStarPathBuilderResult pathBuilderResult;

            List<Vector2Int> path = new List<Vector2Int>();

            pathBuilder.BuildPath(startEndPointProvider.StartPoint.Value, startEndPointProvider.EndPoint.Value, path);

            OnPathBuilt(pathBuilderResult);
        }
    }
}
