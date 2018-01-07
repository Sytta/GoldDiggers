using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Thief: MonoBehaviour
{
    private Animator animator;

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
    public bool hasTeleportPower = true;
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
        animatonSpeed = 1.958f;

        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chest")
        {
            coll = other.gameObject;
            canLoot = !coll.GetComponent<ChestController>().isEmpty();
            gameManger.FindMage();
            if (!gameManger.magePlayer.GetPhotonView().isMine)
                coll.GetComponent<ChestController>().ShowInviteMessage();
        }
        else
            canLoot = false;
    }

    void OnTriggerExit(Collider other)
    {
        canLoot = false;
        if (other.tag == "Chest")
        {
            other.gameObject.GetComponent<ChestController>().CloseInviteMessage();
        }
    }

    [PunRPC]
    public void Bust()
    {
        animator.SetTrigger("PutInPrison");
        int[] data = new int[2];
        data[0] = -1 * (int)(goldYield / 2);
        data[1] = this.GetComponent<GenericUser>().myID;
        goldYield += data[0];
        this.gameObject.GetComponent<GenericUser>().currentGold = goldYield;

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<PhotonView>().RPC("stolenCash", PhotonTargets.All, data);

    }

    public void SpawnThief(int location)
    {   
        EventManager.Instance.QueueEvent(new OnPowerUpCreated(PowerUpType.Teleportation, teleportCooldown));

        if(location == 2)
        {
            this.gameObject.GetComponent<GenericUser>().Teleport(new Vector3(0.5f, -2f, -5.5f), this.gameObject);
        }
        else if (location == 3)
        {
            this.gameObject.GetComponent<GenericUser>().Teleport(new Vector3(-4.5f, -2f, -5.5f), this.gameObject);
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

        // Animation
        animator.speed = timeModifier;
        

        keyDownCounter = 0;
        Debug.Log("Getting $$$ in " + animatonSpeed / timeModifier + " second(s)" );

        animator.SetBool("Robber", true);
        yield return new WaitForSecondsRealtime(animatonSpeed / timeModifier);
        // Stop animation
        animator.SetBool("Robber", false);

        int collected = coll.GetComponent<ChestController>().DecreaseGold(goldAmount);
        int[] data = new int[2];
        data[0] = collected;
        data[1] = this.GetComponent<GenericUser>().myID;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<PhotonView>().RPC("stolenCash", PhotonTargets.All, data);
        goldYield += collected;
        this.gameObject.GetComponent<GenericUser>().currentGold = goldYield;
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
        if (GameObject.FindGameObjectWithTag("Diablo") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Diablo"));
        }
        if(canLoot && coll != null)
        {
			if (Input.GetKeyUp(KeyCode.Mouse0))
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
				if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    keyDownCounter += 1;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && canTeleport)
        {
            EventManager.Instance.QueueEvent(new OnPowerUpUsed(PowerUpType.Teleportation));
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