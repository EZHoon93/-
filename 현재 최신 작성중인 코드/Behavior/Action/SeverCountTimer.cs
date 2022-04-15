


using Photon.Pun;

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskIcon("{SkinColor}WaitIcon.png")]
    [TaskCategory("My")]
    public class SeverCountTimer : Action
    {
        public SharedInt startServerTime;
        public SharedFloat waitTIme;
        public SharedFloat storeTime;

        public override TaskStatus OnUpdate()
        {

            storeTime.Value = waitTIme.Value - (PhotonNetwork.ServerTimestamp - startServerTime.Value) * .001f;
            if (storeTime.Value <= 0)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;

        }
    }
}