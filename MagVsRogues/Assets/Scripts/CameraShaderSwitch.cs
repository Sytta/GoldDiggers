using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraShaderSwitch : MonoBehaviour
{
    private List<Renderer> thiefs = new List<Renderer>();

    private GameManagerCustom gm;
    private PhotonView photonview;

    public Shader seeThroughShader;
    public List<Shader> originalShaders = new List<Shader>();

    //public Color myColor; // color you want the camera to render it as
    //public Material material; // material you want the camera to change
    //public string colorPropertyName; // name of the color property in the material's shader

    private void Update()
    {
        if (Input.GetKey("p"))
        {
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerCustom>();
            if (PhotonNetwork.player.ID == gm.Round && thiefs.Count < 1)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < players.Length; i++)
                {
                    photonview = players[i].GetComponent<PhotonView>();
                    if (photonview.ownerId != PhotonNetwork.player.ID)
                    {
                        thiefs.Add(players[i].GetComponentInChildren<Renderer>());
                        originalShaders.Add(players[i].GetComponentInChildren<Renderer>().material.shader);
                    }
                }
            }

            SeeThrough();
        } else if (originalShaders.Count > 0)
        {
            Recover();
        }
    }

    void SeeThrough()
    {
        foreach (Renderer thief in thiefs)
        {
            thief.material.shader = seeThroughShader;
        }
    }

    void Recover()
    {
        for (int i = 0; i < thiefs.Count; i++)
        {
            thiefs[i].material.shader = originalShaders[i];
        }
    }

    //private Color _default;
}
