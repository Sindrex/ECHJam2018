using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour {

    public GameObject[] indoors;
    public GameObject outdoor_collidables;

	// Use this for initialization
	void Start () {
        //enableIndoors();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void enableIndoors()
    {
        foreach(GameObject g in indoors)
        {
            if(g != null)
            {
                g.SetActive(true);
            }
        }
        if(outdoor_collidables != null)
        {
            outdoor_collidables.SetActive(false);
        }
    }

    public void disbleIndoors()
    {
        foreach (GameObject g in indoors)
        {
            if (g != null)
            {
                g.SetActive(false);
            }
        }
        if (outdoor_collidables != null)
        {
            outdoor_collidables.SetActive(true);
        }
    }
}
