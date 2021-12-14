using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private int tutorialProgression;
    [SerializeField] private bool controllerConnected;
    public GameObject[] pcTutorialArray;
    public GameObject[] controllerTutorialArray;
    public GameObject helicopterTrigger;
    public Animator stage1,stage2,stage3,stage4;
    void Start()
    {
        foreach(GameObject tutorialText in pcTutorialArray)
        {
            tutorialText.gameObject.SetActive(false);
        }
        foreach(GameObject tutorialText in controllerTutorialArray)
        {
            tutorialText.gameObject.SetActive(false);
        }
        checkForController();
        if (controllerConnected == false)
        {
            pcTutorialArray[tutorialProgression].SetActive(true);
        }
        else
        {
            controllerTutorialArray[tutorialProgression].SetActive(true);
        }
    }

    public void Update()
    {
        if(tutorialProgression == 0)
        {
            //move
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("HorizontalController") != 0 || Input.GetAxis("VerticalController") != 0)
            {
                tutorialProgression++;
                UpdateTutorialUI();
            }
        }
        else if(tutorialProgression == 1)
        {
            //sprint
            if(Input.GetButtonDown("Sprint"))
            {
                tutorialProgression++;
                UpdateTutorialUI();
                stage1.SetBool("Activate", true);
            }
        }
        else if (tutorialProgression == 2)
        {
            //melee
            if (Input.GetButtonDown("Fire2") || Input.GetAxisRaw("Fire2") != 0)
            {
                tutorialProgression++;
                UpdateTutorialUI();
                stage2.SetBool("Activate", true);
            }
            
        }
        else if (tutorialProgression == 3)
        {
            //interact
            if (Input.GetButtonDown("Interact"))
            {
                tutorialProgression++;
                UpdateTutorialUI();
                stage3.SetBool("Activate", true);
            }
        }
        else if (tutorialProgression == 4)
        {
            //shoot
            if (Input.GetButtonDown("Fire1") || Input.GetAxisRaw("Fire1") != 0)
            {
                tutorialProgression++;
                UpdateTutorialUI();
            }
        }
        else if (tutorialProgression == 5)
        {
            //reload
            if (Input.GetButtonDown("ReloadButton"))
            {
                tutorialProgression++;
                UpdateTutorialUI();
                stage4.SetBool("Activate", true);
            }
        }
        else if (tutorialProgression == 6)
        {
            //swap weapons
            if(Input.GetAxis("Mouse ScrollWheel") != 0 || Input.GetButtonDown("RightBumber"))
            {
                tutorialProgression++;
                UpdateTutorialUI();
            }
        }
        else if (tutorialProgression == 7)
        {
            //show map
            if(Input.GetButtonDown("ToggleMap"))
            {
                tutorialProgression++;
                UpdateTutorialUI();
            }
        }
        else if (tutorialProgression == 8)
        {
            UnlockHelicopter();
            tutorialProgression++;
        }
    }

    public void UpdateTutorialUI()
    {
        if(controllerConnected == false)
        {
            pcTutorialArray[tutorialProgression - 1].SetActive(false);
            pcTutorialArray[tutorialProgression].SetActive(true);
        }
        else
        {
            controllerTutorialArray[tutorialProgression - 1].SetActive(false);
            controllerTutorialArray[tutorialProgression].SetActive(true);
        }
    }

    public void checkForController()
    {
        if(Input.GetJoystickNames().Length >= 1)
        {
            controllerConnected = true;
        }
        else
        {
            controllerConnected = false;
        }
    }

    public void UnlockHelicopter()
    {
        helicopterTrigger.SetActive(true);
    }
}