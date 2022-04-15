
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class EntitiyAniamtorUpdate : Action
    {
        public string name;
        private EntityAnimator _entityAnimator;
        
        //private Animator _animator;
        public override void OnAwake()
        {
            _entityAnimator = GetComponent<EntityAnimator>();
            //_animator = GetComponent<Animator>();
        }


        public override TaskStatus OnUpdate()
        {
            if (_entityAnimator == null)
            {
                return TaskStatus.Failure;
            }
            _entityAnimator.UpdateTriggerAniamtor(name);
            return TaskStatus.Success;
        }
    }
}