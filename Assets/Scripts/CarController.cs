using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CarController : MonoBehaviour
{
    [SerializeField] List<Transform> wheelPos;
    [SerializeField] List<Transform> visualWheels;
    [SerializeField] float visualWheelOffset = .2f;

    [SerializeField] float tireMass = 1.0f;

    [Header("Suspension")]
    [SerializeField] float suspensionSus = 20f;
    [SerializeField] float suspensionDamper = 10f;
    [SerializeField] float suspensionOffset = .5f;
    [SerializeField] float restpoint = 1f;

    [Header("Steering")]
    [SerializeField] float frontGrip = 1f;
    [SerializeField] float backGrip = 1f;
    [SerializeField] float curAngle = 0f;
    [SerializeField] float maxAngle = 45f;
    [SerializeField] float curDesiredAngle = 0f;
    [SerializeField] float angleSpeed = 1f;

    [Header("Acceleration")]
    [SerializeField] AnimationCurve torqueCurve;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float topSpeed = 10f;

    [Header("Debug")]
    [SerializeField] bool doSuspension = true;
    [SerializeField] bool doSteering = true;
    [SerializeField] bool doDrive = true;
    

    private Rigidbody rb;

    private float steerAngleT = 0f;
    private float accelInput = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	private void Update()
	{
        accelInput = Input.GetAxis("Vertical") * acceleration;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            steerAngleT = 0;

        steerAngleT += angleSpeed * Time.deltaTime;
        steerAngleT = Mathf.Clamp01(steerAngleT);

        if (Input.GetKey(KeyCode.A))
            curDesiredAngle = maxAngle;
        else if (Input.GetKey(KeyCode.D))
            curDesiredAngle = -maxAngle;
        else
            curDesiredAngle = 0;

        curAngle = Mathf.Lerp(curAngle, curDesiredAngle, angleSpeed);
        wheelPos[1].localRotation = Quaternion.Euler(90, 0, curAngle);
        wheelPos[3].localRotation = Quaternion.Euler(90, 0, curAngle);
		
        
	}
    
	void FixedUpdate()
    {
        for (int i = 0; i < wheelPos.Count; i++)
		{
			RaycastHit hit;
			bool bruh = Physics.Raycast(wheelPos[i].position, wheelPos[i].forward, out hit, restpoint + suspensionOffset);
			if (!bruh)
			    continue;

            visualWheels[i].localPosition = new Vector3(0, 0, hit.distance - visualWheelOffset);
            Debug.Log(hit.distance);

			if (doSuspension)
                DoSuspension(wheelPos[i], hit);

            if (doSteering)
            {
                if (i % 2 == 0)
                {
                    DoSteer(wheelPos[i], backGrip);
                }
                else
                {
                    DoSteer(wheelPos[i], frontGrip);
                }
            }

            if (doDrive)
                DoDrive(wheelPos[i], accelInput);


        }
	}

    void DoSuspension(Transform wheel, RaycastHit hit)
    {
		

		Vector3 wheelVel = rb.GetPointVelocity(wheel.position);
		Vector3 springDir = wheel.forward;

		float vel = Vector3.Dot(-springDir, wheelVel);
		float offset = restpoint - hit.distance;

		float force = (offset * suspensionSus) - (vel * suspensionDamper);

		rb.AddForceAtPosition(-springDir * force, wheel.position);
       
	}
    void DoSteer(Transform wheel, float grip)
    {
        Vector3 steerDir = wheel.up;
        
        Vector3 wheelVel = rb.GetPointVelocity(wheel.position);

        float steerVel = Vector3.Dot(steerDir, wheelVel);


        float desiredVelChange = -steerVel * grip;

        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

        rb.AddForceAtPosition(steerDir * tireMass * desiredAccel, wheel.position);
    }

    void DoDrive(Transform wheel, float accelInput)
    {
        Vector3 accelDir = wheel.right;

        float carSpeed = Vector3.Dot(transform.right, rb.velocity);

        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);

        float availableTorque = torqueCurve.Evaluate(normalizedSpeed) * accelInput;
        
        rb.AddForceAtPosition(accelDir * availableTorque, wheel.position);

    }
}
