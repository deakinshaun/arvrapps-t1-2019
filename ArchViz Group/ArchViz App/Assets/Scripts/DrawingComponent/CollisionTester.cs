using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        GetComponent<Rigidbody>().AddForce(Vector3.forward * 10f * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
       
        GetComponent<Rigidbody>().AddForce(Vector3.left * 10f * Input.GetAxis("Vertical"), ForceMode.Acceleration);

        if (Input.GetButtonDown("Jump"))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
