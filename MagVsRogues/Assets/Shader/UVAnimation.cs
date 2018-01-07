using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimation : MonoBehaviour {

	public int uvTileY = 4;
	public int uvTileX = 4;
	
	public int fps = 30;
	public int lastframe = 4;
	
	private int index;
	private Vector2 size;
	private Vector2 offset;
	private Renderer myrenderer;
	
	// Use this for initialization
	void Start () {
		myrenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		index = (int)(Time.time * fps);
		
		index = index % (lastframe);
		
		size = new Vector2(1.0f / uvTileY, 1.0f / uvTileX);
		
		var uIndex = index % uvTileX;
		var vIndex = index / uvTileX;
		
		offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);
		
		myrenderer.material.SetTextureOffset("_MainTex", offset);
		myrenderer.material.SetTextureScale("_MainTex", size);
	}
}
