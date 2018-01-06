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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chest")
        {
            coll = other.gameObject;
            canLoot = !coll.GetComponent<ChestController>().isEmpty();
        }
        else
            canLoot = false;
    }

    void OnTriggerExit(Collider other)
    {
        canLoot = false;
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
        Debug.Log("Getting $$$ in " + animatonSpeed / timeModifier + " second(s)" );
        yield return new WaitForSecondsRealtime(animatonSpeed / timeModifier);

        goldYield += coll.GetComponent<ChestController>().DecreaseGold(goldAmount);

        Debug.Log("Got :" + goldYield + "g");
        isLooting = false;
    }



    private void Update()
    {
        if(canLoot && coll != null)
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                if (isLooting == false)
                {
                    canLoot = !coll.GetComponent<ChestController>().isEmpty();
                    if(canLoot)
                        StartCoroutine(WaitForLoot());
                }
            }

            if(isLooting)
            {
                timeMultiplier += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    keyDownCounter += 1;
                }
            }


        }
    }

}