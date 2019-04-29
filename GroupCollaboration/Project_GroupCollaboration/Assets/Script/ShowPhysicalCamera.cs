using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPhysicalCamera : MonoBehaviour {

    public Material camTextMaterial;
    public Camera camera;

    private WebCamTexture webcamTexture;

	// Use this for initialization
	void Start () {
        webcamTexture = new WebCamTexture();

        camTextMaterial.mainTexture = webcamTexture;
        webcamTexture.Play();

	}
	
	// Update is called once per frame
	void Update () {
        float pos = (camera.nearClipPlane + 0.01f);
        transform.position = camera.transform.position + camera.transform.forward * pos;
        float h = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2.0f;
        transform.localScale = new Vector3(h * camera.aspect, h, 1.0f);
	}
}
