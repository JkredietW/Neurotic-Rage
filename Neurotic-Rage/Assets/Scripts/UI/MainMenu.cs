using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float secondsToReopen;
    public Animator garageDoor;
    public bool garage;

    void Start()
    {
        StartCoroutine(IEStart());
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

    public void ChangeUIPanels()
    {

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