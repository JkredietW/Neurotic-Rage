using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Scrollbar scrolbar;
    public ScrollRect myScrollRect;
    public FadeToFromBlack fb;
    public GameObject playPanel;
    public GameObject settingsPanel;
    public GameObject tuturialButton;
    public float secondsToReopen;
    public float secondsTillCameraShake;
    public Animator garageDoor;
    public Transform scoreContentObject;
    public List<TextMeshProUGUI> scorebord;
    public bool garage;
    public EventSystem eventSystem;
    public GameObject main;
    public GameObject[] tips;
    [Header("Sounds")]
    public AudioSource hover;
    public AudioSource pressed;
    public TMP_Dropdown skinDropDown;

    //input detection
    int lastInput; //0 = mouse, 1 = controller
    Vector2 lastpos;
    GameObject lastFirstSelected;
	private int playerOneInputType;

    private StatHolder statsScript;

    void Start()
    {
        StartCoroutine(IEStart());
        statsScript = new StatHolder();
        scorebord = new List<TextMeshProUGUI>();
        foreach (Transform item in scoreContentObject)
        {
            scorebord.Add(item.GetComponent<TextMeshProUGUI>());
        }
        GetSaves();
        ChangeActivePanel(main);
        if (PlayerPrefs.GetString("Tuturial") == "true")
		{
            tuturialButton.SetActive(true);
		}
        skinDropDown.value = PlayerPrefs.GetInt("Skin");
    }
    public void SetInp5Seconds(GameObject obj)
	{
        StartCoroutine(SetActiveAfther(obj));
	}
    IEnumerator SetActiveAfther(GameObject obj)
	{
        obj.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(true);
    }
    public void GetSaves()
    {
        if (System.IO.File.ReadAllText(Application.persistentDataPath + "/Stats.json") != null)
        {
            string data = System.IO.File.ReadAllText(Application.persistentDataPath + "/Stats.json");
            StatHolder _statsScript = JsonUtility.FromJson<StatHolder>(data);
            statsScript = _statsScript;
            statsScript.ResetCurrentGameStats();
            ShowStats();
        }
    }
    public void SetObjectActive(TipsToUnlock ttu)
	{
		if (ttu.text.text == "Basic")
		{
            ttu.tip[0].SetActive(true);
        }
        else if (ttu.text.text == "Neon")
        {
            ttu.tip[1].SetActive(true);
        }
        else if (ttu.text.text == "Big Enemy")
        {
            ttu.tip[2].SetActive(true);
        }
        else if (ttu.text.text == "Small Enemy")
        {
            ttu.tip[3].SetActive(true);
        }
        else if (ttu.text.text == "Glitch Enemy")
        {
            ttu.tip[4].SetActive(true);
        }
	}
    public void SetObjectNotActive(TipsToUnlock ttu)
    {
		for (int i = 0; i < ttu.tip.Length; i++)
		{
            ttu.tip[i].SetActive(false);
        }
    }
    public void ShowStats()
    {
        scorebord[0].text = "Deaths :" + "  " + Mathf.Round(statsScript.total_deaths).ToString();
        scorebord[1].text = "Total runs completed :" + "  " + Mathf.Round(statsScript.total_competedRuns).ToString();
        scorebord[2].text = "Waves completed :" + "  " + Mathf.Round(statsScript.total_competedWaves).ToString();
        scorebord[3].text = "Time played :" + "" + Mathf.Round(statsScript.total_timePlayed).ToString() + "/sec";

        scorebord[4].text = "Damage done :" + "  " + Mathf.Round(statsScript.total_damageDone).ToString();
        scorebord[5].text = "Damage taken :" + "  " + Mathf.Round(statsScript.total_damageTaken).ToString();
        scorebord[6].text = "Damage healed :" + "  " + Mathf.Round(statsScript.total_damageHealed).ToString();
        scorebord[7].text = "Bullets shot :" + "  " + Mathf.Round(statsScript.total_bulletsShot).ToString();
        scorebord[8].text = "Bullets hit :" + "  " + Mathf.Round(statsScript.total_bulletsHit).ToString();
        scorebord[9].text = "Bullets missed :" + "  " + Mathf.Round(statsScript.total_bulletsMissed).ToString();
        scorebord[10].text = "Times reloaded :" + "  " + Mathf.Round(statsScript.total_timesReloaded).ToString();
        scorebord[11].text = "Time spend shooting :" + "  " + Mathf.Round(statsScript.total_timeWastedNotShooting).ToString() + "/sec";
        scorebord[12].text = "Time wasted not shooting :" + "  " + Mathf.Round(statsScript.total_timeWastedShooting).ToString() + "/sec";
        scorebord[13].text = "Distance walked :" + "  " + Mathf.Round(statsScript.total_distanceWalked).ToString() + "/m";

        scorebord[14].text = "Money collected :" + "  " + Mathf.Round(statsScript.total_moneyCollected).ToString();
        scorebord[15].text = "Money spend :" + "  " + Mathf.Round(statsScript.total_moneySpend).ToString();
        scorebord[16].text = "Items bought :" + "  " + Mathf.Round(statsScript.total_itemsBought).ToString();
        scorebord[17].text = "Ammo bought :" + "  " + Mathf.Round(statsScript.total_ammoBought).ToString();
        scorebord[18].text = "Health bought :" + "  " + Mathf.Round(statsScript.total_healthBought).ToString();
        scorebord[19].text = "Upgrades bought :" + "  " + Mathf.Round(statsScript.total_upgradesBought).ToString();
        scorebord[20].text = "Shops opened :" + "  " + Mathf.Round(statsScript.total_shopsOpened).ToString();

        scorebord[21].text = "Kills :" + "  " + Mathf.Round(statsScript.total_kills).ToString();
        scorebord[22].text = "Small enemy kills :" + "  " + Mathf.Round(statsScript.total_smallEnemyKills).ToString();
        scorebord[23].text = "Medium enemy kills :" + "  " + Mathf.Round(statsScript.total_mediumEnemyKills).ToString();
        scorebord[24].text = "Big enemy kills :" + "  " + Mathf.Round(statsScript.total_bigEnemyKills).ToString();
        scorebord[25].text = "Glitch enemy kills :" + "  " + Mathf.Round(statsScript.total_glitchEnemyKills).ToString();
        scorebord[26].text = "Giant enemy kills :" + "  " + Mathf.Round(statsScript.total_giantEnemyKills).ToString();
    }
    public void SelectSkin(TMP_Dropdown _value)
    {
        int selectedSkin = 0;
        if(Mathf.Round(statsScript.total_competedRuns) > 0 && _value.value == 1)
        {
            selectedSkin = 1;
        }
        else if(Mathf.Round(statsScript.total_giantEnemyKills) > 150 && _value.value == 2)
        {
            selectedSkin = 2;
        }
        else if (Mathf.Round(statsScript.total_smallEnemyKills) > 690 && _value.value == 3)
        {
            selectedSkin = 3;
        }
        else if (Mathf.Round(statsScript.total_glitchEnemyKills) > 950 && _value.value == 4)
        {
            selectedSkin = 4;
        }
        else
        {
            selectedSkin = 0;
            _value.SetValueWithoutNotify(selectedSkin);
        }
        PlayerPrefs.SetInt("Skin", selectedSkin);
        Invoke("SetTipsOff", 1);
    }
	public void ResetScrollBar()
	{
        scrolbar.value = 1;
        Canvas.ForceUpdateCanvases();
        myScrollRect.verticalNormalizedPosition = 1;
        Canvas.ForceUpdateCanvases();
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
    public void SetTipsOff()
	{
		for (int i = 0; i < tips.Length; i++)
		{
            tips[i].SetActive(false);
        }
    }
    public void Hover()
	{
        hover.Play();
    }
    public void Pressed()
	{
        pressed.Play();
    }
    public void PlaySound(AudioSource audio)
    {
		if (audio.isPlaying)
		{
            return;
		}
        audio.Play();
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
    public void ChangeInput(TMP_Dropdown _value)
    {
        int playerOneInputType = _value.value;
        PlayerPrefs.SetInt("playerinput", playerOneInputType);
    }
}