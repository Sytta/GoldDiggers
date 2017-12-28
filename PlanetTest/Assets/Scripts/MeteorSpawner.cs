using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {

    public GravityAttractor planet;
    public Transform PrefabTransform;

	
	// Update is called once per frame
	void Update () {
        //Camera Shake
        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnMeteor();
        }
    }

    private void SpawnMeteor()
    {
        Vector3 spawnPosition = Random.onUnitSphere * 50f;
        Transform meteor = Instantiate(PrefabTransform, spawnPosition, Quaternion.identity);
        meteor.GetComponent<GravityBody>().planet = this.planet;
    }
}
