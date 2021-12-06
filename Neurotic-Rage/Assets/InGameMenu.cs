using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
	public bool menuActive;
	public GameObject inGameMenu;
	public void Menu()
	{
		menuActive = !menuActive;
		for (int i = 0; i < inGameMenu.transform.childCount; i++)
		{
			inGameMenu.transform.GetChild(i).transform.gameObject.SetActive(false);
		}
		inGameMenu.transform.GetChild(0).transform.gameObject.SetActive(false);
		inGameMenu.SetActive(menuActive);
	}
	public void SetTrue(GameObject panel)
	{
		panel.SetActive(true);
	}
	public void SetFalse(GameObject panel)
	{
		panel.SetActive(false);
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Menu();
		}
	}
}