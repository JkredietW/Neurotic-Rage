using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public FadeToFromBlack ftb;
	private void Start()
	{
        ftb.FadeFromBlack(2);
    }
    public void StartMain(int i)
	{
        StartCoroutine(BeginMainScene(i));
	}
	public IEnumerator BeginMainScene(int i)
	{
        ftb.FadeToBlack(2);
        yield return new WaitForSeconds(1.5f);
        LoadScene(i);

    }
    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i, LoadSceneMode.Single);
    }
    public void AddExtraScene(int i)
    {
        SceneManager.LoadScene(i, LoadSceneMode.Additive);
    }
}
