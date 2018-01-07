using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIService : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private Image timerFill;
    [SerializeField] private Text timerTxt;

    [Header("Gold")]
    [SerializeField] private Text goldMageTxt;
    [SerializeField] private Text goldThiefTxt;

    [Header("PowerUps")]
    [SerializeField] private Transform powerUpsContainer;
    [SerializeField] private GameObject powerUpPrefab;

    [Header("Scores")]
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private GameObject roundTitle;
    [SerializeField] private GameObject finalTitle;
    [SerializeField] private GameObject roundScoreContainer;
    [SerializeField] private GameObject totalScoreContainer;
    [SerializeField] private Text[] playerNames;
    [SerializeField] private Text[] playerRoundScores;
    [SerializeField] private Text[] playerTotalScores;
    [SerializeField] private GameObject endGameMessage;
    [SerializeField] private Button menuButton;

    private GameManagerCustom gameManager;

	// Use this for initialization
	void Start()
	{
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerCustom>();

        menuButton.onClick.AddListener(OnMenuClicked);

        EventManager.Instance.AddListener<OnPowerUpCreated>(Handle);
        EventManager.Instance.AddListener<OnPowerUpReset>(Handle);
	}

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener<OnPowerUpCreated>(Handle);
    }

	// Update is called once per frame
	void Update()
	{
        RefreshTimerVisuals();
        RefreshGoldVisuals();
	}

    void OnMenuClicked()
    {
        Application.LoadLevel("MainMenu");
    }

    private void RefreshTimerVisuals()
    {
        timerFill.fillAmount = gameManager.gameTime / gameManager.roundTotalTime;
        timerTxt.text = ((int)(gameManager.gameTime)).ToString();
    }

    private void RefreshGoldVisuals()
    {
        goldThiefTxt.text = (gameManager.GoldThief1 + gameManager.GoldThief2).ToString();
        goldMageTxt.text = gameManager.GoldMage.ToString();
    }

    public void ShowRoundScores()
    {
        scoreboard.SetActive(true);
        roundTitle.SetActive(true);
        finalTitle.SetActive(false);
        roundScoreContainer.SetActive(true);
        totalScoreContainer.SetActive(false);
        menuButton.gameObject.SetActive(false);

        for (int i = 0; i < playerRoundScores.Length; i++)
        {
            if (PhotonPlayer.Find(i + 1) != null)
                playerNames[i].text = PhotonPlayer.Find(i + 1).NickName;
            playerRoundScores[i].text = gameManager.ScoringEndRound[i].ToString();
        }
    }

    public void ShowTotalScores()
    {
        scoreboard.SetActive(true);
        roundTitle.SetActive(false);
        finalTitle.SetActive(true);
        roundScoreContainer.SetActive(false);
        totalScoreContainer.SetActive(true);
        menuButton.gameObject.SetActive(true);

        for (int i = 0; i < playerTotalScores.Length; i++)
        {
            if (PhotonPlayer.Find(i + 1) != null)
                playerNames[i].text = PhotonPlayer.Find(i + 1).NickName;
            playerTotalScores[i].text = gameManager.ScoringOverall[i].ToString();
        }
    }

    public void CloseScores()
    {
        scoreboard.SetActive(false);
    }

    public void ShowEndGameMessage()
    {
        endGameMessage.SetActive(true);
    }

    public void CloseEndGameMessage()
    {
        endGameMessage.SetActive(false);
    }

    public void Handle(OnPowerUpCreated e)
    {
        GameObject powerUp = Instantiate(powerUpPrefab, powerUpsContainer);
        PowerUpUI uiComp = powerUp.GetComponent<PowerUpUI>();
        if (uiComp != null)
            uiComp.SetUp(e.Type, e.Cooldown);
    }

    public void Handle(OnPowerUpReset e)
    {
        foreach (Transform child in powerUpsContainer)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
