using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float planetMass = 1000.0f;
    public float sunMass = 100000.0f;

    public GameObject sun;

    float UniversalGravity = 6.667f * Mathf.Pow(10, -11);

    private Rigidbody rb;
    private Rigidbody sunRb;
    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 sunPos = sun.transform.position;
        float distance = Vector3.Distance(sunPos, transform.position);

        float gravityMagnitude = planetMass * sunMass / distance * UniversalGravity;
        Vector3 direction = Vector3.Normalize(sunPos - transform.position);

        Vector3 gravityForce = direction * gravityMagnitude;

        rb.AddForce(gravityForce);
    }
}
