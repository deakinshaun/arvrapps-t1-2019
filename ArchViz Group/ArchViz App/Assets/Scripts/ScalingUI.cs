using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingUI : MonoBehaviour
{
    GameObject placedModel;

    [SerializeField]
    Vector3 storedScale;
    [SerializeField]
    bool is_scaleStored = false;

    [SerializeField]
    float duration_Of_Lerp = 1f;
    bool is_scale_buttonUP = false;
    bool is_rotate_buttonUP = false;

    public void SetModel()
    {
        if (MainManager.instance.MainModel == null)
            placedModel = GameObject.FindGameObjectWithTag("Model").gameObject;
        else
            placedModel = MainManager.instance.MainModel;
    }

    public void UnlockScale()
    {
        is_scaleStored = false;
        transform.GetChild(5).gameObject.SetActive(false);
    }

    public void StoreScale()
    {
        if (!is_scaleStored)
        {
            storedScale = placedModel.transform.localScale;
            // Store in the manager script also
            //MainManager.instance.StoredScale = storedScale;
            is_scaleStored = true;
            transform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            // Lerp to stored scale
            StartCoroutine(LerpToStoredScale());
        }
    }

    IEnumerator LerpToStoredScale()
    {
        float scaleDuration = 2;
        Vector3 currentScale = placedModel.transform.localScale;
       
        for (float t = 0; t < 1; t += Time.deltaTime / scaleDuration)
        {
            placedModel.transform.localScale = Vector3.Lerp(currentScale, storedScale, t);
            yield return new WaitForEndOfFrame();
        }
    }

    #region Rotation Functions
    public void RotateButtonDown(bool right)
    {
        StartCoroutine(SmoothRotation(right));
    }

    public void RotateButtonUp()
    {
        is_rotate_buttonUP = true;
    }

    IEnumerator SmoothRotation(bool right)
    {
        Vector3 rotateSide;
        if (right)
            rotateSide = new Vector3(0.0f, 10.0f * Time.deltaTime, 0.0f);
        else
            rotateSide = new Vector3(0.0f, -10.0f * Time.deltaTime, 0.0f);
        while (!is_rotate_buttonUP)
        {
            placedModel.transform.Rotate(rotateSide, Space.Self);
            yield return new WaitForEndOfFrame();
        }
        is_rotate_buttonUP = false;
        yield return null;
    }
    #endregion

    #region Scaling functions
    public void ScaleButtonDown(bool up)
    {
        StartCoroutine(SmoothScaling(up));
    }

    public void ScaleButtonUp()
    {
        is_scale_buttonUP = true;
    }

    IEnumerator SmoothScaling(bool up)
    {
        Vector3 targetScale;
        if (up)
        {
            targetScale = new Vector3(1.0f, 1.0f, 1.0f);
            duration_Of_Lerp = 10;
            //targetScale = new Vector3(0.001f, 0.001f, 0.001f);
        }
        else
        {
            targetScale = new Vector3(0.01f, 0.01f, 0.01f);
            duration_Of_Lerp = 1f;
            //targetScale = new Vector3(-0.001f, -0.001f, -0.001f);
        }
            

        float LerpStartTime = Time.deltaTime;
        while (!is_scale_buttonUP)
        {
            //Vector3 modelScale = placedModel.transform.localScale;
            //placedModel.transform.localScale += targetScale;
            float SinceLerpStart = Time.deltaTime - LerpStartTime;
            float percentageComplete = SinceLerpStart / (duration_Of_Lerp);
            placedModel.transform.localScale = Vector3.Lerp(placedModel.transform.localScale,
                targetScale, percentageComplete);

            yield return null;
            //yield return new WaitForEndOfFrame();
        }
        is_scale_buttonUP = false;
        yield return null;
    }
    #endregion
}
