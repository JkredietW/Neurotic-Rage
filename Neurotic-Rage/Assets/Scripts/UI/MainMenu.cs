using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public FadeToFromBlack fb;
    public GameObject playPanel;
    public GameObject settingsPanel;
    public float secondsToReopen;
    public float secondsTillCameraShake;
    public Animator garageDoor;
    public ScoreBord scorebord;
    public bool garage;
    public EventSystem eventSystem;
    public GameObject main;

    //input detection
    int lastInput; //0 = mouse, 1 = controller
    Vector2 lastpos;
    GameObject lastFirstSelected;

    void Start()
    {
        StartCoroutine(IEStart());
		for (int i = 0; i < scorebord.scores.Length; i++)
		{
            scorebord.scores[i].text.text = scorebord.scores[i].playerPrefName +" "+ PlayerPrefs.GetFloat(scorebord.scores[i].playerPrefName).ToString();
        }
        ChangeActivePanel(main);
    }
    private void Update()
    {
        var mouse = Mouse.current;
        if(mouse.position.ReadValue() != lastpos)
        {
            lastInput = 0;
        }
        lastpos = mouse.position.ReadValue();

        var allGamepads = Gamepad.all;
        if (lastInput == 0)
        {
            if (allGamepads.Count > 0)
            {
                Vector3 controllerInput = new Vector3(allGamepads[0].leftStick.x.ReadValue(), 0, allGamepads[0].leftStick.y.ReadValue());
                if (controllerInput.magnitude > 0.1)
                {
                    lastInput = 1;
                    eventSystem.SetSelectedGameObject(null);
                    eventSystem.SetSelectedGameObject(lastFirstSelected);
                }
            }
        }
    }
    public void BeginScene(int i)
	{
        StartCoroutine(StartScene(i));
	}
    public IEnumerator StartScene(int i)
	{

        yield return new WaitForSeconds(1.5f);
        IELoadScene(i);
	}
    public void LoadScene(int sceneToLoad)
    {
        //StartCoroutine(IELoadScene(sceneToLoad));
    }

    public void AddExtraScene(int extraSceneToLoad)
    {
        StartCoroutine(IEPlay(extraSceneToLoad));
    }

    public void ChangeActivePanel(GameObject panelToChange)
    {
        StartCoroutine(IEChangeActivePanel(panelToChange));
    }
    public void CloseAndOpenGarage()
    {
        StartCoroutine(CAOG());
    }

    public IEnumerator IEStart()
    {
        Time.timeScale = 1;
        garage = !garage;
        garageDoor.SetBool("Garage", garage);
        yield return new WaitForSeconds(secondsToReopen);
        fb.FadeFromBlack(1);
    }

    public IEnumerator CAOG()
    {
        garage = !garage;
        garageDoor.SetBool("Garage", garage);
        yield return new WaitForSeconds(secondsToReopen);
        garage = !garage;
        garageDoor.SetBool("Garage", garage);
        yield return new WaitForSeconds(secondsTillCameraShake);
        
        CameraShaker.Instance.ShakeOnce(4f, 10f, .1f, .1f);
        yield return new WaitForSeconds(.1f);
        CameraShaker.Instance.ShakeOnce(4f, 10f, .1f, .1f);
    }

    public IEnumerator IEPlay(int sceneToLoad)
    {
        garage = !garage;
        garageDoor.SetBool("Garage", garage);
        yield return new WaitForSeconds(secondsToReopen);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

    public IEnumerator IEChangeActivePanel(GameObject panelToChange)
    {
        yield return new WaitForSeconds(secondsToReopen);
        panelToChange.SetActive(!panelToChange.activeSelf);
        eventSystem.SetSelectedGameObject(null);
        lastFirstSelected = panelToChange.GetComponent<MenuPanel>().firstSelectedObject;
        eventSystem.SetSelectedGameObject(lastFirstSelected);
    }
    public void IELoadScene(int i)
	{
        SceneManager.LoadScene(i, LoadSceneMode.Single);
	}
	public void IEAddExtraScene(int i)
	{
		SceneManager.LoadScene(i, LoadSceneMode.Additive);
	}
    public void Quit()
	{
        Application.Quit();
    }
}
[System.Serializable]
public class ScoreBord
{
    public Score[] scores;
}
[System.Serializable]
public class Score
{
    public TextMeshProUGUI text;
    public string playerPrefName;
}