using UnityEngine;
using System.Collections;

public class UI_Game : MonoBehaviour {

    public bool AR = true;
    public GameObject camera;

    public void toggleARVR()
    {
        if (AR)
        {
            camera.SetActive(true);
            AR = false;
        }
        else {

            camera.SetActive(false);
            AR = true;
        }

    }
}
