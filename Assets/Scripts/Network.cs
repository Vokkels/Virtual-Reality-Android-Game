using UnityEngine;
using System.Collections;
using LAN_Networking;
using System;

public class Network : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        try
        {

            NetworkClient client = new NetworkClient();
        }
        catch (Exception x)
        {
            Debug.Log(x.Message);
        }
	}
}
