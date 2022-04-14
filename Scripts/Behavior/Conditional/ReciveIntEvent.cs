
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
    [TaskCategory("My")]
    public class ReciveIntEvent : Conditional
    {
        public IntEventChannelSO intEventChannelSO;
        public SharedInt _reciveEventResult;
        private bool eventReceived = false;
        private bool registered = false;

        public override void OnAwake()
        {
        }
        public override void OnStart()
        {
            if (!registered)
            {
                registered = true;
                if (intEventChannelSO != null)
                    intEventChannelSO.onEventRaised += ReceivedEvent;
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
                if (intEventChannelSO != null)
                    intEventChannelSO.onEventRaised -= ReceivedEvent;
            }
            eventReceived = false;
        }

        public override void OnBehaviorComplete()
        {
            eventReceived = false;
            registered = false;
        }

        private void ReceivedEvent(int value)
        {
            _reciveEventResult.Value = value;
            eventReceived = true;
        }
    }
}
