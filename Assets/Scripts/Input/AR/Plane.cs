using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Helpers;

public class Plane : MonoBehaviour
{
    public void DestroyObj()
    {
        Destroy(gameObject);
    }
    
    public void TriggerAlphaPlanes()
    {
        if (Helper.isPlay) HidePlanes();
        else ShowPlanes();
    }

    private void ShowPlanes()
    {
        ChangeAlpha(1);
    }

    private void HidePlanes()
    {
        ChangeAlpha(0);
    }

    private void ChangeAlpha(float a)
    {
        float r = GetComponent<MeshRenderer>().sharedMaterial.color.r;
        float g = GetComponent<MeshRenderer>().sharedMaterial.color.g;
        float b = GetComponent<MeshRenderer>().sharedMaterial.color.b;
        ChangeColor(r, g, b, a);
    }

    private void ChangeColor(float r, float g, float b, float a)
    {
        GetComponent<MeshRenderer>().sharedMaterial.color = Helper.VectorCreater(r, g, b, a);
    }
}
