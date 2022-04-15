

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
    [TaskCategory("My")]
    public class ReciveVoidEvent : Conditional
    {
        public VoidEventChannelSO voidEventChannelSO;
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
                if(voidEventChannelSO != null)
                    voidEventChannelSO.onEventRaised += ReceivedEvent;
            }
        }
        public override TaskStatus OnUpdate()
        {
            return eventReceived ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnEnd()
        {
            if (eventReceived )
            {
                registered = false;
                if (voidEventChannelSO != null)
                    voidEventChannelSO.onEventRaised -= ReceivedEvent;
            }
            eventReceived = false;
        }

        public override void OnBehaviorComplete()
        {
            eventReceived = false;
            registered = false;
        }

        private void ReceivedEvent()
        {
            eventReceived = true;
        }
    }
}
