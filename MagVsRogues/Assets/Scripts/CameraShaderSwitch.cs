using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraShaderSwitch : MonoBehaviour
{
    private Material[] thiefs;

    private GameManagerCustom gm;
    private PhotonView photonview;

    public Shader seeThroughShader;
    public Shader originalShader;

    //public Color myColor; // color you want the camera to render it as
    //public Material material; // material you want the camera to change
    //public string colorPropertyName; // name of the color property in the material's shader

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerCustom>();
        if (PhotonNetwork.player.ID == gm.Round)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                photonview = players[i].GetComponent<PhotonView>();
                if (!photonview.isMine)
                {
                    thiefs[i] = players[i].GetComponentInChildren<Material>();
                }
            }
        }
    }

    void OnPreRender()
    {
        foreach (Material thief in thiefs)
        {
            thief.shader = seeThroughShader;
        }
    }

    void OnPostRender()
    {
        foreach (Material thief in thiefs)
        {
            thief.shader = originalShader;
        }
    }

    //private Color _default;
}
