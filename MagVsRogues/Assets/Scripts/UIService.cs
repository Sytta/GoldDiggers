using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIService : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private Image timerFill;
    [SerializeField] private float roundTime = 60.0f;
    [SerializeField] private Text timerTxt;

    [Header("Gold")]
    [SerializeField] private float maxGold = 1000;
    [SerializeField] private Text goldMageTxt;
    [SerializeField] private Text goldThiefTxt;

    [Header("PowerUps")]
    [SerializeField] private Transform powerUpsContainer;
    [SerializeField] private GameObject powerUpPrefab;

    private GameManagerCustom gameManager;

	// Use this for initialization
	void Start()
	{
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManagerCustom>();

        EventManager.Instance.AddListener<OnGoldModified>(Handle);
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

    public void Handle(OnGoldModified e)
    {
        //RefreshGoldVisuals(e.goldAmount);
    }
}
