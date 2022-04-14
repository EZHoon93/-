
using UnityEngine;
namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class IsAI : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            return this.gameObject.CompareTag("AI") ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
