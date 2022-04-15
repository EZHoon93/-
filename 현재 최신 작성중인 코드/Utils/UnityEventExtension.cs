using UnityEngine.Events;
using Photon.Pun;
using System;


public class UnityEventObject : UnityEvent<object>
{
//    public static UnityEventObject operator +(UnityEventObject a, UnityAction<object> b)
//    {
//        a.AddListener(b);
//        return a;
//    }
//    public static UnityEventObject operator -(UnityEventObject a, UnityAction<object> b)
//    {
//        a.RemoveListener(b);
//        return a;
//    }
//}

//public class UnityEventExtension
//{

//    [System.Serializable] public class floatEvent : UnityEvent<float> { }
 

//    [System.Serializable]
//    public class PhotonInstantiateEvent : UnityEvent<PhotonMessageInfo>
//    {
//        public static PhotonInstantiateEvent operator +(PhotonInstantiateEvent a, UnityAction<PhotonMessageInfo> b)
//        {
//            a.AddListener(b);
//            return a;
//        }
//        public static PhotonInstantiateEvent operator -(PhotonInstantiateEvent a, UnityAction<PhotonMessageInfo> b)
//        {
//            a.RemoveListener(b);
//            return a;
//        }
//    }

//    [System.Serializable]
//    public class PhotonDestoryEvent : UnityEvent<PhotonView>
//    {
//        public static PhotonDestoryEvent operator +(PhotonDestoryEvent a, UnityAction<PhotonView> b)
//        {
//            a.AddListener(b);
//            return a;
//        }
//        public static PhotonDestoryEvent operator -(PhotonDestoryEvent a, UnityAction<PhotonView> b)
//        {
//            a.RemoveListener(b);
//            return a;
//        }
//    }

}
