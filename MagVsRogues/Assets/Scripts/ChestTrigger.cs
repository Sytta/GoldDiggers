using UnityEngine;
using System.Collections;

public class ChestTrigger : MonoBehaviour
{
    private bool canLoot = false;
    private bool isLooting = false;
    private float animatonSpeed;
    private float timeMultiplier = 0;
    private int keyDownCounter = 0;
    private float chronoKey = 0;
    private GameObject coll = null;
    private int goldYield = 0;
    private float timeModifier = 1.0f;

    [SerializeField] private int goldAmount = 5;
    [SerializeField] private float magicPPS = 10;

    private void Start()
    {
        // TODO Get time from animation
        animatonSpeed = 1.0f;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Chest")
        {
            canLoot = true;
        }
        coll = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Chest")
        {
            canLoot = false;
        }
        coll = null;
    }

    private IEnumerator WaitForLoot()
    {
        isLooting = true;
        // TODO Set animation speed here
        if (timeMultiplier >= 0)
            timeModifier = Mathf.Max(keyDownCounter / timeMultiplier - magicPPS, 1);
        else
            timeModifier = 1;
        timeMultiplier = 0;
        keyDownCounter = 0;
        yield return new WaitForSecondsRealtime(animatonSpeed / timeModifier);

        goldYield += coll.GetComponent<ChestController>().DecreaseGold(goldAmount);
        isLooting = false;
    }



    private void Update()
    {
        if(canLoot)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if(isLooting == false)
                    StartCoroutine(WaitForLoot());
            }

            if(isLooting)
            {
                timeMultiplier += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    keyDownCounter += 1;
                }
            }


        }
    }

}