using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Events/GameStateTime")]
public class GameStateTimerEventSO : DescriptionBaseSO
{
    public UnityAction<GameState, int> onEventRaised;


	public void RaiseEvent(GameState gameState , int time)
	{
		onEventRaised?.Invoke(gameState, time);
	}
}
