using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class objectEventChannelSO : DescriptionBaseSO
{
	//public T ss;
	public UnityAction<object> onEventRaised;

	public void RaiseEvent(object value)
	{
		if (onEventRaised != null)
			onEventRaised.Invoke(value);
	}
}
