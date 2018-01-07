using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraShaderSwitch : MonoBehaviour
{
    private Dictionary<int,Renderer> thiefs = new Dictionary<int, Renderer>();
    private GameManagerCustom gm;
    private PhotonView photonview;
    public CameraSimple mainCamera;
    public Shader seeThroughShader;
    public List<Shader> originalShaders = new List<Shader>();


    public float visionCooldown = 15.0f;
    public float visionTimer = 15.0f;
    public bool canUseVision = true;

    //public Color myColor; // color you want the camera to render it as
    //public Material material; // material you want the camera to change
    //public string colorPropertyName; // name of the color property in the material's shader

    private void Update()
    {
		if (Input.GetKey(KeyCode.Mouse1) && canUseVision)
        {
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerCustom>();
            var myPlayerId = mainCamera.Target.GetComponent<GenericUser>().myID;
            if (myPlayerId == gm.Round)
            {

                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                //Debug.Log("player count : " + players.Length);
                for (int i = 0; i < players.Length; i++)
                {
                    var playerId = players[i].GetComponent<GenericUser>().myID;
                    if (!thiefs.ContainsKey(playerId) && playerId != myPlayerId)
                    {
                        //Debug.Log("Added player : " + players[i].GetComponent<GenericUser>().myID);
                        thiefs.Add(playerId, players[i].GetComponentInChildren<Renderer>());
                        originalShaders.Add(players[i].GetComponentInChildren<Renderer>().material.shader);
                    }
                }
            }

            SeeThrough();

            EventManager.Instance.QueueEvent(new OnPowerUpUsed(PowerUpType.Infrared));

        } else if (originalShaders.Count > 0)
        {
            
            Recover();
        }
		if (Input.GetKeyUp(KeyCode.Mouse1) && canUseVision) {
			canUseVision = false;
		}

        if (!canUseVision)
        {
            if(visionTimer >= 0.0f)
            {
                visionTimer -= Time.deltaTime;
            }
            if(visionTimer <= 0.0f)
            {
                canUseVision = true;
                visionTimer = visionCooldown;
            }
        }
    }

    void SeeThrough()
    {
        // Animation
        mainCamera.Target.GetComponent<Animator>().SetBool("SeeThrough", true);

        foreach (Renderer thief in thiefs.Values)
        {
            if (thief.material.shader != null)
                thief.material.shader = seeThroughShader;
        }
    }

    void Recover()
    {
        // Animation
        mainCamera.Target.GetComponent<Animator>().SetBool("SeeThrough", false);

        int cnt = 0;
        foreach (Renderer thief in thiefs.Values)
        {
            if (thief != null  && thief.material != null && thief.material.shader != null)
                thief.material.shader = originalShaders[cnt++];
        }

    }

    //private Color _default;
}
