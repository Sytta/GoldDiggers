using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonManager : MonoBehaviour {

    private bool isOpen, changingState;
	// Use this for initialization
	void Start () {
        isOpen = false;
        changingState = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (changingState == true)
        {
            if(isOpen == false) // opening
            {
                if(this.gameObject.transform.parent.transform.rotation.eulerAngles.y < 90 || this.gameObject.transform.parent.transform.rotation.eulerAngles.y >= 180)
                    this.gameObject.transform.parent.transform.Rotate(new Vector3(0, 60f * Time.deltaTime, 0));
                else
                {
                    isOpen = true;
                    changingState = false;
                }
            }
            else
            {
                if (this.gameObject.transform.parent.transform.rotation.eulerAngles.y > 0 && this.gameObject.transform.parent.transform.rotation.eulerAngles.y <= 180)
                    this.gameObject.transform.parent.transform.Rotate(new Vector3(0, -60f * Time.deltaTime, 0));
                else
                {
                    isOpen = false;
                    changingState = false;
                }
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (changingState == false)
            {
                changingState = true;
            }
            Debug.Log(changingState);
        }
    }

}
