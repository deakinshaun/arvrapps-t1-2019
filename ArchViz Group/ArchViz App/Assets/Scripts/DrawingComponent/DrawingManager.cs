﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingManager : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private bool isDrawWater;
    [SerializeField]
    private DrawingUI drawingUI;


    // Number of electricity line points
    [SerializeField, Space]
    private int EPoints = 0;
    // List which contains all the electricity markers
    [SerializeField]
    private List<GameObject> EList = new List<GameObject>();

    // GameObject Electricity Line Marker Prefab
    [SerializeField]
    private GameObject EMarker;
    // Line Renderer for Eletricity Lines
    [SerializeField]
    private LineRenderer ElineRenderer;


    // Number of water line points
    [SerializeField, Space]
    private int WPoints = 0;
    // List which contains all the water markers
    [SerializeField]
    private List<GameObject> WList = new List<GameObject>();

    // GameObject Electricity Line Marker Prefab
    [SerializeField]
    private GameObject WMarker;
    // Line Renderer for Eletricity Lines
    [SerializeField]
    private LineRenderer WlineRenderer;

    public bool IsDrawWater { get => isDrawWater; set => isDrawWater = value; }

    private void Start()
    {
        if(ElineRenderer == null || EMarker == null)
        {
            Debug.Log("Electricity Line Renderer or Marker not attached to script");
        }
        if(WlineRenderer == null || WMarker == null)
        {
            Debug.Log("Water Line Renderer or Marker not attached to script");
        }

        // Setup UI
    }

    void Update()
    {
        if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            RaycastCheck();
        }
    }

    // Fuction to raycast from the screen to world
    void RaycastCheck()
    {
        RaycastHit hit;
        // TODO:Change next line to supprt touch on mobile devices.
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            if(EventSystem.current.IsPointerOverGameObject() == false)
            {
                Debug.Log(hit.collider.name + " was hit.");

                // If the line hits the wall
                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    // Debug raycast to check
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

                    // Add the hit position as an element ot the line renderer
                    CreatePointMarker(hit.point);
                }
            }
        }
    }

    void CreatePointMarker(Vector3 pointPosition)
    {
        if (isDrawWater)
        {
            // Instantiate a pointerMarker on Hit point
            GameObject wm = Instantiate(WMarker, pointPosition, Quaternion.identity,
                this.transform.GetChild(0).transform);

            // Add it to the list of markers
            WList.Add(wm);
        }
        else
        {
            // Instantiate a pointerMarker on Hit point
            GameObject em = Instantiate(EMarker, pointPosition, Quaternion.identity,
                this.transform.GetChild(1).transform);

            // Add it to the list of markers
            EList.Add(em);
        }
        // Display in Line Renderer
        LineRender();
    }

    // Function to redraw the the lineRender on every new added point
    void LineRender()
    {
        if (isDrawWater)
        {
            WlineRenderer.positionCount = 0;
            WlineRenderer.positionCount = WList.Count;
            for (int i = 0; i < WList.Count; i++)
            {
                WlineRenderer.SetPosition(i, WList[i].transform.position);
            }

            // Enable if the points need to be hidden
            /*
            foreach(GameObject go in WList)
            {
                go.SetActive(false);
            }
            */
        }
        else
        {
            ElineRenderer.positionCount = 0;
            ElineRenderer.positionCount = EList.Count;
            for (int i = 0; i < EList.Count; i++)
            {
                ElineRenderer.SetPosition(i, EList[i].transform.position);
            }

            // Enable if the points need to be hidden
            /*
            foreach (GameObject go in EList)
            {
                go.SetActive(false);
            }
            */
        }
    }

    public void ErasePreviousPoint()
    {
        // Remove previous point from list
        if (isDrawWater)
        {
            Destroy(WList[WList.Count - 1]);
            WList.RemoveAt(WList.Count - 1);
        }
        else
        {
            Destroy(EList[EList.Count - 1]);
            EList.RemoveAt(EList.Count - 1);
        }
            
        // Redisplay line render
        LineRender();
    }

    public void DisplayWLineRender(bool status)
    {
        // Disable Line Render
        WlineRenderer.gameObject.SetActive(status);

        // Disable all the points in the list
        this.transform.GetChild(0).gameObject.SetActive(status);
    }

    public void DisplayELineRender(bool status)
    {
        // Disable Line Render
        ElineRenderer.gameObject.SetActive(status);

        // Disable all the points in the list
        this.transform.GetChild(1).gameObject.SetActive(status);
    }
}
