using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class CheckGameState : Conditional
    {
        public GameStateSO gameStateSO;
        public GameState checkGameState;
        public GameState[] checkGameStates;

        public override TaskStatus OnUpdate()
        {
            if (gameStateSO.State == checkGameState)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }

     
    }
}