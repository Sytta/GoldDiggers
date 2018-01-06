using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour {

    private GameObject diablo = null;

    [SerializeField] private int size = 2;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Z))
        {

            if (diablo == null)
            { 
                diablo = PhotonNetwork.Instantiate("Diablo", new Vector3(0, 0, 0), Quaternion.identity, 0);
                diablo.transform.SetParent(this.gameObject.transform);
                diablo.transform.localPosition = new Vector3(0, 0, size);
                diablo.transform.localScale = new Vector3(size, 0.01f, size);
            }
            
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            Destroy(diablo);
            diablo = null; 
        }
        if (diablo != null)
        {
            diablo.transform.Rotate(0, 4.0f, 0, Space.Self);
        }
    }
}
