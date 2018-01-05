/*
Score.cs
Oct 6/2016
Peter Laliberte - BurgZerg Arcade

Track the player score and allow some other classes to add to the score.
*/

using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour {
    int score;                       //the current score
    int timerBonusValue = 1000;      //bonus for each second survived
    Text scoreUI;                    //reference to the display for the score



    void Awake()
    {
        //get a reference tot he display for the score
        scoreUI = GetComponent<Text>();
    }



    void Start()
    {
        //initialize the score
        score = 0;
        UpdateDisplay();
    }



    public void Add(int amt)
    {
        //add a set amount of points tot he score
        score += amt;
        UpdateDisplay();
    }



    void UpdateDisplay()
    {
        //update the display of the score
        scoreUI.text = String.Format("{0:n0}", score);
    }



    public void TimerBonus(int amt)
    {
        //add a bonus to the score based on the amount of time left on the timer
        Add(amt * timerBonusValue);
    }
}
