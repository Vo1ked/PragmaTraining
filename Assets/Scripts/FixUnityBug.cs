using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixUnityBug : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    transform.GetComponent<Camera>().enabled = false;
        transform.GetComponent<Camera>().enabled = true;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
