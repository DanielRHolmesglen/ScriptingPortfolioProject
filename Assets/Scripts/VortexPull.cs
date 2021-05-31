using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VortexPull : MonoBehaviour
{
    private List<Rigidbody> _toPull = new List<Rigidbody>();

    private void Start()
    {
        // I no longer care about performance. C# has hurt me.
        _toPull = GameObject.FindGameObjectsWithTag("Vortex")
            .Select(gameObject => gameObject.GetComponent<Rigidbody>())
            .ToList();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            foreach (var body in _toPull)
            {
                var offset = transform.position - body.position;
                var pullStrength = 13f - Mathf.Clamp(offset.magnitude, 0f, 13f);
                body.AddForce(offset.normalized * (pullStrength * 4.5f));
            }
        }
        else if (Input.GetMouseButton(1))
        {
            foreach (var body in _toPull)
            {
                body.AddExplosionForce(10f, transform.position, 20f);
            }
        }
    }
}