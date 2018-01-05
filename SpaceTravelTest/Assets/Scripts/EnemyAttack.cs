using UnityEngine;
using System.Collections;


[DisallowMultipleComponent]
public class EnemyAttack : MonoBehaviour {
    public Laser laser;
    public float attackRange;
    public float attackSpeed;
    public float laserLength = 300f;
    public float aimModifier = 1f;
    public Transform target;

    float attackTimer = 0f;
    float laserOnTimer = .1f;

    Transform t;

    
    //***check out the design patterns videos on youtube - if no good ones, then make some.***


    /*
    //V2
    Lasers are a fixed length and travel to the destinaltion over a set time
    Wave counter
    */



    void Awake()
    {
        t = transform;

        if (!target) target = GameObject.FindGameObjectWithTag("Player").transform; //find the player Transform
        if (!laser) laser = GetComponentInChildren<Laser>();                        //get the Laser component in the child GameObject - this will be an array later
    }



    void Update()
    {
        if (!target) return;

        //check to see if it is time to attack
        if (Time.time > attackTimer)
        {
            attackTimer = Time.time + attackSpeed;                                  //reset the attack timer

            if (!HaveLineOfSight())
                return;

            if (!PlayerInRange())
                return;

            if (!PlayerInfront())
                return;

            FireLaser();
        }
    }


    bool HaveLineOfSight()
    {
        Vector3 dir = (target.position - transform.position).normalized;    //calculate the direction 

        if (!Physics.Raycast(transform.position, dir, attackRange))
            return false;
        else
            return true;
    }



    bool PlayerInRange()
    {
        float distance = Vector3.Distance(transform.position, target.position); //calculate the distance to the target

        if (distance > attackRange) return false;                                     //if we are not in range, then return
        else return true;
    }



    bool PlayerInfront()
    {
        Vector3 heading = target.position - t.position;
        float dot = Vector3.Dot(heading, target.transform.forward);

        if (dot > 0) return true;
        else return false;
    }



    void FireLaser()
    {
        Renderer rend = target.GetComponent<Renderer>();

        Vector3 targetCenter = rend.bounds.center;

        float x = targetCenter.x + Random.Range(-rend.bounds.extents.x, rend.bounds.extents.x) * aimModifier;
        float y = targetCenter.y + Random.Range(-rend.bounds.extents.y, rend.bounds.extents.y) * aimModifier;
        float z = targetCenter.z + Random.Range(-rend.bounds.extents.z, rend.bounds.extents.z) * aimModifier;

        Vector3 shotTarget = new Vector3(x, y, z);


        Vector3 dir = (shotTarget - transform.position).normalized;    //calculate the shot direction 
        RaycastHit hit;


        //if we hit the player
        if (Physics.Raycast(transform.position, dir, out hit, laserLength))
        {
            if(hit.transform.CompareTag("Player"))
            {
                //apply some force to the spot that took the hit
                hit.rigidbody.AddForceAtPosition(dir * laser.laserForce, hit.point, ForceMode.Impulse);
                //add particle effect to spot where ship got hit.
                Instantiate(laser.hitParticles, hit.point, Quaternion.identity);

                Player.TakeDamage(laser.damage);    //make the player take damage
            }

            laser.Activate(hit.point);          //fire the laser at the target
            StartCoroutine("TurnOffLaser");     //turn the laser off after it has fired
        }
        else
        {
            laser.Activate(dir * attackRange);          //fire the laser at the target
            StartCoroutine("TurnOffLaser");                      //turn the laser off after it has fired
        }
    }



    //move this to the laser script
    IEnumerator TurnOffLaser()
    {
        yield return new WaitForSeconds(laserOnTimer);   //wait a set amount of time before we turn off the laser
        laser.Deactivate();                             //turn off the laser
    }
}
