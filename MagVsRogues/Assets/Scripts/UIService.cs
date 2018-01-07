using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    private GameManagerCustom gameManager;

	// Use this for initialization
	void Start()
	{
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerCustom>();

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

    public void ShowScores()
    {
        scoreboard.SetActive(true);
    }

    public void CloseScores()
    {
        scoreboard.SetActive(false);
    }

    public void Handle(OnPowerUpCreated e)
    {
        GameObject powerUp = Instantiate(powerUpPrefab, powerUpsContainer);
        PowerUpUI uiComp = powerUp.GetComponent<PowerUpUI>();
        if (uiComp != null)
            uiComp.SetUp(e.Type);
    }

    public void Handle(OnPowerUpReset e)
    {
        foreach (Transform child in powerUpsContainer)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
