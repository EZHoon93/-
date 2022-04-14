
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class SendGameResultToServer : Conditional
    {
        public TeamRunTimeSet teamRunTimeSet;
        public PhotonEventChannelSO photonGameResultEventSO;

        public override TaskStatus OnUpdate()
        {
            if(teamRunTimeSet == null || photonGameResultEventSO == null)
            {
                return TaskStatus.Failure;
            }

            object isHideWin;
            if (teamRunTimeSet.HiderCount == 0)
            {
                isHideWin = true;
            }
            else
            {
                isHideWin = false;
            }
            photonGameResultEventSO.RaiseEventToServer(isHideWin);
            return TaskStatus.Success;
        }


    }
}