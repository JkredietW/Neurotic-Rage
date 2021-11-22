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
    }
    public void IELoadScene(int i)
	{
        SceneManager.LoadScene(i, LoadSceneMode.Single);
    }
    public void IEAddExtraScene(int i)
	{
        SceneManager.LoadScene(i, LoadSceneMode.Additive);
    }
}