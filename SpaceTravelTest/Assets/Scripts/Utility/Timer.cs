/*
Timer.cs
Oct 6/2016
Peter Laliberte - BurgZerg Arcade

A simple timer that uses the InvokeRepeating method to keep track of time.

*/
using UnityEngine;
using UnityEngine.UI;
using System; //needed for TimeSpan

[RequireComponent(typeof(Text))]
public class Timer : MonoBehaviour {
    int timer;   //keep track of the seconds that have passed since the level started
    Text timerUI;       //reference to the UI element that displays the time



    void Awake()
    {
        //make sure that we have a reference to the Text component
        timerUI = GetComponent<Text>();
    }



    void Start()
    {
        //initialize the timer
        timer = 0;
        UpdateDisplay();
    }



    public void StartTimer()
    {
        //start the timer. It will call the method tick every second after the first second
        InvokeRepeating("Tick", 1, 1);
    }



    public void StopTimer()
    {
        //stop the timer
        CancelInvoke("Tick");
    }



    //give other classes a way to access the amount of seconds that have passed
    public int GetTotalTime
    {
        get { return timer; }
    }



    void Tick()
    {
        //add a second to the timer
        timer++;
        UpdateDisplay();
    }



    void UpdateDisplay()
    {
        //format the time and then pass it to the display
        TimeSpan time = TimeSpan.FromSeconds(timer);
        timerUI.text = time.ToString();
    }
}
