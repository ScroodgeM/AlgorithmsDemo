//this empty line for UTF-8 BOM header

using AlgorithmsDemo.Algoritms;
using AlgorithmsDemo.DTS;
using UnityEngine;
using UnityTools.Runtime.Promises;

namespace AlgorithmsDemo.World
{
    public class VectorFieldSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject worldRoot;
        [SerializeField] private FollowVectorField unitPrefab;
        [SerializeField] private Vector2Int target;

        private readonly Deferred<VectorField> vectorFieldDeferred = Deferred<VectorField>.GetFromPool();

        private WorldForPathBuilder worldForPathBuilder;

        private void Start()
        {
            worldForPathBuilder = worldRoot.GetComponentInChildren<WorldForPathBuilder>();

            if (worldForPathBuilder != null)
            {
                VectorField vectorField = new VectorField(worldForPathBuilder);

                vectorField.BuildField(target);

                vectorFieldDeferred.Resolve(vectorField);
            }
        }

        private void Update()
        {
            if (worldForPathBuilder != null && Input.GetMouseButton(0) == true)
            {
                Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                Vector3 clickPointOnGround = clickRay.origin - clickRay.direction / clickRay.direction.y * clickRay.origin.y;

                if (worldForPathBuilder.IsCellWalkable(clickPointOnGround.ToVector2Int()) == true)
                {
                    FollowVectorField unitInstance = Instantiate(unitPrefab, clickPointOnGround, Quaternion.identity);

                    vectorFieldDeferred.Done(vectorField => { unitInstance.Init(vectorField); });
                }
            }
        }

        internal IPromise<VectorField> GetField() => vectorFieldDeferred;
    }
}
