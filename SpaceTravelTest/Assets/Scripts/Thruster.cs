/*
Thruster.cs
Oct 6/2016
Peter Laliberte -  BurgZerg Arcade

Turn on/off a trail renderer when a ship moves

*/
using UnityEngine;


[DisallowMultipleComponent]
[RequireComponent(typeof(TrailRenderer))]
public class Thruster : MonoBehaviour {
    TrailRenderer trail;        //the trail renderer for this thruster



    void Awake()
    {
        //get a reference to the trail render component
        trail = GetComponent<TrailRenderer>();
    }



    public void Activate(bool isActive =  true)
    {
        //turn on/off the trail renderer
        if (isActive)
            trail.enabled = true;
        else
            FadeTrailRender();
    }


    void FadeTrailRender()
    {
        //turn the trail renderer component off
        trail.enabled = false;

        /*** For extra credit, make the trail renderer slowly retract instead of just turning off ***/
    }
}
