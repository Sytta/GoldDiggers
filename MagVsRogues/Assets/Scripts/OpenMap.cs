using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMap : MonoBehaviour {

    [SerializeField] private float prisonArea;
    [SerializeField] private GameObject open;
    [SerializeField] private GameObject close;

    [SerializeField] private GameObject map;
    private GameManagerCustom gm;

	// Use this for initialization
	void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerCustom>();

    }
	
	// Update is called once per frame
	void Update () {
        gm.FindMage();
        if (gm.magePlayer != null && gm.magePlayer.GetPhotonView().isMine)
        {
            if (Vector3.Distance(transform.position, gm.magePlayer.transform.position) < prisonArea)
            {
                if (!map.active)
                {
                    open.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.M))
                    {
                        ActivateMap();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.M))
                    {
                        DeactivateMap();
                    }
                }
            }
            else
            {
                open.SetActive(false);
                DeactivateMap();
            }
        }
	}

    public void ActivateMap()
    {
        map.SetActive(true);
        open.SetActive(false);
        close.SetActive(true);
    }

    public void DeactivateMap()
    {
        map.SetActive(false);
        close.SetActive(false);
    }
}
