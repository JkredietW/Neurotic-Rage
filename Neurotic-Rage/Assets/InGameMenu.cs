using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
	public bool menuActive;
	public GameObject inGameMenu;
	public GameObject pauseMenu;
	public Texture2D cursorTexture;
	public Texture2D corshair;
	public void Menu()
	{
		menuActive = !menuActive;
		if (menuActive)
		{
			Time.timeScale = 0;
			inGameMenu.SetActive(false);
			pauseMenu.SetActive(true);
			Vector2 newpost = Vector2.zero;
			if (cursorTexture != null)
			{
				newpost = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
			}
			Cursor.SetCursor(cursorTexture, newpost, CursorMode.ForceSoftware);
		}
		else
		{
			Time.timeScale = 1;
			inGameMenu.SetActive(true);
			pauseMenu.SetActive(false);
			Vector2 newpost = Vector2.zero;
			if (corshair != null)
			{
				newpost = new Vector2(corshair.width / 2, corshair.height / 2);
			}
			Cursor.SetCursor(corshair, newpost, CursorMode.ForceSoftware);
		}
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
	public void ExitGame()
	{
		Application.Quit();
	}
	public void LoadScene(int i)
	{
		SceneManager.LoadScene(i);
	}
}