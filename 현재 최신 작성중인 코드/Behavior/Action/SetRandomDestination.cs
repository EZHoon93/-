using UnityEngine;
namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class SetRandomDestination : Action
    {

        public SharedFloat distance;

        public SharedVector3 resultValue;

        public override TaskStatus OnUpdate()
        {
            resultValue.Value = UtillGame.GetRandomPointOnNavMesh(Vector3.zero, distance.Value);


            return TaskStatus.Success;
        }
     
    }
}