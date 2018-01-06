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
        initialHight = goldPile.localPosition.y - magicH;
    }



    void UpdateMesh()
    {
        var y = (float)gold / initialGold * initialHight + magicH;
        goldPile.localPosition = new Vector3(goldPile.localPosition.x, y , goldPile.localPosition.z);
        if (gold <= 0)
            goldPile.gameObject.active = false;
    }

    public int DecreaseGold(int ammount)
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