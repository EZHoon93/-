
namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class LocalPlayerInput : Action
    {
        public InputReader inputReader;
        public SharedVector3 storeMoveVector3;

        public override TaskStatus OnUpdate()
        {
            if (inputReader == null)
                return TaskStatus.Failure;
            storeMoveVector3.Value = inputReader.MoveInputVector3;
            return TaskStatus.Running;
        }

    }
}