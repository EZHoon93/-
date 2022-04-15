
using BehaviorDesigner.Runtime;

using Photon.Pun;

using UnityEngine;
using UnityEngine.AI;

public class AIPlayerController : MonoBehaviourPun
{
    NavMeshAgent _navMeshAgent;
    BehaviorTree _behaviorTree;
    PlayerController _playerController;
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _behaviorTree = GetComponent<BehaviorTree>();
        _playerController = GetComponent<PlayerController>();

        
    }
    private void OnEnable()
    {
        bool isMine = photonView.IsMine;

        _navMeshAgent.enabled = isMine;
        //_behaviorTree.enabled = isMine;
        if (isMine)
        {
            //Test();

            Invoke("Test", 1.0f);
        
        }
    }
    
    void Test()
    {
        var dest = UtillGame.GetRandomPointOnNavMesh(Vector3.zero, 20);
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(dest);
    }

    void PhotonOnDestroy(PhotonView photonView)
    {

    }



}
