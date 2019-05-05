using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTrail : MonoBehaviour
{
    [SerializeField]
    Camera camera;

    void Update()
    {
        if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            //Plane objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);

            //Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //float rayDistance;
            //if (objPlane.Raycast(mRay, out rayDistance))
            //{
            //    this.transform.position = mRay.GetPoint(rayDistance);
            //}

            RaycastHit hit;
            //Ray ray = camera.ScreenPointToRay(Input.GetTouch(0).position);
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.collider.name + " was hit.");
                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    Debug.DrawRay(ray.origin, hit.point * -1, Color.yellow);
                    this.transform.position = hit.point;
                }
            }
        }
    }
}
