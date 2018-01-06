using UnityEngine;
using System.Collections;

public class DemoRPGMovement : MonoBehaviour 
{
    public RPGCamera Camera;

    void OnJoinedRoom()
    {
        CreatePlayerObject();
    }

    void CreatePlayerObject()
    {
        Vector3 position = new Vector3(-0.4f, 1.599893f, -7.19f);

        GameObject newPlayerObject = PhotonNetwork.Instantiate( "Char_2", position, Quaternion.identity, 0 );

        Camera.Target = newPlayerObject.transform;
    }
}
