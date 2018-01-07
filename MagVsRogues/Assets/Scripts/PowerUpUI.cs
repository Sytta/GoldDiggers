using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerUpUI : MonoBehaviour
{
    [SerializeField] private Image powerUpBackground;
    [SerializeField] private Image powerUpCooldown;
    [SerializeField] private Text powerUpKey;
    [SerializeField] private Text powerUpDesc;
    [SerializeField] private float cooldown;
    [SerializeField] private Sprite[] powerUpImgs;
    [SerializeField] private string[] powerUpKeys;
    [SerializeField] private string[] powerUpDescs;
    private float timer;
    private PowerUpType type;

    public void SetUp(PowerUpType t, float cd)
    {
        // Assign correct images when they'll be created
        type = t;
        powerUpBackground.sprite = powerUpImgs[(int)type];
        powerUpCooldown.sprite = powerUpImgs[(int)type];
        powerUpKey.text = powerUpKeys[(int)type];
        powerUpDesc.text = powerUpDescs[(int)type];

        cooldown = cd;
        timer = cooldown;
        RefreshVisuals();

        EventManager.Instance.AddListener<OnPowerUpUsed>(Handle);
    }

    private void RefreshVisuals()
    {
        if (cooldown != 0)
            powerUpCooldown.fillAmount = timer / cooldown;
        else
            powerUpCooldown.fillAmount = 1.0f;
    }

    private IEnumerator StartCooldown()
    {
        while (timer != cooldown)
        {
            yield return new WaitForSeconds(1.0f);
            timer++;
            RefreshVisuals();
        }
    }

    public void Handle(OnPowerUpUsed e)
    {
        if (e.Type != type)
            return;

        timer = 0.0f;
        RefreshVisuals();
        StopAllCoroutines();
        StartCoroutine(StartCooldown());
    }
}

public enum PowerUpType
{
    Teleportation,
    Infrared,
    Jail
}
