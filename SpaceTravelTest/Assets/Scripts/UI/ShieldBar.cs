/*
ShieldBar
Oct 16/2016
Peter Laliberte - BurgZerg Arcade

Display the Shield strength on the screen and adjust it acording to he current shield %
*/
using UnityEngine;


[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class ShieldBar : MonoBehaviour {
    RectTransform bar;      //the RectTransform of the shild bar to adjust



    void Awake()
    {
        //get a reference to the RectTransform to adjust for the shield bar
        bar = GetComponent<RectTransform>();
    }



    public void UpdateDisplay(float adj)
    {
        //make sure that the value is between 0 and 1
        if (adj < 0)
            adj = 0;
        else if (adj > 1)
            adj = 1;

        //adjust the size of the grapgic on the local x
        bar.localScale = new Vector3(adj, bar.localScale.y, bar.localScale.z);
    }
}
