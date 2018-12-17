using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStarScript : MonoBehaviour {

    public bool go; //testing

    public GameObject myStar;
    private GameObject playerCam;

    public GameObject lerpTo;
    private Vector3 lerpStart;
    private bool beginLerp;
    public float lerpSpeed = 1;
    private float lerpDistance;
    private float startTime;

	// Use this for initialization
	void Start () {
        myStar.SetActive(false);
        playerCam = GameObject.Find("Main Camera"); 
	}
	
	// Update is called once per frame
	void Update () {
        if (go || Input.GetKeyDown(KeyCode.G))
        {
            startShootingStar();
            go = false;
        }
        if (beginLerp)
        {
            playerCam.transform.position = Vector3.Lerp(lerpStart, lerpTo.transform.position, (Time.time - startTime) * lerpSpeed / lerpDistance);
        }
	}

    public void startShootingStar()
    {
        startTime = Time.time;
        lerpStart = playerCam.transform.position;
        lerpDistance = Vector3.Distance(lerpStart, lerpTo.transform.position);
        beginLerp = true;
        StartCoroutine("shootStar");
    }

    IEnumerator shootStar()
    {
        yield return new WaitForSeconds(1.5f);
        myStar.SetActive(true);
        myStar.GetComponent<Animator>().Play("ShootingStarAnim", 0, 0);
    }
}
