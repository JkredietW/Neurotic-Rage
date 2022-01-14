using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsToUnlock : MonoBehaviour
{
	public TextMeshProUGUI text;
	public GameObject[] tip;
	public void ResetAll()
	{
		for (int i = 0; i < tip.Length; i++)
		{
			tip[i].SetActive(false);
		}
	}
}
