


namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class HasInputObject : Conditional
    {
        public SharedInt fireCode;
        public SharedInputControllerObject  reusltInputControllerObject;
        PlayerInventory _playerInventory;

        public override void OnAwake()
        {
            _playerInventory = GetComponent<PlayerInventory>();
        }

        public override TaskStatus OnUpdate()
        {
            //reusltInputControllerObject .Value = _playerInventory.CheckInputObject(fireCode.Value);
            return reusltInputControllerObject.Value == null ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
