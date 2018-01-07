using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIService : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private Image timerFill;
    [SerializeField] private float roundTime = 60.0f;
    [SerializeField] private Text timerTxt;
    private float timer;

    [Header("Gold")]
    [SerializeField] private float maxGold = 1000;
    [SerializeField] private Text goldMageTxt;
    [SerializeField] private Text goldThiefTxt;

    [Header("PowerUps")]
    [SerializeField] private Transform powerUpsContainer;
    [SerializeField] private GameObject powerUpPrefab;

	// Use this for initialization
	void Start()
	{
        EventManager.Instance.AddListener<OnRoundStarted>(Handle);
        EventManager.Instance.AddListener<OnGoldModified>(Handle);
	}

	// Update is called once per frame
	void Update()
	{
			
	}

    private IEnumerator TimerCountdown()
    {
        while (timer != 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            timer--;
            RefreshTimerVisuals();
        }
    }

    private void RefreshTimerVisuals()
    {
        timerFill.fillAmount = timer / roundTime;
        timerTxt.text = timer.ToString();
    }

    private void RefreshGoldVisuals(int gold)
    {
        goldThiefTxt.text = gold.ToString();
        goldMageTxt.text = (maxGold - gold).ToString();
    }

    public void Handle(OnRoundStarted e)
    {
        timer = roundTime;
        RefreshTimerVisuals();
        StopAllCoroutines();
        StartCoroutine(TimerCountdown());
    }

    public void Handle(OnGoldModified e)
    {
        RefreshGoldVisuals(e.goldAmount);
    }
}
