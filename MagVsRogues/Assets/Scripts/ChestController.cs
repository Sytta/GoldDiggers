using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour
{
    public float magicH = -0.2f;
    public int gold;
    public Transform goldPile;
    public int initialGold;
    private float initialHight;

    void Start()
    {
        initialGold = gold;
        initialHight = goldPile.localPosition.z - magicH;
    }

    private void LateUpdate()
    {
        this.gameObject.GetComponent<GenericUser>().currentGold = gold;
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
        int withdraw = taken - gold;
        gold += withdraw;
        this.gameObject.GetComponent<PhotonView>().RPC("Withdraw", PhotonTargets.All, withdraw);
        UpdateMesh();
        return withdraw;
    }

    [PunRPC]
    void UpdateSync(int g)
    {
        gold = g;
        UpdateMesh();
    }

    [PunRPC]
    void Withdraw(int g)
    {
        gold -= g;
        UpdateMesh();
    }

    [PunRPC]
    void initGold(int g)
    {
        gold = g;
        initialGold = g;
    }

    public bool isEmpty()
    {
        return gold <= 0;
    }


}