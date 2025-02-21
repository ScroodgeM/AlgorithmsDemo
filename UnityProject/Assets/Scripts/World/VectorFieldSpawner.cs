//this empty line for UTF-8 BOM header

using System;
using AlgorithmsDemo.Algoritms;
using AlgorithmsDemo.DTS;
using UnityEngine;

namespace AlgorithmsDemo.World
{
    public class VectorFieldSpawner : MonoBehaviour
    {
        internal event Action<string> OnVectorFieldUpdated = processInfoMessage => { };
        internal VectorField VectorField => vectorField;

        [SerializeField] private GameObject worldRoot;
        [SerializeField] private FollowVectorField unitPrefab;
        [SerializeField] private Vector2Int target;
        [SerializeField] private int iterations;

        private WorldForPathBuilder worldForPathBuilder;
        private VectorField vectorField;

        private void Start()
        {
            worldForPathBuilder = worldRoot.GetComponentInChildren<WorldForPathBuilder>();
            if (worldForPathBuilder != null)
            {
                vectorField = new VectorField(worldForPathBuilder);

                Physics.SyncTransforms();
                vectorField.BuildField(target, iterations, out string processInfoMessage);
                OnVectorFieldUpdated(processInfoMessage);
            }
        }

        private void Update()
        {
            if (worldForPathBuilder != null)
            {
                Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 clickPointOnGround = clickRay.origin - clickRay.direction / clickRay.direction.y * clickRay.origin.y;
                Vector2Int clickPosition = clickPointOnGround.ToVector2Int();

                if (Input.GetMouseButtonDown(0) == true && clickPosition == target)
                {
                    iterations++;
                    vectorField.BuildField(target, iterations, out string processInfoMessage);
                    OnVectorFieldUpdated(processInfoMessage);
                }

                if (Input.GetMouseButton(0) == true && clickPosition != target)
                {
                    if (worldForPathBuilder.IsCellWalkable(clickPosition) == true)
                    {
                        Instantiate(unitPrefab, clickPointOnGround, Quaternion.identity).Init(vectorField);
                    }
                }
            }
        }
    }
}
