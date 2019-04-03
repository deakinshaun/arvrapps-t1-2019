using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScalingRotation : MonoBehaviour
{

    public Vector3 horizontal_touch_distance;      //DIstance to be moved by 2 finger touch
    
    void Start()
    {
        
    }

    void Update()
    {
        UpdateHorizontalTouchDIstance();
    }

    private void UpdateHorizontalTouchDIstance()
    {
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Touch touch_zero = Input.GetTouch(0);
            Touch touch_one = Input.GetTouch(1);
            Vector3 touch_zero_distance = touch_zero.position - touch_zero.deltaPosition;
            Vector3 touch_one_distance = touch_one.position - touch_one.deltaPosition;
            horizontal_touch_distance = new Vector3((touch_zero_distance.x + touch_one_distance.x / 2), 0.0f, (touch_zero_distance.z + touch_one_distance.z / 2));
        }
    }
}
