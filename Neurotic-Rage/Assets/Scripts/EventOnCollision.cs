using UnityEngine;
using UnityEngine.Events;
public class EventOnCollision : MonoBehaviour
{
    public UnityEvent function;
	private bool triggerd;
	private void OnTriggerEnter(Collider other)
	{
		if (!triggerd)
		{
			TriggerEvent();
		}
	}
	public void TriggerEvent()
	{
		triggerd = true;
		function.Invoke();
		Destroy(gameObject);
	}
}