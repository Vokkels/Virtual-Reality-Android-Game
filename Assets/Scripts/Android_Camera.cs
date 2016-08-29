/*
    FJD MALAN
    25131699
    07/13/16
*/

using UnityEngine;
using System.Collections;

public class Android_Camera : MonoBehaviour {

    private WebCamTexture androidCamera;
   
    /*
    Changes the texture of a gameobject its attatched to,
    to a live camera feed.
        */
	void Start () {

        try {

            //Check webcam devices
            WebCamDevice[] cams = UnityEngine.WebCamTexture.devices;

            if (cams.Length >= 1)
            {
                androidCamera = new WebCamTexture();
                Renderer render = GetComponent<Renderer>();
                //render.material.mainTexture = androidCamera;
                this.GetComponent<Renderer>().material.mainTexture = androidCamera;
                androidCamera.Play();
            }
            else {
                Debug.Log("Error! -> No Camera Detected!");
            }
        }
        catch (System.Exception x)
        {
            Debug.Log(x.Message);
        }
       	}
    }
