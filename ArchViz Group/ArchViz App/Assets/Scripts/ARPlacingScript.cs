using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARPlacingScript : MonoBehaviour
{
    public GameObject model;
    public GameObject base_plane;    //Creating a prefab of a workable plane
    private ARSessionOrigin ar_origin;    //Creating an instance of the ARSessionIrigin which is the placing point of the model
    private Pose placement_pose;          //Indidcator and position of the models placement
    private bool placement_pose_valid = false;     //Creating a boolena to check if there is a suitable plane to place the model
    public GameObject placement_indicator;
    public Vector3 touch_distance;      //DIstance to be moved by 2 finger touch
    public float rotation_speed = 0.01f;        //Rotational speed of the moel. Needs to be a very low value like 0.01
    public float scale_factor = 0.001f;     //Float to control the scaling factor as its too fast

    GameObject model_placed;

    void Start()
    {
        ar_origin = FindObjectOfType<ARSessionOrigin>();   //Creating an instance as soon as thr app starts
    }

   
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (placement_pose_valid && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)  //Only place object is surface is valid, user touching the screen and only on the inital tap
        {
            PlaceModel();
        }
        UpdateModel();        //Function to rotate and scale the model
    }

    private void UpdateModel()
    {
        if(Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
        { 
            Touch touch_zero = Input.GetTouch(0);      //Storing touch one
            Touch touch_one = Input.GetTouch(1);       //Storing touch 2
            Vector3 touch_zero_distance = touch_zero.position - touch_zero.deltaPosition;        //Calculating the difference between the first finger touches
            Vector3 touch_one_distance = touch_one.position - touch_one.deltaPosition;           //Calculating the difference between the second finger touches
            touch_distance = new Vector3((touch_zero_distance.x + touch_one_distance.x / 2), (touch_zero_distance.y + touch_one_distance.y / 2), 0.0f);           //Making an average of both finger displacement for better accuracy
            model_placed.transform.Rotate(0, touch_distance.x * rotation_speed,0, Space.Self);       //Changing the rortation of the model in the y axis by using a 2 finger touch horizontal swipe
            model_placed.transform.localScale += new Vector3 (touch_distance.y * scale_factor, touch_distance.y * scale_factor, touch_distance.y * scale_factor);           //Changing the scale of the model by using 2 finger touch in the vertical direction finger swipe
            //Scaling needs a better way like a UI slider for better control. 
        }
    }

    private void PlaceModel()
    {   
        model_placed = Instantiate(model, placement_pose.position, placement_pose.rotation);
        //Instantiate(base_plane, placement_pose.position, placement_pose.rotation);   //Creating a plane underneath the model on tap
    }

    private void UpdatePlacementIndicator()
    {
        if (placement_pose_valid)
        {
            placement_indicator.SetActive(true);       //Only show indicator if the surface is present 
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
        var screen_centre = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));    // Variable passed on tp raycast which converts the cnetre of the camera view to screen points 
        var hits = new List<ARRaycastHit>();       //Creating a new empty list to store if the virtual raycast hits anything in the real world
        ar_origin.Raycast(screen_centre, hits, TrackableType.Planes);

        //If no suitable surface found, cannot place object
        placement_pose_valid = hits.Count>0;



        if(placement_pose_valid)
        {
            placement_pose = hits[0].pose;   //Setting the placement pose to the suitable position found

            placement_pose.position += touch_distance;

            var camera_forward = Camera.current.transform.forward;       //Get the forward direction of the camera

            var camera_xz = new Vector3(camera_forward.x, 0, camera_forward.z).normalized;   //New vector to ignore y axis of camera

            placement_pose.rotation = Quaternion.LookRotation(camera_xz);     //Use the camera xz to change the direction of the placement indicator 
            

        }
    }
}
