using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterActable : MonoBehaviour
{
    protected PlayerMovement player;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>())
        {
            OnPlayerEnter(other.GetComponent<PlayerMovement>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            OnPlayerExit();
        }
    }
    public virtual void OnPlayerEnter(PlayerMovement _thisOne)
    {
        player = _thisOne;
    }
    public virtual void OnPlayerExit()
    {
        player = null;
    }
}
