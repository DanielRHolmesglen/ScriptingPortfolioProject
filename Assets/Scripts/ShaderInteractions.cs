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
        r.material.SetColor("Color_A105B5F", Random.ColorHSV());
        Debug.Log("interaction one");
    }
    public void ShaderInteractionTwo()
    {
        if (!CROneRunning && !CRTwoRunning) StartCoroutine(Dissolve());
        Debug.Log("interaction two");
    }
    public void ShaderInteractionThree()
    {
        if (!CRTwoRunning && !CROneRunning) StartCoroutine(Solve());
        Debug.Log("interaction three");
    }

    IEnumerator Dissolve()
    {
        CROneRunning = true;
        while (r.material.GetFloat("_dis_Rate") < 1)
        {
            r.material.SetFloat("_dis_Rate", r.material.GetFloat("_dis_Rate") + 0.01f);
            yield return new WaitForEndOfFrame();
        }
        CROneRunning = false;
    }
    IEnumerator Solve()
    {
        CRTwoRunning = true;
        while (r.material.GetFloat("_dis_Rate") > 0)
        {
            r.material.SetFloat("_dis_Rate", r.material.GetFloat("_dis_Rate") - 0.01f);
            yield return new WaitForEndOfFrame();
        }
        CRTwoRunning = false;
    }
}
