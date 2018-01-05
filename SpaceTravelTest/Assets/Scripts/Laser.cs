using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Laser : MonoBehaviour {
    public int damage = 10;
    public float laserForce = 100f;
    public GameObject hitParticles;

    bool isActive = false;

    LineRenderer lr;
    Vector3 target;
    AudioSource shootSound;



    void Awake()
    {
        if (!lr) lr = GetComponent<LineRenderer>();
        if (!hitParticles) Debug.Log("Add a particle effect for the laser hits");

        shootSound = GetComponent<AudioSource>();
    }



    void Start()
    {
        Deactivate();
    }


    void Update()
    {
        if (!isActive) return;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, target);
        lr.enabled = true;

        if (!shootSound.isPlaying)
            shootSound.Play();
    }



    public void Activate(Vector3 target)
    {
        this.target = target;
        isActive = true;
    }



    public void Deactivate()
    {
        lr.enabled = false;
        isActive = false;
    }
}
