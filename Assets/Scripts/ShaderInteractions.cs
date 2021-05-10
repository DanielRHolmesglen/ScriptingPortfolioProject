using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script controls the shader using UI buttons
/// </summary>
public class ShaderInteractions : MonoBehaviour
{
    [SerializeField] Renderer r;
    [SerializeField] bool CROneRunning, CRTwoRunning;

    public void ShaderInteractionOne()
    {
        r.material.SetColor("_BaseColour", Random.ColorHSV());
        Debug.Log("interaction one");
    }
    public void ShaderInteractionTwo()
    {
        if (!CROneRunning) StartCoroutine(Dissolve());
        Debug.Log("interaction two");
    }
    public void ShaderInteractionThree()
    {
        if (!CROneRunning) StartCoroutine(Reappear());
        Debug.Log("interaction three");
    }

    IEnumerator Dissolve()
    {
        CROneRunning = true;

        while (r.material.GetFloat("_DissolveRate") < 1)
        {
            r.material.SetFloat("_DissolveRate", r.material.GetFloat("_DissolveRate") + 0.01f);
            yield return new WaitForEndOfFrame();
        }

        CROneRunning = false;
    }
    IEnumerator Reappear()
    {
        CROneRunning = true;

        while (r.material.GetFloat("_DissolveRate") > 0)
        {
            r.material.SetFloat("_DissolveRate", r.material.GetFloat("_DissolveRate") - 0.01f);
            yield return new WaitForEndOfFrame();
        }

        CRTwoRunning = false;
    }

}

