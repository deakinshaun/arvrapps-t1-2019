using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.UI;
using System;

public class ARPlacingScript : MonoBehaviour
{
    public Image image;

    public GameObject model;
    //Creating a prefab of a workable plane
    public GameObject base_plane;    

    public GameObject placement_indicator;
    //Distance to be moved by 2 finger touch
    public Vector3 touch_distance;
    //Rotational speed of the moel. Needs to be a very low value like 0.01
    public float rotation_speed = 0.01f;
    //Float to control the scaling factor as its too fast
    public float scale_factor = 0.001f;     

    //Creating an instance of the ARSessionIrigin which is the placing point of the model
    private ARSessionOrigin ar_origin;    
    //Indidcator and position of the models placement
    private Pose placement_pose;          
    //Creating a boolean to check if there is a suitable plane to place the model
    private bool placement_pose_valid = false;     
    private GameObject model_placed;
    private bool _isPlaced = false;

    void Start()
    {
        //Creating an instance as soon as thr app starts
        ar_origin = FindObjectOfType<ARSessionOrigin>();   
    }
   
    void Update()
    {
        // If model is not already placed
        if (!_isPlaced)
        {
            PlaceModel();
        }
            
  
        //Function to rotate and scale the model
        UpdateModel();        
    }

    private void UpdateModel()
    {
        if(Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Touch touch_zero = Input.GetTouch(0);      //Storing touch one
            Touch touch_one = Input.GetTouch(1);       //Storing touch 2
            StartCoroutine(ScaleModel(touch_zero, touch_one));
        }
    }

    IEnumerator ScaleModel(Touch touch_zero, Touch touch_one)
    {
        //Calculating the difference between the first finger touches
        Vector3 touch_zero_distance = touch_zero.position - touch_zero.deltaPosition;
        //Calculating the difference between the second finger touches
        Vector3 touch_one_distance = touch_one.position - touch_one.deltaPosition;
        //Making an average of both finger displacement for better accuracy
        touch_distance = new Vector3((touch_zero_distance.x + touch_one_distance.x / 2), (touch_zero_distance.y + touch_one_distance.y / 2), 0.0f);
        //Changing the rortation of the model in the y axis by using a 2 finger touch horizontal swipe
        model_placed.transform.Rotate(0, touch_distance.x * rotation_speed, 0, Space.Self);
        //Changing the scale of the model by using 2 finger touch in the vertical direction finger swipe
        model_placed.transform.localScale += new Vector3(touch_distance.y * scale_factor, touch_distance.y * scale_factor, touch_distance.y * scale_factor);

        //TODO: Scaling needs a better way like a UI slider for better control. 

        yield return new WaitForEndOfFrame();
    }

    private void PlaceModel()
    {
        // Start Plane Tracking
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        //Only place object is surface is valid, user touching the screen and only on the inital tap
        if (placement_pose_valid && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            model_placed = Instantiate(model, placement_pose.position, placement_pose.rotation);
            _isPlaced = true;
            
            int count = GameObject.FindGameObjectWithTag("Model").transform.childCount;
            GameObject.FindGameObjectWithTag("DrawingManager").transform.SetParent(
            GameObject.FindGameObjectWithTag("Model").transform);
            if (GameObject.FindGameObjectWithTag("Model").transform.childCount > count)
            {
                image.color = Color.green;
            }
        }
        //Creating a plane underneath the model on tap
        //Instantiate(base_plane, placement_pose.position, placement_pose.rotation);   
    }

    private void UpdatePlacementIndicator()
    {
        if (placement_pose_valid)
        {
            //Only show indicator if the surface is present 
            placement_indicator.SetActive(true);      
            Debug.Log("Surface Found");
            placement_indicator.transform.SetPositionAndRotation(placement_pose.position, placement_pose.rotation);
        }
        else
        {
            placement_indicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        // Variable passed on tp raycast which converts the cnetre of the camera view to screen points 
        var screen_centre = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        //Creating a new empty list to store if the virtual raycast hits anything in the real world
        var hits = new List<ARRaycastHit>();       
        ar_origin.Raycast(screen_centre, hits, TrackableType.Planes);

        //If no suitable surface found, cannot place object
        placement_pose_valid = hits.Count>0;

        if(placement_pose_valid)
        {
            //Setting the placement pose to the suitable position found
            placement_pose = hits[0].pose;   

            placement_pose.position += touch_distance;

            //Get the forward direction of the camera
            var camera_forward = Camera.current.transform.forward;
            //New vector to ignore y axis of camera
            var camera_xz = new Vector3(camera_forward.x, 0, camera_forward.z).normalized;
            //Use the camera xz to change the direction of the placement indicator 
            placement_pose.rotation = Quaternion.LookRotation(camera_xz);    
        }
    }
}
