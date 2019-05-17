using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SiteSelector : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        
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
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Debug.Log(hit.collider.name + " was hit.");

                // If the line hits the wall
                if (hit.collider != null && hit.collider.gameObject.tag == "CSite")
                {
                    // Debug raycast to check
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

                    // Open Window
                    Debug.Log("Site Selected");
                }
            }
        }
    }
}
