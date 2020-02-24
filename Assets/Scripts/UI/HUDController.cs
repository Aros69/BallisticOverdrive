using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public enum HUDMode
{
    none,
    playing,
    spectating,
    connecting,
    waitingForPlayer,
    blueTeamVictory,
    redTeamVictory
}

public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    [Header("Game HUD")]
    [SerializeField] private GameObject gameHUD;
    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text rounds;
    [SerializeField] private TMP_Text blueScore;
    [SerializeField] private TMP_Text redScore;

    [Header("Player Bars")]
    [SerializeField] private float barSpacing = 15f;
    [SerializeField] private Image raygunIcon;
    [SerializeField] private Image bulletgunIcon;
    [SerializeField] private Image healthIcon;
    [SerializeField] private RectTransform healthBarContainer;
    [SerializeField] private RectTransform ammoBarContainer;

    [Header("TextInfos")]
    [SerializeField] private GameObject connectingText;
    [SerializeField] private GameObject waitingForPlayerText;
    [SerializeField] private GameObject blueTeamWinsText;
    [SerializeField] private GameObject redTeamWinsText;
    [SerializeField] private GameObject spectatingText;
    [SerializeField] private GameObject blackScreen;

    [Header("EscapeMenu")]
    [SerializeField] private GameObject escapeMenu;

	[Header("Death Screen")]
	[SerializeField] private GameObject deathScreen;

    private HUDMode mode;
    private bool showMenu = false;

    private GameObject[] healthBars;
    private GameObject[] ammoBars;
    private float maxHealthBarSize;
    private float maxAmmoBarSize;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        mode = HUDMode.none;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(showMenu){
                HideMenu();
            } else {
                ShowMenu();
            }
        }
    }

    public void BlackScreen(bool active){
        blackScreen.SetActive(active);
    }

    public void SetPlayerColor(Color color)
    {
        raygunIcon.color = color;
        bulletgunIcon.color = color;
        healthIcon.color = color;
    }

    public void UpdateTimeLeft(int timeInSeconds)
    {
        int minutes = timeInSeconds / 60;
        int seconds = timeInSeconds % 60;

        time.text = minutes + ":" + seconds;
    }

    public void UpdateRounds(int roundCounter)
    {
        rounds.text = "Round " + roundCounter;
    }

    public void UpdateScores(int blueTeamScore, int redTeamScore)
    {
        blueScore.text = blueTeamScore.ToString();
        redScore.text = redTeamScore.ToString();
    }

    public void SetWeaponRailgun()
    {
        raygunIcon.gameObject.SetActive(true);
        bulletgunIcon.gameObject.SetActive(false);
    }

    public void SetWeaponBulletgun()
    {
        raygunIcon.gameObject.SetActive(false);
        bulletgunIcon.gameObject.SetActive(true);
    }

    public void SetMaxLife(int maxLife)
    {
        GameObject template = ((Transform)healthBarContainer).GetChild(0).gameObject;

        maxHealthBarSize = (healthBarContainer.rect.width - maxLife * (barSpacing + 1)) / maxLife;

        healthBars = new GameObject[maxLife];
        for(int i = 0; i < maxLife; i++)
        {
            healthBars[i] = Instantiate(template, healthBarContainer.transform);
            healthBars[i].SetActive(true);
            ((RectTransform)healthBars[i].transform).sizeDelta = new Vector2(maxHealthBarSize, 40);
            ((RectTransform)healthBars[i].transform).anchoredPosition = new Vector2(barSpacing*(i+1)+maxHealthBarSize*i, 0);
        }
    }

    public void UpdateLife(int life)
    {
		Debug.Log("life: " + life);
        for (int i = 0; i < life; i++)
        {
            healthBars[i].SetActive(true);
        }

        for (int i = life; i < healthBars.Length; i++)
        {
            healthBars[i].SetActive(false);
        }
    }

    public void SetMaxAmmo(int maxAmmo)
    {
        GameObject template = ((Transform)ammoBarContainer).GetChild(0).gameObject;

        maxAmmoBarSize = (ammoBarContainer.rect.width - maxAmmo * (barSpacing + 1)) / maxAmmo;

		if (ammoBars != null)
		{
			for(int i = 0; i < ammoBars.Length; i++)
			{
				Destroy(ammoBars[i]);
			}
		}

        ammoBars = new GameObject[maxAmmo];
        for (int i = 0; i < maxAmmo; i++)
        {
            ammoBars[i] = Instantiate(template, ammoBarContainer.transform);
            ammoBars[i].SetActive(true);
            ((RectTransform)ammoBars[i].transform).sizeDelta = new Vector2(maxAmmoBarSize, 40);
            ((RectTransform)ammoBars[i].transform).anchoredPosition = new Vector2(barSpacing * (i + 1) + maxAmmoBarSize * i, 0);
        }
    }

    public void UpdateAmmo(float ammo)
    {
        int ammoCount = (int)ammo;
        ammo = ammo - ammoCount;
        for (int i = 0; i < ammoCount; i++)
        {
            ammoBars[i].SetActive(true);
            ((RectTransform)ammoBars[i].transform).sizeDelta = new Vector2(maxAmmoBarSize, 40);
        }

        if(ammoCount < ammoBars.Length)
        {
            ammoBars[ammoCount].SetActive(true);
            ((RectTransform)ammoBars[ammoCount].transform).sizeDelta = new Vector2(maxAmmoBarSize*ammo, 40);
        }

        for (int i = ammoCount+1; i < ammoBars.Length; i++)
        {
			ammoBars[i].SetActive(false);
        }
    }

    public void ShowMenu(){
        showMenu = true;
        escapeMenu.SetActive(true);
        Cursor.visible = true;
    }

    public void HideMenu(){
        showMenu = false;
        escapeMenu.SetActive(false);
        Cursor.visible = false;
    }

    public void LeaveGame(){
		ClientScene.localPlayer.GetComponent<ServerCommunication>().CmdPlayerLeave();
		if (ODNetworkManager.singleton.mode == NetworkManagerMode.ClientOnly){
            ODNetworkManager.singleton.StopClient();
        } else {
            ODNetworkManager.singleton.StopHost();
        }
    }

    public void Quit(){
		ClientScene.localPlayer.GetComponent<ServerCommunication>().CmdPlayerLeave();
		Application.Quit();
    }

    public void SetMode(HUDMode newMode)
    {
        mode = newMode;
        switch (mode)
        {
            case HUDMode.none:
                ModeNone();
                break;

            case HUDMode.playing:
                ModePlaying();
                break;

            case HUDMode.spectating:
                ModeSpectating();
                break;

            case HUDMode.connecting:
                ModeConnecting();
                break;

            case HUDMode.waitingForPlayer:
                ModeWaitingForPlayer();
                break;

            case HUDMode.blueTeamVictory:
                ModeBlueTeamVictory();
                break;

            case HUDMode.redTeamVictory:
                ModeRedTeamVictory();
                break;
        }
    }

    private void ModeNone()
    {
        gameHUD.SetActive(false);
        connectingText.SetActive(false);
        waitingForPlayerText.SetActive(false);
        blueTeamWinsText.SetActive(false);
        redTeamWinsText.SetActive(false);
        spectatingText.SetActive(false);
		deathScreen.SetActive(false);
	}

    private void ModePlaying()
    {
        gameHUD.SetActive(true);
        connectingText.SetActive(false);
        waitingForPlayerText.SetActive(false);
        blueTeamWinsText.SetActive(false);
        redTeamWinsText.SetActive(false);
        spectatingText.SetActive(false);
		deathScreen.SetActive(false);
	}

    private void ModeSpectating()
    {
        gameHUD.SetActive(true);
        connectingText.SetActive(false);
        waitingForPlayerText.SetActive(false);
        blueTeamWinsText.SetActive(false);
        redTeamWinsText.SetActive(false);
        spectatingText.SetActive(true);
		deathScreen.SetActive(false);
	}

    private void ModeConnecting()
    {
        gameHUD.SetActive(false);
        connectingText.SetActive(true);
        waitingForPlayerText.SetActive(false);
        blueTeamWinsText.SetActive(false);
        redTeamWinsText.SetActive(false);
        spectatingText.SetActive(false);
		deathScreen.SetActive(false);
	}

    private void ModeWaitingForPlayer()
    {
        gameHUD.SetActive(false);
        connectingText.SetActive(false);
        waitingForPlayerText.SetActive(true);
        blueTeamWinsText.SetActive(false);
        redTeamWinsText.SetActive(false);
        spectatingText.SetActive(false);
		deathScreen.SetActive(false);
	}

    private void ModeBlueTeamVictory()
    {
        gameHUD.SetActive(false);
        connectingText.SetActive(false);
        waitingForPlayerText.SetActive(false);
        blueTeamWinsText.SetActive(true);
        redTeamWinsText.SetActive(false);
        spectatingText.SetActive(false);
		deathScreen.SetActive(false);
	}

    private void ModeRedTeamVictory()
    {
        gameHUD.SetActive(false);
        connectingText.SetActive(false);
        waitingForPlayerText.SetActive(false);
        blueTeamWinsText.SetActive(false);
        redTeamWinsText.SetActive(true);
        spectatingText.SetActive(false);
		deathScreen.SetActive(false);
	}

	private void ModePlayerDie()
	{
		gameHUD.SetActive(false);
		connectingText.SetActive(false);
		waitingForPlayerText.SetActive(false);
		blueTeamWinsText.SetActive(false);
		redTeamWinsText.SetActive(false);
		spectatingText.SetActive(false);
		deathScreen.SetActive(true);
	}
}
