using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroTrack : MonoBehaviour
{

    private Gyroscope gyro;
    private bool gyroSupported;

    // Use this for initialization
    void Start()
    {
        gyroSupported = SystemInfo.supportsGyroscope;
        if (gyroSupported)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gyroSupported)
        {
            transform.rotation = Quaternion.Euler(90, 0, 90) * gyro.attitude * Quaternion.Euler(180, 180, 0);
        }
    }
}
