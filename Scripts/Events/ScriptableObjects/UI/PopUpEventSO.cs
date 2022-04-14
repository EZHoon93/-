using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Events/PopUp Event Channel", order = 0)]
public class PopUpEventSO : DescriptionBaseSO
{
	public delegate GameObject PopAction(UIPopUpSO uIPopUpSO) ;

	public PopAction onEventRaised;
	//public UnityAction<UIPopUpSO> onEventRaised;

	public GameObject RaiseEvent(UIPopUpSO uIPopUpSO)
	{
		if (onEventRaised != null)
        {
			return onEventRaised.Invoke(uIPopUpSO);
		}
		return null;
	}
}
