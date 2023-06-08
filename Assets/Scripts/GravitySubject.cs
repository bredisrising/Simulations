using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySubject : MonoBehaviour
{
    static List<GravitySubject> gravitySubjects;
    Rigidbody rb;

    [SerializeField] Vector3 initialVelocity = Vector3.zero;

	private void OnEnable()
	{
        if (gravitySubjects == null)
        {
			gravitySubjects = new List<GravitySubject>();
		}
		gravitySubjects.Add(this);
	}

	void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationY;

        //Vector3 initialVelocity = new Vector3(Random.Range(-300f, 300f), 0, Random.Range(-300f, 300f));
        rb.AddForce(initialVelocity, ForceMode.VelocityChange);
    }

	private void FixedUpdate()
	{
	    foreach (GravitySubject subject in gravitySubjects)
        {
            if (subject != this)
                DoGravity(subject);   
        }	
	}


    void DoGravity(GravitySubject subject)
    {
        Vector3 direction = subject.rb.position - rb.position;
        float distance = direction.magnitude;

        float gForce = (subject.rb.mass * rb.mass) / Mathf.Pow(distance, 2) * Constants.G;

        Vector3 gVector = direction.normalized * gForce;

        rb.AddForce(gVector);
    }
}
