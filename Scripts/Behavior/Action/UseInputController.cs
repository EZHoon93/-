

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class UseInputController : Action
    {
        public SharedVector3 sharedInputVector3;
        public SharedInt fireCode;

        PlayerInventory _playerInventory;
        
        public override void OnAwake()
        {
            _playerInventory = GetComponent<PlayerInventory>();
        }

        public override TaskStatus OnUpdate()
        {
            var inputVector3 = sharedInputVector3.Value;
            if(inputVector3.sqrMagnitude ==0 || _playerInventory == null)
            {
                return TaskStatus.Failure;
            }
            var inputController = _playerInventory.GetInputController(fireCode.Value);
            if(inputController == null)
            {
                return TaskStatus.Failure;
            }

            inputController.Fire(inputVector3);
            return TaskStatus.Success;
        }

    }
}