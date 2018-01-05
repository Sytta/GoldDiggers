using UnityEngine;
using System.Collections;


[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour {
    static float shieldPower;         //starting shield power
    public static ShieldBar shieldDisplay;

    public GameManager gm;
    public GameObject goBoom;

    int regenRate = 5;             //regenerate 10 shield every second
    static float maxShieldPower = 100;       //max shield power you can have

    float HyperDrivePower { get; set; }


    public bool isAlive = true;

    AudioSource deathSound;




    void Awake()
    {
        deathSound = GetComponent<AudioSource>();

        if (!gm)
            gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        shieldDisplay = GameObject.FindGameObjectWithTag("Shield Display").GetComponent<ShieldBar>();
    }



    void Start()
    {
        shieldPower = maxShieldPower;
        InvokeRepeating("RegenShield", 1, 1);
    }



    //get rid of this!
    void Update()
    {
        if (!isAlive) return;
        if (shieldPower < 0) DestroyMe();
    }




    void RegenShield()
    {
        if (shieldPower < maxShieldPower) shieldPower += regenRate;
        if (shieldPower > maxShieldPower) shieldPower = maxShieldPower;

        shieldDisplay.UpdateDisplay(shieldPower / maxShieldPower);
    }



    public static void TakeDamage( int dmg )
    {
        shieldPower -= dmg;
        shieldDisplay.UpdateDisplay(shieldPower/maxShieldPower);
    }



    void DestroyMe()
    {
        GetComponent<MeshRenderer>().enabled = false;

        //activate the Reset button
        gm.PlayerDied();

        isAlive = false;

        if (goBoom) Instantiate(goBoom, transform.position, Quaternion.identity);

        deathSound.Play();
        Destroy(gameObject, .5f);
    }
}
