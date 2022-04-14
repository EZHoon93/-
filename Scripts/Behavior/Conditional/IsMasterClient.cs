using Photon.Pun;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class IsMasterClient : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            return PhotonNetwork.IsMasterClient ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
