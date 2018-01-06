using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUser : MonoBehaviour {
    public int myID = -1;
	// Use this for initialization
	void Start () {
        myID = this.gameObject.GetComponent<PhotonView>().ownerId;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
