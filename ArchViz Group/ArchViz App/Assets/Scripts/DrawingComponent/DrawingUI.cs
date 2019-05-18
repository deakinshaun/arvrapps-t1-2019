using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingUI : MonoBehaviour
{
    [SerializeField]
    private DrawingManager drawingManager;

    // Water UI elements
    [SerializeField, Space]
    private GameObject WDrawButton;
    [SerializeField]
    private GameObject WViewButton;

    // Electricity elements
    [SerializeField, Space]
    private GameObject EDrawButton;
    [SerializeField]
    private GameObject EViewButton;

    void Start()
    {
        // Get the refernece of the FrawingManager
        drawingManager = GameObject.FindWithTag("DrawingManager").GetComponent<DrawingManager>();
        if(drawingManager == null)
            Debug.Log("DrawingManager refernece not set in DrawingUI");

        // Get all the UI elements
        WDrawButton = transform.GetChild(0).gameObject;
        WViewButton = transform.GetChild(1).gameObject;
        EDrawButton = transform.GetChild(2).gameObject;
        EViewButton = transform.GetChild(3).gameObject;

        // Inititalse the scene
        drawingManager.IsDrawWater = false;
    }

    public void OnClickWaterDraw()
    {
        if (drawingManager.IsDrawWater)
        {
            // Delete previous point
            drawingManager.ErasePreviousPoint();
        }
        else
        {
            // Switch to Water Draw
            drawingManager.IsDrawWater = true;

            // Enable water line renderer
            drawingManager.DisplayWLineRender(true);
            // Disable electricity line renderer
            drawingManager.DisplayELineRender(false);

            //--------------------------------------------
            // UI tasks

            // Set electricity view toggle to disabled
            EViewButton.GetComponent<Toggle>().isOn = false;

            //Disable WView Toggle
            WViewButton.gameObject.SetActive(false);
            // Enable EView Toggle
            EViewButton.gameObject.SetActive(true);

            // Set background color to green
            WDrawButton.GetComponent<Image>().color = Color.green;
            // Set background color of electricity draw to white
            EDrawButton.GetComponent<Image>().color = Color.white;

            // Switch Image to Redo Image 
            WDrawButton.transform.GetChild(0).gameObject.SetActive(false);
            WDrawButton.transform.GetChild(1).gameObject.SetActive(true);
            Debug.Log("Getting some fucking thing right");

            // Switch Electricity Redo logo
            EDrawButton.transform.GetChild(0).gameObject.SetActive(true);
            EDrawButton.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void OnClickElectricityDraw()
    {
        if (!drawingManager.IsDrawWater)
        {
            // Delete previous point
            drawingManager.ErasePreviousPoint();
        }
        else
        {
            // Switch to Water Draw
            drawingManager.IsDrawWater = false;

            // Enable electricity line renderer
            drawingManager.DisplayELineRender(true);
            // Disable water line renderer
            drawingManager.DisplayWLineRender(false);

            //--------------------------------------------
            // UI tasks

            // Set water view toggle to disabled
            WViewButton.GetComponent<Toggle>().isOn = false;

            // Disable EView Toggle
            EViewButton.gameObject.SetActive(false);
            // Enable WView Toggle
            WViewButton.gameObject.SetActive(true);

            // Set background color to green
            EDrawButton.GetComponent<Image>().color = Color.green;
            // Set background color of water draw to white
            WDrawButton.GetComponent<Image>().color = Color.white;

            // Switch Image to Redo Image 
            EDrawButton.transform.GetChild(0).gameObject.SetActive(false);
            EDrawButton.transform.GetChild(1).gameObject.SetActive(true);
            // Switch Electricity Redo logo
            WDrawButton.transform.GetChild(0).gameObject.SetActive(true);
            WDrawButton.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void OnClickWaterView(bool status)
    {
        drawingManager.DisplayWLineRender(status);
        WViewButton.transform.GetChild(1).gameObject.SetActive(!status);
    }

    public void OnClickElectricityView(bool status)
    {
        drawingManager.DisplayELineRender(status);
        EViewButton.transform.GetChild(1).gameObject.SetActive(!status);
    }
}
