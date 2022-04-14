using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolGameObjectEvent : UnityEvent<bool, GameObject> { }
public class ZoneTriggerController : MonoBehaviour
{
	//[SerializeField] private BoolGameObjectEvent _enterTriggerZone = default;
	[SerializeField] private GameObject _enterObject;
	[SerializeField] private LayerMask _layers = default;

	private void OnTriggerEnter(Collider other)
	{
		if ((1 << other.gameObject.layer & _layers) != 0)
		{
			//_enterTriggerZone.Invoke(true, other.gameObject);
			var triggeController = other.GetComponent<ITriggerController>();
            if (triggeController != null)
            {
				triggeController.OnTriggerChangeDetected(true, _enterObject);
            }
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ((1 << other.gameObject.layer & _layers) != 0)
		{
			var triggeController = other.GetComponent<ITriggerController>();
			if (triggeController != null)
			{
				triggeController.OnTriggerChangeDetected(false, _enterObject);
			}
		}
	}
}
