using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    /* Author: Sindrex
     * Date 14.12.2018
     *
     * Add sound clips in audiolist in inspector (editor)
     * Add a key (string) for each audio, at the same index as the audio it corresponds to
     * To play a given sound/Audio: playAudio(myKey)!
     * If you want to change volume and maxDist through AudioSource instead, set volume and/or maxDist to 0.
     * DO NOT HIT APPLY in inspector
     */

    public float volume;
    public float maxDist;
    public List<AudioClip> audioList = new List<AudioClip>();
    public string[] audioKey;

    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        if (volume > 0)
        {
            source.volume = volume;
        }
        if (maxDist > 0)
        {
            source.maxDistance = maxDist;
        }
        //playAudio("Zoe");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playAudio(string key)
    {
        int index = System.Array.IndexOf(audioKey, key);
        if(index >= 0)
        {
            source.PlayOneShot(audioList[index]);
        }
    }
}
