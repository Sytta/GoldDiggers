using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class Asteroid : MonoBehaviour {
    public Vector3 rotationDirection;
    public float rotationSpeed;
    public float impactForceDamp = .25f;
    public GameObject boomFX;

    int dmgTaken = 20;

    Transform myT;
    AudioSource crashSound;



    void Awake()
    {
        myT = transform;
        crashSound = GetComponent<AudioSource>();
    }


    void Start()
    {
        RotationDirection();
    }


    void Update()
    {
        //roate the asteroid
        myT.Rotate(rotationDirection * Time.deltaTime * rotationSpeed);
    }


    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        Vector3 force = (other.impulse / Time.fixedDeltaTime) * impactForceDamp;    //the force to be applied to the player when collisions happen
        Vector3 point = other.contacts[0].point;                                    //the "contact point" on the player where the collision happenned

        other.rigidbody.AddForceAtPosition(force, point, ForceMode.Impulse);        //apply the force to the player
        if (boomFX)  Instantiate(boomFX, point, Quaternion.identity);               //spawn particle system on the contact point
        if(!crashSound.isPlaying)
            crashSound.Play();                                                      //Play Crash Sound
        Player.TakeDamage(dmgTaken);
    }



    void RotationDirection()
    {
        rotationSpeed = Random.Range(10, 20);
        rotationDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }


}
