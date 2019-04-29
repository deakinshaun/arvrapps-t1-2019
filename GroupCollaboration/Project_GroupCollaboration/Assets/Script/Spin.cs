using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation *= Quaternion.AngleAxis(0.5f, new Vector3(0.5f, -0.5f, 0.1f));
	}
}
