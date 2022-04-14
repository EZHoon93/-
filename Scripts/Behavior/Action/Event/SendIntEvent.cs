

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class SendIntEvent : Action
    {
        public SharedInt sharedInt;
        public IntEventChannelSO intEventChannelSO;
        public override TaskStatus OnUpdate()
        {
            if (intEventChannelSO == null)
                return TaskStatus.Failure;

            intEventChannelSO.RaiseEvent(sharedInt.Value);
            return TaskStatus.Success;
        }


    }
}
