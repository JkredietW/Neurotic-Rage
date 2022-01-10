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
    public ScoreBord scorebord;
    public bool garage;
    public EventSystem eventSystem;
    public GameObject main;
    [Header("Sounds")]
    public AudioSource hover;
    public AudioSource pressed;

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
        GetSaves();
        ChangeActivePanel(main);
        if (PlayerPrefs.GetString("Tuturial") == "true")
		{
            tuturialButton.SetActive(true);
		}
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
    public void ShowStats()
    {
        scorebord.scores[0].text.text = "Deaths" + "  " + Mathf.Round(statsScript.total_deaths).ToString();
        scorebord.scores[1].text.text = "Total runs completed" + "  " + Mathf.Round(statsScript.total_competedRuns).ToString();
        scorebord.scores[2].text.text = "Waves completed" + "  " + Mathf.Round(statsScript.total_competedWaves).ToString();
        scorebord.scores[3].text.text = "Time played" + "" + Mathf.Round(statsScript.total_timePlayed).ToString() + "/sec";

        scorebord.scores[4].text.text = "Damage done" + "  " + Mathf.Round(statsScript.total_damageDone).ToString();
        scorebord.scores[5].text.text = "Damage taken" + "  " + Mathf.Round(statsScript.total_damageTaken).ToString();
        scorebord.scores[6].text.text = "Damage healed" + "  " + Mathf.Round(statsScript.total_damageHealed).ToString();
        scorebord.scores[7].text.text = "Bullets shot" + "  " + Mathf.Round(statsScript.total_bulletsShot).ToString();
        scorebord.scores[8].text.text = "Bullets hit" + "  " + Mathf.Round(statsScript.total_bulletsHit).ToString();
        scorebord.scores[9].text.text = "Bullets missed" + "  " + Mathf.Round(statsScript.total_bulletsMissed).ToString();
        scorebord.scores[10].text.text = "Times reloaded" + "  " + Mathf.Round(statsScript.total_timesReloaded).ToString();
        scorebord.scores[11].text.text = "Time spend shooting" + "  " + Mathf.Round(statsScript.total_timeWastedNotShooting).ToString() + "/sec";
        scorebord.scores[12].text.text = "Time wasted not shooting" + "  " + Mathf.Round(statsScript.total_timeWastedShooting).ToString() + "/sec";
        scorebord.scores[13].text.text = "Distance walked" + "  " + Mathf.Round(statsScript.total_distanceWalked).ToString() + "/m";

        scorebord.scores[14].text.text = "Money collected" + "  " + Mathf.Round(statsScript.total_moneyCollected).ToString();
        scorebord.scores[15].text.text = "Money spend" + "  " + Mathf.Round(statsScript.total_moneySpend).ToString();
        scorebord.scores[16].text.text = "Items bought" + "  " + Mathf.Round(statsScript.total_itemsBought).ToString();
        scorebord.scores[17].text.text = "Ammo bought" + "  " + Mathf.Round(statsScript.total_ammoBought).ToString();
        scorebord.scores[18].text.text = "Health bought" + "  " + Mathf.Round(statsScript.total_healthBought).ToString();
        scorebord.scores[19].text.text = "Upgrades bought" + "  " + Mathf.Round(statsScript.total_upgradesBought).ToString();
        scorebord.scores[20].text.text = "Shops opened" + "  " + Mathf.Round(statsScript.total_shopsOpened).ToString();

        scorebord.scores[21].text.text = "Kills" + "  " + Mathf.Round(statsScript.total_kills).ToString();
        scorebord.scores[22].text.text = "Small enemy kills" + "  " + Mathf.Round(statsScript.total_smallEnemyKills).ToString();
        scorebord.scores[23].text.text = "Medium enemy kills" + "  " + Mathf.Round(statsScript.total_mediumEnemyKills).ToString();
        scorebord.scores[24].text.text = "Big enemy kills" + "  " + Mathf.Round(statsScript.total_bigEnemyKills).ToString();
        scorebord.scores[25].text.text = "Glitch enemy kills" + "  " + Mathf.Round(statsScript.total_glitchEnemyKills).ToString();
        scorebord.scores[26].text.text = "Giant enemy kills" + "  " + Mathf.Round(statsScript.total_giantEnemyKills).ToString();
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
    public void Hover()
	{
        hover.Play();
    }
    public void Pressed()
	{
        pressed.Play();
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