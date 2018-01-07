using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Thief: MonoBehaviour
{
    private bool canLoot = false;
    public bool isLooting = false;
    private float animatonSpeed;
    private float timeMultiplier = 0;
    private int keyDownCounter = 0;
    private float chronoKey = 0;
    private GameObject coll = null;
    private int goldYield = 0;
    private float timeModifier = 1.0f;

    [SerializeField] private int goldAmount = 5;
    [SerializeField] private float magicMinPPS = 4;
    [SerializeField] private float magicMaxPPS = 20;
    [SerializeField] private float magicMinSpeedUp = 1;
    [SerializeField] private float magicMaxSpeedUp = 5;
    private float a, b;


    // TELEPORT LOGIC
    public float teleportCooldown = 15.0f;
    public float teleportTimer = 15.0f;
    public bool canTeleport = true;
    public bool hasTeleportPower = false;
    public List<Transform> TeleporterLocations;
    public GameManagerCustom gameManger;
    private GameObject mageCharacter;

    private void Start()
    {
        //Teleport logic
        gameManger = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerCustom>();
        TeleporterLocations = gameManger.teleportLocations;

        a = (magicMaxSpeedUp - magicMinSpeedUp) / (magicMaxPPS - magicMinPPS);
        b = magicMaxSpeedUp - a * magicMaxPPS;
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

    public void SpawnThief(int location)
    {   
        if(location == 2)
        {
            this.gameObject.GetComponent<GenericUser>().Teleport(new Vector3(0.5f, -2f, -7.5f), this.gameObject);
        }
        else if (location == 3)
        {
            this.gameObject.GetComponent<GenericUser>().Teleport(new Vector3(-4.5f, -2f, -7.5f), this.gameObject);
        }
    }

    private IEnumerator WaitForLoot()
    {
        isLooting = true;
        // TODO Set animation speed here
        if (timeMultiplier >= 0)
            timeModifier = Mathf.Max(a * (keyDownCounter / timeMultiplier) + b, 1);
        else
            timeModifier = 1;
        timeMultiplier = 0;
        keyDownCounter = 0;
        Debug.Log("Getting $$$ in " + animatonSpeed / timeModifier + " second(s)" );
        yield return new WaitForSecondsRealtime(animatonSpeed / timeModifier);

        int collected = coll.GetComponent<ChestController>().DecreaseGold(goldAmount);
        int[] data = new int[2];
        data[0] = collected;
        data[1] = this.GetComponent<GenericUser>().myID;
        GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManagerCustom>().GetComponent<PhotonView>().RPC("stolenCash", PhotonTargets.All, data);
        goldYield += collected;

        Debug.Log("Got :" + goldYield + "g");
        isLooting = false;
    }

    private Transform SelectTeleport()
    {
        Transform selectedTeleport = TeleporterLocations[0];
        List<Transform> sortedTeleports =
            (TeleporterLocations.OrderBy
            (x => Vector3.Distance
                        (mageCharacter.transform.position,
                        x.position)
                        )
            )
            .ToList();

        int random = Random.Range(0, sortedTeleports.Count - 1);

        return sortedTeleports[random];

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

        if (Input.GetKeyUp(KeyCode.T) && canTeleport)
        {
            gameManger.FindMage();
            mageCharacter = gameManger.magePlayer;
            {
                var teleportLocation = SelectTeleport();
                this.gameObject.GetComponent<GenericUser>().Teleport(teleportLocation.position, this.gameObject);

            }
            canTeleport = false;
        }

        if(!canTeleport && hasTeleportPower)
        {
            // Decrement cooldown only if cooldown is active
            if(teleportTimer >= 0.0f)
            {
                teleportTimer -= Time.deltaTime;
            }
            // If cooldown is over, we can now use it again.
            if(teleportTimer <= 0.0f)
            {
                canTeleport = true;
                teleportTimer = teleportCooldown;
            }
        }
    }
}