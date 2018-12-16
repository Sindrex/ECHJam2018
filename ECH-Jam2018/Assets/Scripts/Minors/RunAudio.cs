using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAudio : MonoBehaviour {

    public string audioKey;
    private bool hasRun = false;

	// Use this for initialization
	void Start () {
        //transform.GetChild(0).GetComponent<SoundController>().playAudio(audioKey);
	}
	
	// Update is called once per frame
	void Update () {
        if (!hasRun)
        {
            transform.GetChild(0).GetComponent<SoundController>().playAudio(audioKey);
            hasRun = true; 
        }
	}
}
