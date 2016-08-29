using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tracking : MonoBehaviour {


    public Quaternion lookAngle;

    //public Text speedText;

    float initOrienntationX;
    float initOrienntationY;
    float initOrienntationZ;

    // Use this for initialization
    void Start () {

        Input.gyro.enabled = true;

        //float initOrienntationX = Input.gyro.rotationRateUnbiased.x;
        //float initOrienntationY = Input.gyro.rotationRateUnbiased.y;
        //float initOrienntationZ = -Input.gyro.rotationRateUnbiased.z;

        //  lookVector = Vector3.zero;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        this.transform.Rotate(0, initOrienntationY - Input.gyro.rotationRateUnbiased.y, 0);
        this.transform.Rotate(initOrienntationX - Input.gyro.rotationRateUnbiased.x, 0, initOrienntationZ + Input.gyro.rotationRateUnbiased.z);
    }
}
