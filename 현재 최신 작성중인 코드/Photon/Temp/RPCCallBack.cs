using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Photon.Pun;

public interface IChildRpc
{

}
public class RPCCallBack : MonoBehaviourPun
{
    Dictionary<string, MethodInfo> _childMethodInfoDic = new Dictionary<string, MethodInfo>();
    Dictionary<string, object> _childMethodInfoDic2 = new Dictionary<string, object>();

    private void Awake()
    {
        foreach(var child in GetComponentsInChildren<IChildRpc>())
        {
            var list = child.GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(method => method.GetCustomAttribute<PunRPC>() != null)
            .ToList();

            foreach(var l in list)
            {
                _childMethodInfoDic.Add(l.Name, l);
                _childMethodInfoDic2.Add(l.Name, child);
            }
        }
    }


    public void RPC(string methodName, RpcTarget rpcTarget, params object[] parameters)
    {
       photonView.RPC("CallBack", rpcTarget, methodName, parameters);
    }

    [PunRPC]
    public void CallBack(string methodName, object[] datas, PhotonMessageInfo photonMessageInfo)
    {

        if (_childMethodInfoDic.ContainsKey(methodName))
        {
            var type = _childMethodInfoDic2[methodName];
            var parameters = _childMethodInfoDic[methodName].GetParameters();
            if (parameters.Length > 0)
            {
                if(parameters[parameters.Length - 1].ParameterType == typeof(PhotonMessageInfo))
                {
                    var data = datas.ToList();
                    data.Add(photonMessageInfo);
                    _childMethodInfoDic[methodName].Invoke(type, data.ToArray());
                }
                else
                {
                    _childMethodInfoDic[methodName].Invoke(type, datas);
                }
            }
        }
    }
}
