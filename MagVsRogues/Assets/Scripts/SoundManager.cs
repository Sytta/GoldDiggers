using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;
	public AudioSource teleportSound;
	public AudioSource prisonSound;
	public AudioSource spellSound;
	public AudioSource coinPickUpSound;
	public AudioSource winSound;
	public AudioSource bgMusic;
	// Use this for initialization
	void Start () {
		instance = this;
	}
	public void playTeleport(){
		teleportSound.Play ();
	}

	public void playPrison(){
		prisonSound.Play ();
	}
	public void playSpell(){
		spellSound.Play ();
	}
	public void playCoin(){
		coinPickUpSound.Play ();
	}
	public void playBgMusic(){
		bgMusic.Play ();}
	public void playWin(){
		winSound.Play ();}
}
