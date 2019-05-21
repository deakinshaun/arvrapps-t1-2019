using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;
using System.Linq;

//#if UNITY_EDITOR
//using input = GoogleARCore.InstantPreviewInput;
//#endif

public class ARController : MonoBehaviour {

    //A list of planes which ARCore detected in the current frame
    private List<TrackedPlane> m_NewTrackedPlanes = new List<TrackedPlane>();

    public GameObject GridPrefab;

    //The portal scene
    public GameObject Portal;

    public GameObject PanoramaSphere;

    //the user camera
    public GameObject ARCamera;

    //Notification text
    public Text TextNotification;
    private bool touched = false;

    public Button closePortal;

    Renderer panoRender;

    public Texture2D[] PanoTextures;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        userNotification(touched);

        //check arcore session status
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        //Fill m_NewTrackedPlanes with the planes that ARCore detected in the current frame
        Session.GetTrackables<TrackedPlane>(m_NewTrackedPlanes, TrackableQueryFilter.New);

        //
        for (int i = 0; i < m_NewTrackedPlanes.Count; i++)
        {
            GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, transform);

            //Set the position of grid and modify the vertices of the attached mesh
            grid.GetComponent<GridVisualiser>().Initialize(m_NewTrackedPlanes[i]);
        }

        //check if the user touched the screen
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            touched = true;
            return;
        }
        else
        {
            touched = false;
        }



        //Now check if the user touched any of the tracked planes
        TrackableHit hit;
        if (Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out hit))
        {
            
            //Now place the portal on the top of the tracked plane user touched

            //enable portal
            Portal.SetActive(true);

            //Create a new Anchor
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

            //Set the portal position to where user hit
            Portal.transform.position = hit.Pose.position;
            Portal.transform.rotation = hit.Pose.rotation;

            //Let the portal face the camera
            Vector3 cameraPosition = ARCamera.transform.position;

            //The portal should only rotate on Y axis
            cameraPosition.y = hit.Pose.position.y;

            //Rotate the portal to face the camera
            Portal.transform.LookAt(cameraPosition, Portal.transform.up);

            //Attach the portal to anchor
            Portal.transform.parent = anchor.transform;
        }

    }

    public void userNotification(bool t)
    {

        if (t && Portal.activeSelf)
        {
            TextNotification.text = "Portal Spawned!";
        }
        else if (!Portal.activeSelf)
        {
            TextNotification.text = "Detecting planes... Tap a grid to open portal"; ;
        }

    }

    public void ClosePortal()
    {
        Portal.SetActive(false);
    }

    int i = 0;
    public void nextPanorama()
    {
        ++i;
        panoRender = PanoramaSphere.GetComponent<Renderer>();
        panoRender.sharedMaterial.EnableKeyword("_MainTex");
        panoRender.sharedMaterial.SetTexture("_MainTex", PanoTextures[Mathf.Abs(i % PanoTextures.Length)]);
    }

    public void previousPanorama()
    {
        --i;
        panoRender = PanoramaSphere.GetComponent<Renderer>();
        panoRender.sharedMaterial.EnableKeyword("_MainTex");
        panoRender.sharedMaterial.SetTexture("_MainTex", PanoTextures[Mathf.Abs(i % PanoTextures.Length)]);
    }


}

