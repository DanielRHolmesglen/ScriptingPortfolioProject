using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexFollow : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        // This is a bit of a cheat to figure out how far away the camera is from the ground. It works since the ground
        // is at (0, 0, 0)
        var mousePosition = Input.mousePosition;
        var cameraDistance = new Vector3(mousePosition.x, mousePosition.y, _camera.transform.position.magnitude);
        var position = _camera.ScreenToWorldPoint(cameraDistance);
        position.y = 1.5f;
        transform.position = position;
    }
}
