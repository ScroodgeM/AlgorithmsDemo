//this empty line for UTF-8 BOM header

using AlgorithmsDemo.Algoritms;
using AlgorithmsDemo.DTS;
using UnityEngine;

namespace AlgorithmsDemo.World
{
    public class FollowVectorField : MonoBehaviour
    {
        [SerializeField] private float speed;

        private VectorField vectorField;
        private Vector3? nextGoal;

        internal void Init(VectorField vectorField)
        {
            this.vectorField = vectorField;
        }

        private void Update()
        {
            if (vectorField != null)
            {
                if (nextGoal.HasValue == true)
                {
                    Vector3 meToNextPoint = nextGoal.Value - transform.position;
                    float maxStep = speed * Time.deltaTime;
                    if (meToNextPoint.sqrMagnitude > maxStep * maxStep)
                    {
                        transform.position += meToNextPoint.normalized * maxStep;
                    }
                    else
                    {
                        transform.position = nextGoal.Value;
                        nextGoal = null;
                    }
                }

                if (nextGoal.HasValue == false)
                {
                    Vector2Int myPositionAsVector2Int = transform.position.ToVector2Int();
                    Vector2Int nextGoalAsVector2Int = myPositionAsVector2Int + vectorField.GetDirectionToTarget(myPositionAsVector2Int);

                    if (myPositionAsVector2Int == nextGoalAsVector2Int)
                    {
                        // arrived
                        Destroy(gameObject);
                    }
                    else
                    {
                        Vector3 nextGoalAsVector3 = nextGoalAsVector2Int.ToVector3() + Random.onUnitSphere * 0.3f;
                        nextGoalAsVector3.y = 0f;
                        nextGoal = nextGoalAsVector3;
                    }
                }
            }
        }
    }
}
