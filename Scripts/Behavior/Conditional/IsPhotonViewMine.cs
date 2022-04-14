
using Photon.Pun;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class IsPhotonViewMine : Conditional
    {
        PhotonView _photonView;
        public override void OnAwake()
        {
            _photonView = GetComponent<PhotonView>();
        }
        public override TaskStatus OnUpdate()
        {
            if (_photonView == null)
                return TaskStatus.Failure;

            return _photonView.IsMine ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
