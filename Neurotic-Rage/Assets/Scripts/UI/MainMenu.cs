using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playPanel;
    public GameObject settingsPanel;
    public float secondsToReopen;
    public Animator garageDoor;
    public bool garage;

    void Start()
    {
        StartCoroutine(IEStart());
    }

    public void Play(int sceneToLoad, int extraSceneToAdd)
    {
        StartCoroutine(IEPlay(sceneToLoad, extraSceneToAdd));
    }

    public void ChangeActivePanel(GameObject panelToChange)
    {
        StartCoroutine(IEChangeActivePanel(panelToChange));
    }

    void Update()
    {

    }

    public void CloseAndOpenGarage()
    {
        StartCoroutine(CAOG());
    }

    public IEnumerator IEStart()
    {
        yield return new WaitForSeconds(secondsToReopen);
        garage = !garage;
        garageDoor.SetBool("Garage", garage);
    }

    public IEnumerator CAOG()
    {
        garage = !garage;
        garageDoor.SetBool("Garage", garage);
        yield return new WaitForSeconds(secondsToReopen);
        garage = !garage;
        garageDoor.SetBool("Garage", garage);
        yield return new WaitForSeconds(.25f);
        
        CameraShaker.Instance.ShakeOnce(4f, 10f, .1f, .1f);
        yield return new WaitForSeconds(.1f);
        CameraShaker.Instance.ShakeOnce(4f, 10f, .1f, .1f);
    }

    public IEnumerator IEPlay(int sceneToLoad, int extraSceneToAdd)
    {
        garage = !garage;
        garageDoor.SetBool("Garage", garage);
        yield return new WaitForSeconds(secondsToReopen);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        SceneManager.LoadScene(extraSceneToAdd, LoadSceneMode.Additive);
    }

    public IEnumerator IEChangeActivePanel(GameObject panelToChange)
    {
        yield return new WaitForSeconds(secondsToReopen);
        panelToChange.SetActive(!panelToChange.activeSelf);
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