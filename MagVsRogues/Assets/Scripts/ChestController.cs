using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ChestController : MonoBehaviour
{
    public float magicH = -0.2f;
    public int gold;
    public Transform goldPile;
    private int initialGold;
    private float initialHight;

    void Start()
    {
        initialGold = gold;
        initialHight = goldPile.localPosition.z - magicH;
    }



    void UpdateMesh()
    {
        var z = (float)gold / initialGold * initialHight + magicH;
        goldPile.localPosition = new Vector3(goldPile.localPosition.x, goldPile.localPosition.y , z);
        if (gold <= 0)
            goldPile.gameObject.active = false;
    }

    public int DecreaseGold(int ammount)
    {
        int taken = gold;
        gold -= ammount;
        if (gold < 0)
            gold = 0;
        this.gameObject.GetComponent<PhotonView>().RPC("UpdateSync", PhotonTargets.All, gold);
        UpdateMesh();
        return taken - gold;
    }

    [PunRPC]
    void UpdateSync(int g)
    {
        gold = g;
        UpdateMesh();
    }


    [PunRPC]
    void initGold(int g)
    {
        gold = g;
        initialGold = gold;
        initialHight = goldPile.localPosition.z - magicH;
        UpdateMesh();
    }

    public bool isEmpty()
    {
        return gold <= 0;
    }
}