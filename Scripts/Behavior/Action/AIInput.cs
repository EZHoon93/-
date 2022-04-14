



using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class AIInput : Action
    {
        public NavMeshAgent navMeshAgent;
        public SharedVector3 storeMoveVector3;

        public override void OnAwake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public override TaskStatus OnUpdate()
        {
            if (navMeshAgent == null)
                return TaskStatus.Failure;
            storeMoveVector3.Value = navMeshAgent.velocity;
            return TaskStatus.Running;
        }

    }
}
