using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/KillNotify")]
public class NotifyKillInfoEventSO : DescriptionBaseSO
{
    //public delegate void KillInfo(string d, string s );
    public UnityAction<string, string> onEventRaised;

	public void RaiseEvent(string killInfo, string deathInfo)
	{
		if (onEventRaised != null)
			onEventRaised.Invoke(killInfo, deathInfo);
	}

}
