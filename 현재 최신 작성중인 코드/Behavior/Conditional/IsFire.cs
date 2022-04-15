
using UnityEngine;
namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    [TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
    public class IsFire : Conditional
    {
        public InputReader inputReader;
        public SharedInt sharedFireCode;
        public SharedVector3 sharedInputVector;


        private bool eventReceived = false;
        private bool registered = false;
    
        public override void OnStart()
        {
            if (!registered)
            {
                inputReader.onInputFireEvent += OnReciveUp;
                registered = true;
            }
        }
        public override TaskStatus OnUpdate()
        {
            return eventReceived ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            if (eventReceived)
            {
                inputReader.onInputFireEvent -= OnReciveUp;
                registered = false;
            }
            eventReceived = false;
        }

   
        private void OnReciveUp(int fireCode , Vector3 inputVector)
        {
            sharedFireCode.Value = fireCode;
            sharedInputVector.Value = inputVector;
            eventReceived = true;
        }

        public override void OnBehaviorComplete()
        {
            eventReceived = false;
            registered = false;
        }
    }
}