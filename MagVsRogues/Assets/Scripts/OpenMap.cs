using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMap : MonoBehaviour {

    [SerializeField] private float prisonArea;
    [SerializeField] private Button open;
    [SerializeField] private Button close;

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
                    open.gameObject.SetActive(true);
            }
            else
            {
                open.gameObject.SetActive(false);
                DeactivateMap();
            }
        }
	}

    public void ActivateMap()
    {
        map.SetActive(true);
        open.gameObject.SetActive(false);
        close.gameObject.SetActive(true);
    }

    public void DeactivateMap()
    {
        map.SetActive(false);
        close.gameObject.SetActive(false);
    }
}
