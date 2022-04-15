
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class EntityMove : Action 
    {
        [SerializeField] private SharedFloat _moveSpeed;
        [SerializeField] private SharedVector3 _inputVector3;

        private NavMeshAgent _agent;
        private EntityAnimator _entityAnimator;
        //private Animator _animator;
        public override void OnAwake()
        {
            _agent = this.gameObject.GetOrAddComponent<NavMeshAgent>();
            _entityAnimator = GetComponent<EntityAnimator>();
            //_animator = GetComponent<Animator>();
        }
      

        public override TaskStatus OnUpdate()
        {
            _agent.Move(_inputVector3.Value.normalized * Time.deltaTime * _moveSpeed.Value);
            //_animator.SetFloat("Speed", _inputVector3.Value.sqrMagnitude);
            if (_entityAnimator)
            {
                _entityAnimator.UpdateMoveAnimator(_inputVector3.Value.sqrMagnitude);
            }
            return TaskStatus.Running;
        }
    }
}