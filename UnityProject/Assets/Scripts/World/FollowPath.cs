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
        internal event Action<IEnumerable<Vector2Int>> OnPathUpdated = _ => { };

        [SerializeField] private GameObject worldRoot;
        [SerializeField] private float speed;

        private readonly List<Vector2Int> currentPath = new List<Vector2Int>();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                Vector3 clickPointOnGround = clickRay.origin - clickRay.direction / clickRay.direction.y * clickRay.origin.y;

                WorldForPathBuilder worldForPathBuilder = worldRoot.GetComponentInChildren<WorldForPathBuilder>();

                if (worldForPathBuilder != null)
                {
                    AStarPathBuilder pathBuilder = new AStarPathBuilder(worldForPathBuilder);

                    currentPath.Clear();

                    pathBuilder.BuildPath(transform.position.ToVector2Int(), clickPointOnGround.ToVector2Int(), currentPath);

                    OnPathUpdated(currentPath);

                    if (currentPath.Count > 0)
                    {
                        currentPath.RemoveAt(0);
                    }
                }
            }

            if (currentPath.Count > 0)
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
    }
}
