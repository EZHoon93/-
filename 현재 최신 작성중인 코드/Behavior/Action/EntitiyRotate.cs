
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class EntityRotate : Action
    {
        public SharedFloat rotateSpeed;
        public SharedVector3 inputVector3;
        public SharedBool isImmediately;



        public override TaskStatus OnUpdate()
        {
            if (isImmediately.Value)
            {
                Quaternion rot = Quaternion.LookRotation(inputVector3.Value);
                this.transform.rotation = rot;
                return TaskStatus.Success;
            }
            if (inputVector3.Value.sqrMagnitude == 0)
            {
                return TaskStatus.Running;
            }
            
            Quaternion newRotation = Quaternion.LookRotation(inputVector3.Value);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, rotateSpeed.Value * Time.deltaTime);
            return TaskStatus.Running;
        }
    }
}