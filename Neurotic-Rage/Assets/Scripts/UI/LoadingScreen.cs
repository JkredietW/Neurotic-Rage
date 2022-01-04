using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider slider;
    public GameObject loadingScreen;
    public TextMeshProUGUI text;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI adviceText;
    public string[] advice;
    private bool active;
    private int currentLine,amountDots;
    private AsyncOperation async;
    private string dots;
	private void Update()
	{
		if (active)
		{
            StartCoroutine(LevelCoroutine());
        }
	}
	public void PickNextLine()
    {
        currentLine++;
        if (currentLine >= advice.Length)
        {
            currentLine = 0;
        }
        ShowNextLine();
    }
    public IEnumerator AddDot()
	{
        amountDots++;
		if (amountDots==1)
		{
            dots = ".";
        }
		else if (amountDots == 2)
		{
            dots = "..";
        }
        else if (amountDots == 3)
        {
            dots = "...";
        }
        else
		{
            amountDots = 0;
            loadingText.text = "Loading";
        }
        loadingText.text += dots;
        yield return new WaitForSeconds(0.25f);
        AddNewDot();
    }
    public void AddNewDot()
	{
        StartCoroutine(AddDot());
	}
    public void ShowNextLine()
    {
        adviceText.text = advice[currentLine].ToString();
        Invoke("PickNextLine", 2.5f);
    }
    public void CheckForTuturialPlayed(int i)
	{
        string playerprefTut = PlayerPrefs.GetString("Tuturial");
		if (playerprefTut == "false")
		{
            ChargementScene(i-1);
            print("koe");
        }
		else
		{
            ChargementScene(i);
            print("koekje");
        }

    }
    public void ChargementScene(int i)
    {
        active = true;
        loadingScreen.SetActive(true);
        async = SceneManager.LoadSceneAsync(i);
        async.allowSceneActivation = false;
        PickNextLine();
        AddNewDot();
    }
    IEnumerator LevelCoroutine()
    {
        float pourcentage = 0;
        while (!async.isDone)
        {
            if (async.progress < 0.9f)
            {
                slider.value = async.progress / 0.9f;
                pourcentage = async.progress * 100;
                text.text = (int)pourcentage + "%";
            }
            else
            {
                slider.value = async.progress / 0.9f;
                pourcentage = (async.progress / 0.9f) * 100;
                text.text = (int)pourcentage + "%";
                async.allowSceneActivation = true;
            }

            yield return null;
        }


        yield return async;

    }
}