//using System.Collections;
//using System.Linq;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerTelleportController : MonoBehaviour {

//    public bool canTeleport = false;
//    public List<Transform> TeleporterLocations;
//    public GameManagerCustom gameManger;
//    private GameObject mageCharacter; 
    
//    void Start()
//    {
//        gameManger = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerCustom>();
//        TeleporterLocations = gameManger.teleportLocations;

//    }
	
//	// Update is called once per frame
//	void Update () {
//        if (Input.GetKeyUp(KeyCode.T))
//        {
//            gameManger.FindMage();
//            mageCharacter = gameManger.magePlayer;
//            if (mageCharacter == this.gameObject)
//            {
//                return;
//            }
//            else
//            {
//                var teleportLocation = SelectTeleport();
//                this.transform.position = teleportLocation.position;
//            }

//        }
//	}

//    private Transform SelectTeleport()
//    {
//        Transform selectedTeleport = TeleporterLocations[0];
//        List<Transform> sortedTeleports = 
//            (TeleporterLocations.OrderBy
//            (x => Vector3.Distance
//                        (mageCharacter.transform.position, 
//                        x.position)
//                        )
//            )
//            .ToList();

//        int random = Random.Range(0, sortedTeleports.Count - 1);

//        return sortedTeleports[random];

//    }
//}
