using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;

public class Client : MonoBehaviour
{
	private static TcpClient client;
	private static NetworkStream stream;

	[SerializeField] string host = "localhost";
	[SerializeField] int port = 5000;

	private void Start()
	{
		Connect(host, port);
		
	}

	private void OnDestroy()
	{
		Disconnect(host, port);
	}

	public static float[] SendAndRequest(float[] data)
	{
		Send(data);
		return Receive();
	}

	public static void Connect(string host, int port)
	{
		client = new TcpClient(host, port);
		stream = client.GetStream();
	}

	public static void Disconnect(string host, int port)
	{
		stream.Close();
		client.Close();
	}

	public static void Send(float[] data)
	{
		// Pack the data into a byte array using the Marshal class
		byte[] packedData = new byte[data.Length * sizeof(float)];
		Buffer.BlockCopy(data, 0, packedData, 0, packedData.Length);

		// Send the packed data over the network stream
		stream.Write(packedData, 0, packedData.Length);
	}

	public static float[] Receive()
	{
		// Receive the packed data over the network stream
		byte[] packedData = new byte[client.ReceiveBufferSize];
		int bytesRead = stream.Read(packedData, 0, client.ReceiveBufferSize);

		// Unpack the data into a float array using the Marshal class
		float[] data = new float[bytesRead / sizeof(float)];
		Buffer.BlockCopy(packedData, 0, data, 0, bytesRead);

		return data;
	}

	

}