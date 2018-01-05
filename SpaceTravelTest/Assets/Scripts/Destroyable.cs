using UnityEngine;
using System.Collections;

public class Destroyable : MonoBehaviour {
    public float destructionDelay = 5f;
    public GameObject particleEffect;
    public float impactForceDamp = .15f;

    int crashDMG = 25;
    int scoreValue = 10;

    AudioSource sound;

    public static GameManager gm;


    void Awake()
    {
        sound = GetComponent<AudioSource>();
        gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }



    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;                         //skip the rest if it was not the player that we crashed in to

        Vector3 force = (other.impulse / Time.fixedDeltaTime) * impactForceDamp;    //the force to be applied to the player when collisions happen
        Vector3 point = other.contacts[0].point;                                    //the "contact point" on the player where the collision happenned

        other.rigidbody.AddForceAtPosition(force, point, ForceMode.Impulse);        //apply the force to the player
        Player.TakeDamage(crashDMG);                                                //apply the damage to the player
        DestroyMe();                                                                //Destroy us
    }



    public void DestroyMe()
    {
        if (particleEffect) Instantiate(particleEffect, transform.position, Quaternion.identity);

        if(!sound.isPlaying)
            sound.Play();

        GameManager.score.Add(scoreValue);

        EnemySpawner.curEnemyCount--;
        Destroy(gameObject, .5f);
    }
}
