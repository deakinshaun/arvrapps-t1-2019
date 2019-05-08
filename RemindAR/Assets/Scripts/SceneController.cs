﻿namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using System.Collections;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;

#if UNITY_EDITOR
    using Input = InstantPreviewInput;
#endif

    public class SceneController : MonoBehaviour
    {
        public Camera               FirstPersonCamera;
        public GameObject           DetectedPlanePrefab;
        public GameObject           ARAndroidPrefab;
        public GameObject           SearchingForPlaneUI;
        private GameObject          ARObject;
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();
        private bool                m_IsQuitting = false;
        public static int           CurrentNumberOfGameObjects = 0;
        private int                 numberOfGameObjectsAllowed = 1;

        private TaskController TaskControllerObject;

        public void Update()
        {
            _UpdateApplicationLifecycle();
            _PlaneDetection();
            _InstantiateOnTouch();
        }

        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }
        private void _DoQuit()
        {
            Application.Quit();
        }
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
        public void _PlaneDetection()
        {
            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUI = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUI = false;
                    
                    break;
                }
            }

            SearchingForPlaneUI.SetActive(showSearchingUI);

        }
        public void _InstantiateOnTouch()
        {
        //    Touch touch;
            
        //touch = Input.GetTouch(0);

 
            if (Input.touchCount != 0)
            {
                _SpawnARObject();
            }


        }

        public void _SpawnARObject()
        {
            Touch touch;
            touch = Input.GetTouch(0);
            Debug.Log("touch count is " + Input.touchCount);
            TrackableHit hit;      // Raycast against the location the player touched to search for planes.
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch Began");
                if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
                {

                    if (CurrentNumberOfGameObjects == numberOfGameObjectsAllowed)
                    {
                        ARObject.GetComponent<TaskController>().RemoveContainers();
                        Destroy(ARObject);
                        CurrentNumberOfGameObjects = 0;
                    }
                    if (CurrentNumberOfGameObjects < numberOfGameObjectsAllowed)
                    {
                        Debug.Log("Screen Touched");
                        
                        // Use hit pose and camera pose to check if hittest is from the
                        // back of the plane, if it is, no need to create the anchor.
                        if ((hit.Trackable is DetectedPlane) &&
                            Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                                hit.Pose.rotation * Vector3.up) < 0)
                        {
                            Debug.Log("Hit at back of the current DetectedPlane");
                        }
                        else
                        {

                            ARObject = Instantiate(ARAndroidPrefab, hit.Pose.position, Quaternion.identity);// Instantiate Andy model at the hit pose.             
                            TaskControllerObject = ARObject.GetComponent<TaskController>();

                            // @TODO Modify the rotation of this so it faces upwards                                                                    
                            //ARObject.transform.Rotate(90, 0, 0, Space.Self);// Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                            ARObject.transform.LookAt(FirstPersonCamera.transform);

                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                            ARObject.transform.parent = anchor.transform;
                            CurrentNumberOfGameObjects = CurrentNumberOfGameObjects + 1;

                            // Hide Plane once ARObject is Instantiated 
                            foreach (GameObject Temp in DetectedPlaneGenerator.instance.PLANES) //RK
                            {
                                Temp.SetActive(false);
                            }
                            
                        }

                    }

                }

            }

        }

    }

}


