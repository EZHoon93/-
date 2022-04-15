
using Photon.Pun;

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class RecivePunInstanitateEvent : Conditional
    {
        public EachPhotonViewEventChannelSO eachPhotonViewEventChannelSO;

        private bool eventReceived = false;
        private bool registered = false;

  
        public override void OnStart()
        {
            if (!registered )
            {
                if(eachPhotonViewEventChannelSO != null)
                    eachPhotonViewEventChannelSO.onEventRaised += OnPhotonInstinate;

                registered = true;
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
                if (eachPhotonViewEventChannelSO != null)
                    eachPhotonViewEventChannelSO.onEventRaised -= OnPhotonInstinate;

                registered = false;
            }
            eventReceived = false;
        }


        private void OnPhotonInstinate(PhotonView photonView)
        {
            eventReceived = true;
        }

        public override void OnBehaviorComplete()
        {
            eventReceived = false;
            registered = false;
        }

    }
}
