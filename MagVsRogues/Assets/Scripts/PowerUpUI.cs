using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerUpUI : MonoBehaviour
{
    [SerializeField] private Image powerUpBackground;
    [SerializeField] private Image powerUpCooldown;
    [SerializeField] private float cooldown;
    private float timer;
    private int powerUpId;

    public void SetUp(int id, PowerUpType type)
    {
        // Assign correct images when they'll be created
        powerUpId = id;

        EventManager.Instance.AddListener<OnPowerUpUsed>(Handle);
    }

    private void RefreshVisuals()
    {
        powerUpCooldown.fillAmount = timer / cooldown;
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
        if (e.ID != powerUpId)
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
    SpeedUp,
    Infrared
}
