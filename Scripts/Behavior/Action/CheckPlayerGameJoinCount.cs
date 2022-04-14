
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class CheckPlayerGameJoinCount : Conditional
    {
        public enum Operation
        {
            LessThan,
            LessThanOrEqualTo,
            EqualTo,
            NotEqualTo,
            GreaterThanOrEqualTo,
            GreaterThan
        }
        public Operation operation;

        public IntVariableSO enterPlayerJoinCount;
        public int waitCount;

        public override TaskStatus OnUpdate()
        {
            switch (operation)
            {
                case Operation.LessThan:
                    return enterPlayerJoinCount.Value < waitCount ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.LessThanOrEqualTo:
                    return enterPlayerJoinCount.Value <= waitCount ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.EqualTo:
                    return enterPlayerJoinCount.Value == waitCount ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.NotEqualTo:
                    return enterPlayerJoinCount.Value != waitCount ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.GreaterThanOrEqualTo:
                    return enterPlayerJoinCount.Value >= waitCount ? TaskStatus.Success : TaskStatus.Failure;
                case Operation.GreaterThan:
                    return enterPlayerJoinCount.Value > waitCount ? TaskStatus.Success : TaskStatus.Failure;
            }
            return TaskStatus.Failure;
        }


    }
}