using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSHopTut : MonoBehaviour
{
    public GameObject turnThisOff;
    private int amountOfpressed;
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
		{
            amountOfpressed++;
            if (amountOfpressed >= 3)
			{
                turnThisOff.SetActive(false);        
			}
		}
    }
}