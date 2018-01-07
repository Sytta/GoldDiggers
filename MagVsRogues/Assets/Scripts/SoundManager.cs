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
	public AudioSource ironGate;
	public AudioSource walkSound;
	public AudioSource spellChannelSound;
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
	public void playIronGate(){
		ironGate.Play ();}

	public void playWalk(){
		walkSound.Play ();}
	public void stopWalk(){
		walkSound.Stop ();}

	public void playChannelSpell(){
		spellChannelSound.Play ();}
	public void stopChannelSpell(){
		spellChannelSound.Stop ();}
}
