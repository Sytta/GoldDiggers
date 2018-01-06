using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour {

    private GameObject diablo = null;

    [SerializeField] private int size = 2;
    // Use this for initialization
    void Start () {
        diablo = Instantiate(Resources.Load("Diablo", typeof(GameObject))) as GameObject;
        diablo.SetActive(false);    
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Z))
        {

            if (!diablo.activeSelf)
            {
                diablo.SetActive(true);
                diablo.transform.SetParent(this.gameObject.transform);
                diablo.transform.localPosition = new Vector3(0, 0, size);
                diablo.transform.localScale = new Vector3(size, 0.01f, size);
            }
            
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            diablo.GetComponent<TeleporterDiablo>().Jail();
            diablo.SetActive(false);
        }
        if (diablo.activeSelf)
        {
            diablo.transform.Rotate(0, 4.0f, 0, Space.Self);
        }
    }
}
