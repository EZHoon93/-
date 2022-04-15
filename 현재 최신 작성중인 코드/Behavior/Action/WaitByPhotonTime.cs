


using Photon.Pun;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskIcon("{SkinColor}WaitIcon.png")]
    [TaskCategory("My")]
    public class WaitByPhotonTime : Action
    {
        //public CurrentGameStateVariableSO currentGameStateVariableSO;
        public SharedInt startServerTime;
        public SharedFloat waitTIme;

        public override TaskStatus OnUpdate()
        {
            
            if(startServerTime.Value + waitTIme.Value < PhotonNetwork.Time)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;

        }
    }
}