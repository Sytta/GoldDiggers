using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ChestController : MonoBehaviour
{
    public float magicH = -0.02f;
    public int gold;
    public Transform goldPile;
    private int initialGold;
    private float initialHight;

    void Start()
    {
        initialGold = gold;
        initialHight = goldPile.position.y + magicH;
    }



    void UpdateMesh()
    {
        var z = (float)gold / initialGold * initialHight;
        goldPile.position = new Vector3(goldPile.position.x, goldPile.position.y, 1 - z);
        if (gold <= 0)
            goldPile.gameObject.active = false;
    }

    public int decreseGold(int ammount)
    {
        int taken = gold;
        gold -= ammount;
        if (gold < 0)
            gold = 0;

        UpdateMesh();
        return taken - gold;
    }

    public bool isEmpty()
    {
        return gold <= 0;
    }
}