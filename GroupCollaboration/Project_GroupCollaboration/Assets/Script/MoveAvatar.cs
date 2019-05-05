using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//********************
//Avatar functions hooked up to UI buttons
//
//********************


public class MoveAvatar : MonoBehaviour {

    private float latitude;
    private float longitude;
    private float altitude;
    private float stepSize = 1.0f;

    public SharedLocation networkManager;

	// Use this for initialization
	void Start () {
        latitude = this.transform.position.x;
        altitude = this.transform.position.y;
        longitude = this.transform.position.z;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void setPosition ()
    {
        transform.position = new Vector3(latitude, altitude, longitude);
        //update status via the local client
        networkManager.sendPosition(latitude, altitude, longitude);
    }

    private void setPortalPosition ()
    {
        networkManager.sendPortal(latitude, altitude, longitude);
    }

    public void moveUp()
    {
        altitude += stepSize; setPosition();
    }

    public void moveDown()
    {
        altitude -= stepSize; setPosition();
    }

    public void moveForward()
    {
        longitude += stepSize; setPosition();
    }

    public void moveBackward()
    {
        longitude -= stepSize; setPosition();
    }

    public void moveLeft()
    {
        latitude -= stepSize; setPosition();
    }

    public void moveRight()
    {
        latitude += stepSize; setPosition();
    }

    public void spawnPortal ()
    {

        setPortalPosition();
    }
}
