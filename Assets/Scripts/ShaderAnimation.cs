using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderAnimation : MonoBehaviour
{
    private Material _material;
    private int _rotationTag;
    private int _colorTag;
    
    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        _rotationTag = Shader.PropertyToID("_CoolRotation");
        _colorTag = Shader.PropertyToID("_CoolColor");
    }

    private void Update()
    {
        _material.SetFloat(_rotationTag, _material.GetFloat(_rotationTag) + Time.deltaTime);
        // Outvars are disgusting. I'm not sure why this can't just return a tuple or something "normal"
        Color.RGBToHSV(_material.GetColor(_colorTag), out var h, out var s, out var v);
        _material.SetColor(_colorTag, Color.HSVToRGB(Mathf.Repeat(h + Time.deltaTime / 4f, 1f), s, v));
    }
}
