using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap_Brick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Destroy(this.gameObject);
    }
}
