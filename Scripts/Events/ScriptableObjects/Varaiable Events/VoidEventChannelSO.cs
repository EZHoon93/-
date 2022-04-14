using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>

[CreateAssetMenu(menuName = "Events/Void Event Channel" ,order =0) ]
public class VoidEventChannelSO : DescriptionBaseSO
{
	public UnityAction onEventRaised;
	
	public void RaiseEvent()
	{
		if (onEventRaised != null)
			onEventRaised.Invoke();
	}
}


