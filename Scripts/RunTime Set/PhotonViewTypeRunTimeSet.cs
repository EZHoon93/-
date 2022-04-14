
using System.Linq;

using Photon.Pun;

public class PhotonViewTypeRunTimeSet<T> : RuntimeSet<T> where T : MonoBehaviourPun
{
    public T GetMyController()
    {
        foreach(var item in Items)
        {
            if( PhotonNetwork.LocalPlayer.ActorNumber == item.photonView.ControllerActorNr && item.gameObject.IsValidAI() == false)
            {
                return item;
            }
        }
        return null;
    }

    public T GetItem(int viewID)
    {
        return Items.SingleOrDefault(x => x.photonView.ViewID == viewID);
    }

    public bool TryItem(int viewID , out T result)
    {
        result = GetItem(viewID);

        return result != null ? true: false;
    }
}
