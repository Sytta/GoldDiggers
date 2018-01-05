using UnityEngine;
using System.Collections;
using UnityEngineInternal;


[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class EnemyMovement : MonoBehaviour {
    public Transform target;

    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float movementSpeed = 50f;
    [SerializeField] float rayCastOffset = 2f;
    int numberOfDetectionRays = 5;  //always increase this value by 4+1!!


    Transform t;
    Vector3[] detectionRay;



    void Awake()
    {
        t = transform;

        if (!target) FindTarget();
    }



    void Start()
    {
        detectionRay = new Vector3[numberOfDetectionRays];

        for (int cnt = 0; cnt < numberOfDetectionRays; cnt++)
            detectionRay[cnt] = t.position;
    }



    void Update()
    {
        //if the player is dead,find something else to target and move around

        if (!target || !target.CompareTag("Player")) FindTarget();

        RaycastHit hit;
        detectionRay[0] = (target.position - t.position).normalized;                                //forward
        detectionRay[1] = new Vector3(t.position.x + rayCastOffset, t.position.y, t.position.z);    //left
        detectionRay[2] = new Vector3(t.position.x - rayCastOffset, t.position.y, t.position.z);    //right
        detectionRay[3] = new Vector3(t.position.x, t.position.y - rayCastOffset, t.position.z);    //down
        detectionRay[4] = new Vector3(t.position.x, t.position.y + rayCastOffset, t.position.z);    //up

        //Raycast to see if something is in the way
        for (int cnt = 0; cnt < numberOfDetectionRays; cnt++)
        {
            if (Physics.Raycast(detectionRay[cnt], t.forward, out hit, turnSpeed))
            {
                //if we did not hit ourself with the ray
                if (hit.transform != t)
                {
                    detectionRay[0] += hit.normal * turnSpeed;
                }

                //Draw a line in the editor if we hit something
                if (hit.transform.CompareTag("Player"))
                {
                    Debug.DrawLine(t.position, hit.point, Color.green);
                }
                //else Debug.DrawLine(t.position, hit.point, Color.yellow);

            }
            Debug.DrawRay(detectionRay[cnt], transform.forward, Color.cyan);

        }

        Quaternion rotation = Quaternion.LookRotation(detectionRay[0]);

        t.rotation = Quaternion.Slerp(t.rotation, rotation, Time.deltaTime);
        t.position += t.forward * movementSpeed * Time.deltaTime;
    }






    void FindTarget()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Player");

        if(!temp)
        {
            GameObject[] probes = GameObject.FindGameObjectsWithTag("Enemy");
            target = probes[Random.Range(0, probes.Length)].transform;
        }
        else
            target = temp.transform;
    }
}
