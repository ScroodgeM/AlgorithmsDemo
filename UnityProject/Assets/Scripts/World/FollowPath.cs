//this empty line for UTF-8 BOM header

using System;
using System.Collections.Generic;
using AlgorithmsDemo.Algoritms;
using AlgorithmsDemo.DTS;
using UnityEngine;

namespace AlgorithmsDemo.World
{
    public class FollowPath : MonoBehaviour
    {
        internal event Action<string> OnPathUpdated = processInfoMessage => { };

        [SerializeField] private GameObject worldRoot;
        [SerializeField] private float speed;
        [SerializeField] private int iterations;

        private readonly List<Vector2Int> currentPath = new List<Vector2Int>();
        private AStarPathBuilder pathBuilder;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                iterations++;

                Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                Vector3 clickPointOnGround = clickRay.origin - clickRay.direction / clickRay.direction.y * clickRay.origin.y;

                WorldForPathBuilder worldForPathBuilder = worldRoot.GetComponentInChildren<WorldForPathBuilder>();

                if (worldForPathBuilder != null)
                {
                    if (pathBuilder == null)
                    {
                        pathBuilder = new AStarPathBuilder(worldForPathBuilder);
                    }

                    currentPath.Clear();

                    pathBuilder.BuildPath(transform.position.ToVector2Int(), clickPointOnGround.ToVector2Int(), iterations, currentPath, out string processInfoMessage);

                    OnPathUpdated(processInfoMessage);

                    if (currentPath.Count > 0)
                    {
                        currentPath.RemoveAt(0);
                    }
                }
            }

            if (currentPath.Count > 0 && speed > 0f)
            {
                Vector3 myPosition = transform.position;
                Vector3 nextPoint = currentPath[0].ToVector3();
                Vector3 meToNextPoint = nextPoint - myPosition;
                float maxStep = speed * Time.deltaTime;
                if (meToNextPoint.sqrMagnitude > maxStep * maxStep)
                {
                    transform.position += meToNextPoint.normalized * maxStep;
                }
                else
                {
                    transform.position = nextPoint;
                    currentPath.RemoveAt(0);
                }
            }
        }

        public AStarPathBuilder GetPathBuilder() => pathBuilder;
    }
}
