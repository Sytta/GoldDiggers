using UnityEngine;
using System.Collections;

public class DelayedDestroy : MonoBehaviour {
    public float timer = 5f;

    void Start()
    {
        Destroy(gameObject, timer);
    }
}
