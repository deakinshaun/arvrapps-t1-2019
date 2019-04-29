using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour {

    public Material phyCamMaterial;
    public Material virCamMaterial;
    public GameObject transition;
    //public GameObject portalEntry;

    public bool inPhysical = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (inPhysical)
        {
            phyCamMaterial.SetInt("_Stencil_Level", 1);
            virCamMaterial.SetInt("_Stencil_Level", 0);
        }
        else
        {
            phyCamMaterial.SetInt("_Stencil_Level", 0);
            virCamMaterial.SetInt("_Stencil_Level", 1);

        }

    }

    public void OnTriggerEnter(Collider col)
    {
        //Find portal entry for world transition
        if (col.gameObject == GameObject.Find("doorway"))
        {

            transition.SetActive(true);
            Debug.Log("Changing UNIVERSE");
            inPhysical = !inPhysical;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        transition.SetActive(false);
    }

}

