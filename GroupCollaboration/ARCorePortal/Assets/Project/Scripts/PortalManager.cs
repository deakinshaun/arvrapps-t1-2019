using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalManager : MonoBehaviour {

    public GameObject MainCamera;

    public GameObject PortalScene;

    private Material[] PortalStruct;


	// Use this for initialization
	void Start () {
        PortalStruct = PortalScene.GetComponent<Renderer>().sharedMaterials;

        for (int i = 0; i < PortalStruct.Length; ++i)
        {
            PortalStruct[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
        }
    }
	
	// Update is called once per frame
	void OnTriggerStay (Collider collider) {

        //Vector3 camPositionInPortalSpace = transform.InverseTransformPoint(MainCamera.transform.position);
        Vector3 camPositionInPortalSpace1 = this.transform.position - MainCamera.transform.position;
        float z = camPositionInPortalSpace1.z;

        Debug.Log("camPositionInPortalSpace.z = " + z);

        if (z > -0.6f) //very close or already entered portal
        {
            //Disable stencil test
            for (int i = 0; i < PortalStruct.Length; ++i)
            {
                PortalStruct[i].SetInt("_StencilComp", (int)CompareFunction.Always);
            }
        }
        else
        {
            //Eable stencil test
            for (int i = 0; i < PortalStruct.Length; ++i)
            {
                PortalStruct[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }
        }
	}

    /*
    //set it to not show the panorama scene when spawned
    private void OnEnable()
    {
        //Disable stencil test
        for (int i = 0; i < PortalStruct.Length; ++i)
        {
            PortalStruct[i].SetInt("_StencilComp", 3);
        }
    }
    */


    ////set it to not show the panorama scene when disabled
    private void OnDisable()
    {
        //Disable stencil test
        for (int i = 0; i < PortalStruct.Length; ++i)
        {
            PortalStruct[i].SetInt("_StencilComp", 3);
        }
    }

}
