using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garage : MonoBehaviour
{
	public bool isOpen;
	public Animator anim;
	public void ButtonPressed()
	{
		isOpen =! isOpen;

		anim.SetBool("Garage".Length, isOpen);
	}
}
