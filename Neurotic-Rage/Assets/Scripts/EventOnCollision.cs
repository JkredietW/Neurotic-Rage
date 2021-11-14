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
			if(other.tag==("Player"))
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