
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
    [TaskCategory("My")]
    public class ReciveEachVector3Event : Conditional
    {
        public EachVector3EventChnnelSO eachVector3EventSO;
        public SharedVector3 _reciveEventResult;
        private bool eventReceived = false;
        private bool registered = false;

        PlayerController _playerController;
        public override void OnAwake()
        {
            _playerController = GetComponent<PlayerController>();
        }
        public override void OnStart()
        {
            if (!registered)
            {
                registered = true;
                if (eachVector3EventSO != null)
                    eachVector3EventSO.onEventRaised += ReceivedEvent;
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
                registered = false;
                if (eachVector3EventSO != null)
                    eachVector3EventSO.onEventRaised -= ReceivedEvent;

                //eachVector3EventSO.UnRegisterListener(_playerController.ViewID(), ReceivedEvent);
            }
            eventReceived = false;
        }

        public override void OnBehaviorComplete()
        {
            eventReceived = false;
            registered = false;
        }

        private void ReceivedEvent(Vector3 value)
        {
            Debug.Log("이벤트 받음 ?..!!!");
            _reciveEventResult.Value = value;
            eventReceived = true;
        }
    }
}
