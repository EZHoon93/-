
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [TaskCategory("My")]
    public class SendGameStateToServer : Action
    {
        //public CurrentGameStateVariableSO currentGameStateVariableSO;
        public GameState nextGameState;
        public bool isNextState;
        public GameStateSO gameStateSO;

 
        public override TaskStatus OnUpdate()
        {
            if (gameStateSO == null)
            {
                return TaskStatus.Failure;
            }
            gameStateSO.UpdateNewGameStateToServer(nextGameState);

            return TaskStatus.Success;
        }
    }
}