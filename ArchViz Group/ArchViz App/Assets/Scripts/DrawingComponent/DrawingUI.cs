using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingUI : MonoBehaviour
{
    [SerializeField]
    private DrawingManager drawingManager;

    bool viewOn = false;

    // UI elements
    [SerializeField, Space]
    private GameObject DrawButton;
    [SerializeField]
    private GameObject ViewButton;
    [SerializeField, Space]
    private GameObject RedoButton;


    void Start()
    {
        // Get the refernece of the FrawingManager
        drawingManager = GameObject.FindWithTag("DrawingManager").GetComponent<DrawingManager>();
        if(drawingManager == null)
            Debug.Log("DrawingManager refernece not set in DrawingUI");

        // Get all the UI elements
        DrawButton = transform.GetChild(0).gameObject;
        ViewButton = transform.GetChild(1).gameObject;
        RedoButton = transform.GetChild(2).gameObject;

        // Inititalse the scene
        drawingManager.IsDrawWater = false;
    }

    public void OnClickDraw()
    {
        // If electricity then switch to water
        if (!drawingManager.IsDrawWater)
        {
            Debug.Log("Switching to Water");
            // Switch 
            drawingManager.IsDrawWater = true;

            // Enable water line renderer
            drawingManager.DisplayWLineRender(true);
            // Disable electricity line renderer
            drawingManager.DisplayELineRender(false);

            //--------------------------------------------
            // UI tasks

            // Switch Water Image to Electricity Image 
            DrawButton.transform.GetChild(0).gameObject.SetActive(true);
            DrawButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        // If water then switch to electricity
        else
        {
            // Switch 
            drawingManager.IsDrawWater = false;

            Debug.Log("Switching to Electricity");
            // Disable water line renderer
            drawingManager.DisplayELineRender(true);
            // Enable electricity line renderer
            drawingManager.DisplayWLineRender(false);

            //--------------------------------------------
            // UI tasks

            // Switch Electricity Image to water Image 
            DrawButton.transform.GetChild(0).gameObject.SetActive(false);
            DrawButton.transform.GetChild(1).gameObject.SetActive(true);
        }
        viewOn = false;
    }

    public void OnRedoButton()
    {
        drawingManager.ErasePreviousPoint();
    }

    public void OnClickView()
    {
        viewOn = !viewOn;

        if (viewOn)
        {
            if (!drawingManager.IsDrawWater)
            {
                drawingManager.DisplayWLineRender(true);
            }
            else
            {
                drawingManager.DisplayELineRender(true);
            }
            ViewButton.transform.GetChild(1).gameObject.SetActive(false);
            ViewButton.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            if (!drawingManager.IsDrawWater)
            {
                drawingManager.DisplayWLineRender(false);
            }
            else
            {
                drawingManager.DisplayELineRender(false);
            }
            ViewButton.transform.GetChild(1).gameObject.SetActive(true);
            ViewButton.transform.GetChild(2).gameObject.SetActive(false);
        }

        
    }
}
