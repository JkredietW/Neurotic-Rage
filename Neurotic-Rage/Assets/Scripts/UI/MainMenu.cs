using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}