using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour {

    private GameObject diablo = null;
    private Animator animator;

    [SerializeField] private int size = 2;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (diablo == null)
        {
            diablo = Instantiate(Resources.Load("Diablo", typeof(GameObject))) as GameObject;
            diablo.SetActive(false);
        }
		if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!diablo.activeSelf)
            {
                diablo.SetActive(true);
                // Animation
                animator.SetBool("PutInPrison", true);

                diablo.transform.SetParent(this.gameObject.transform);
                diablo.transform.localPosition = new Vector3(0, 0, size);
                diablo.transform.localScale = new Vector3(size, 0.01f, size);
            }
            
        }
		else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            diablo.GetComponent<TeleporterDiablo>().Jail();
            diablo.SetActive(false);
            // Animation
            animator.SetBool("PutInPrison", false);

        }
        if (diablo != null && diablo.activeSelf)
        {
            diablo.transform.Rotate(0, 4.0f, 0, Space.Self);
        }
    }
}
