
using FoW;

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/fogTeam Event Channel")]
public class FogTeamEventChannelSO : DescriptionBaseSO
{
	// Start is called before the first frame update
	public UnityAction<FogOfWarTeam> onEventRaised;

	public void RaiseEvent(FogOfWarTeam value)
	{
		if (onEventRaised != null)
			onEventRaised.Invoke(value);
	}
}
