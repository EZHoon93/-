



namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class SendVoidEvent : Action
    {
        public VoidEventChannelSO voidEventChannelSO;
        public override TaskStatus OnUpdate()
        {
            if (voidEventChannelSO == null)
                return TaskStatus.Failure;

            voidEventChannelSO.RaiseEvent();
            return TaskStatus.Success;
        }

       
    }
}
