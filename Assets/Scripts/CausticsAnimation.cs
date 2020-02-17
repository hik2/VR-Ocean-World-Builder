using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CausticsAnimation : MonoBehaviour {

    public int frameRate;

    public Texture[] frames;
    private Projector projector;
    

	// Use this for initialization
	void Start ()
    {
        frames = Resources.LoadAll<Texture>("Caustics Textures");
        projector = GetComponent<Projector>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        int frame = (int)(Time.time * frameRate) % 240;
        projector.material.mainTexture = frames[frame];
	}
}
