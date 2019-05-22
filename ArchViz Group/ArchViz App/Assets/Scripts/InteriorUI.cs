using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteriorUI : MonoBehaviour
{
    [SerializeField]
    private DrawingManager drawingManager;

    bool eView = true;
    bool wView = true;

    [SerializeField]
    private Button WaterButton;
    [SerializeField]
    private Button ElectricityButton;

    // Start is called before the first frame update
    void Start()
    {
        // Get the refernece of the FrawingManager
        drawingManager = GameObject.FindWithTag("DrawingManager").GetComponent<DrawingManager>();
        drawingManager.Rerender();
        if (drawingManager == null)
            Debug.Log("DrawingManager refernece not set in DrawingUI");
    }

    public void ToggleElectricity()
    {
        eView = !eView;
        if (eView)
        {
            drawingManager.DisplayELineRender(false);
            ElectricityButton.GetComponent<Image>().color = Color.white;
            return;
        }
        drawingManager.DisplayELineRender(true);
        ElectricityButton.GetComponent<Image>().color = Color.green;
    }

    public void ToggleWater()
    {
        wView = !wView;
        if (wView)
        {
            drawingManager.DisplayWLineRender(false);
            WaterButton.GetComponent<Image>().color = Color.white;
            return;
        }
        drawingManager.DisplayWLineRender(true);
        WaterButton.GetComponent<Image>().color = Color.green;
    }
}
