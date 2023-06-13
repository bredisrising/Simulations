using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] List<Transform> wheelPos;
    [SerializeField] float suspensionSus = 20f;
    [SerializeField] float suspensionOffset = .5f;
    [SerializeField] float restpoint = 1f;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        foreach(Transform wheel in wheelPos) 
        {
            RaycastHit hit;
            Physics.Raycast(wheel.position, wheel.forward, out hit, 5);

            float force = (restpoint - hit.distance) / suspensionOffset * suspensionSus;

            force = Mathf.Clamp(force, -suspensionSus, suspensionSus);
            rb.AddForceAtPosition(-wheel.forward * force, wheel.position);

            //Debug.Log(force);
            //Debug.Log(hit.distance);
        }
           
    }
}
