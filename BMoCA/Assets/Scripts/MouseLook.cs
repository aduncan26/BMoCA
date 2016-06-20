using UnityEngine;
using System.Collections;

public class MouseLook: MonoBehaviour {

    public float sensitivityX = 5f;
    public float sensitivityY = 5f;
    public float minX = -360f;
    public float maxX = 360f;
    public float minY = -90f;
    public float maxY = 90f;
    public float lerpSpeed = 1f;
    public enum TurnAxis
    {
        RotateOnX,
        RotateOnZ
    }
    public TurnAxis axis = TurnAxis.RotateOnX;
    Vector3 xRotVector;
    float rotationX;
    float rotationY;
    Quaternion originalRotation;
    public bool lerp = false;

    Transform trans;

	public bool resetLookX = false;
	public bool resetLookY = false;
	public float resetLookSpeed = 0.97f;

//	float highestValue = 0;

	// Use this for initialization
	void Awake () {
        trans = GetComponent<Transform>();

        if(axis == TurnAxis.RotateOnX)
        {
            xRotVector = Vector3.up;
        }
        else
        {
            xRotVector = -Vector3.forward;
        }
	}

    void OnEnable()
    {
		if (!lerp) {
			originalRotation = trans.localRotation;

			rotationX = 0;
			rotationY = 0;
		} else {
			rotationX = trans.localEulerAngles.y;
			rotationY = trans.localEulerAngles.x;
		}
    }
	
	// Update is called once per frame
	void Update () {
        RotationXY();
		//Debug.Log (Input.GetAxis ("Mouse X"));
		//Debug.Log (rotationX);
		//Debug.Log (Time.deltaTime);
	}

    void RotationXY()
    {
		if (Mathf.Abs (Input.GetAxis ("Mouse X")) > 0f || !resetLookX) {
			rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
		} else {
			rotationX *= resetLookSpeed; //* Time.deltaTime / 0.02f;
		}
		if(Mathf.Abs(Input.GetAxis("Mouse Y")) > 0f || !resetLookY){
			rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
		} else{
			rotationY *= resetLookSpeed;// * Time.deltaTime / 0.02f;
		}
        rotationX = ClampAngle(ref rotationX, minX, maxX);
        rotationY = ClampAngle(ref rotationY, minY, maxY);
        
        if (lerp)
        {
			

//			rotationX = Mathf.Clamp (rotationX, -100, 100);
//
//			if (Mathf.Abs (rotationX) > highestValue) {
//				highestValue = Mathf.Abs (rotationX);
//				Debug.Log (highestValue);
//			}

			Quaternion newTarget;
			if (axis == TurnAxis.RotateOnX) {
				newTarget = Quaternion.Euler (-rotationY, rotationX, 0f);
			} else {
				newTarget = Quaternion.Euler (-rotationY, 0f, -rotationX);
			}
			trans.localRotation = Quaternion.Slerp(trans.localRotation, newTarget, lerpSpeed * Time.deltaTime);
        }
        else
        {
			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, xRotVector);
			Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
    
    }

    public static float ClampAngle(ref float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }
        if (angle > 360F)
        {
            angle -= 360F;
        }
        return Mathf.Clamp(angle, min, max);
    }


}
