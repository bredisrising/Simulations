using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Balancer : MonoBehaviour
{
    [SerializeField] Transform ball;

	private void Start()
	{
		
	}

	private void Update()
	{
		float[] pos = new float[] { ball.transform.position.x, ball.transform.position.y, ball.transform.position.z };

		Client.Send(pos);
	}

}
