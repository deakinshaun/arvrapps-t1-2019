using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SiteSelector : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    public Canvas siteCanvas;

    private Image siteUI;
    private Button siteButton;
    private Text siteText;

    // Start is called before the first frame update
    void Start()
    {
        siteCanvas.enabled = false;

        siteUI = siteCanvas.transform.Find("MovingAnchor").gameObject.transform.Find("SiteUI").gameObject.GetComponent<Image>();
        siteButton = siteCanvas.transform.Find("MovingAnchor").gameObject.transform.Find("EnterSiteButton").gameObject.GetComponent<Button>();
        siteText = siteUI.transform.Find("ScrollView").gameObject.transform.Find("Viewport").gameObject.transform.Find("Content").gameObject.transform.Find("UIText").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            RaycastCheck();
        }
    }

    void RaycastCheck()
    {
        RaycastHit hit;

        // TODO:Change next line to supprt touch on mobile devices.
        Ray ray = camera.ScreenPointToRay(Input.GetTouch(0).position);

        // If it hits a building (Collision box)
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                siteCanvas.enabled = true;

                // Move UI corner next to the selected building
                MoveUI(Input.GetTouch(0).position);

                // If the line hits the wall of the construction site
                if (hit.collider != null && hit.collider.gameObject.tag == "CSite")
                {
                    // Set text
                    siteText.text = "Construction Site\n\n" + "Completed:\n" + "- No stages currently completed" + "\n\nUncompleted:" + "\n- Architect" + "\n- Builder" + "\n- Interior Designer";
                    // Set image size
                    siteUI.rectTransform.sizeDelta = new Vector2(siteUI.rectTransform.sizeDelta.x, 170.0f);
                    // Enable button and set colour to white
                    siteButton.enabled = true;
                    var colours = siteButton.colors;
                    colours.normalColor = new Color(1.0f, 1.0f, 1.0f);
                    siteButton.colors = colours;
                }
                // Else it hits the wall of a normal building
                else
                {
                    // Set text
                    siteText.text = "Completed Building\n\nThis building has already been constructed and approved by \"BurwoodBuilding PTY LTD\"";
                    // Set image size
                    siteUI.rectTransform.sizeDelta = new Vector2(siteUI.rectTransform.sizeDelta.x, 130.0f);
                    // Disable button and set disabled colour
                    siteButton.enabled = false;
                    var colours = siteButton.colors;
                    colours.normalColor = new Color(0.55f, 0.55f, 0.55f);
                    siteButton.colors = colours;
                }
            }
        }
        // Else if it doesn't hit a building and Is not currently on the UI itself then remove UI
        else if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            siteCanvas.enabled = false;
        }
    }

    void MoveUI(Vector3 position)
    {
        Transform UIAnchor = siteCanvas.transform.Find("MovingAnchor");
        UIAnchor.position = position;

        int middleScreenX = Screen.width / 2;
        int middleScreenY = Screen.height / 2;

        // Move UI position X away from right edge of screen (Move left)
        if (UIAnchor.position.x > middleScreenX)
            UIAnchor.position = new Vector3(UIAnchor.position.x - 190.0f, UIAnchor.position.y, UIAnchor.position.z);
        // Move UI position X awau from left edge of screen (Move right)
        else if (UIAnchor.position.x < middleScreenX)
            UIAnchor.position = new Vector3(UIAnchor.position.x + 190.0f, UIAnchor.position.y, UIAnchor.position.z);

        // Move UI position Y away from top edge of screen (Move down)
        if (UIAnchor.position.y > middleScreenY)
            UIAnchor.position = new Vector3(UIAnchor.position.x, UIAnchor.position.y - 130.0f, UIAnchor.position.z);
        // Move UI position Y away from bottom edge of screen (Move up)
        else if (UIAnchor.position.y < middleScreenY)
            UIAnchor.position = new Vector3(UIAnchor.position.x, UIAnchor.position.y + 130.0f, UIAnchor.position.z);

    }
}
