using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class HandGesture : MonoBehaviour
{
    public GameObject PointHolder;
    private MLHandKeyPose[] _gestures;

    // Start is called before the first frame update
    void Start()
    {
        MLHands.Start();
        _gestures = new MLHandKeyPose[5];
        _gestures[0] = MLHandKeyPose.Ok;
        _gestures[1] = MLHandKeyPose.Finger;
        _gestures[2] = MLHandKeyPose.OpenHandBack;
        _gestures[3] = MLHandKeyPose.Fist;
        _gestures[4] = MLHandKeyPose.Thumb;
        MLHands.KeyPoseManager.EnableKeyPoses(_gestures, true, false);

    }


    private void OnDestroy()
    {
        MLHands.Stop();
    }


    // Update is called once per frame
    void Update()
    {
        
        if (GetGesture(MLHands.Right, MLHandKeyPose.OpenHandBack) )
        {
            PointHolder.transform.position = (MLHands.Right.Wrist.KeyPoints[0].Position) + new Vector3(-1.5F, -0.2F, 2.5F); ;
        }
        if (GetGesture(MLHands.Right, MLHandKeyPose.Fist))
        {
            PointHolder.transform.position = PointHolder.transform.position;
        }
        if (GetGesture(MLHands.Left, MLHandKeyPose.OpenHandBack))
        {
            PointHolder.transform.localScale += new Vector3(0.01F, 0.01F, 0.01F);
        }
        if (GetGesture(MLHands.Left, MLHandKeyPose.Fist))
        {
            PointHolder.transform.localScale -= new Vector3(0.01F, 0.01F, 0.01F);
        }
        if (GetGesture(MLHands.Left, MLHandKeyPose.Ok))
        {
            PointHolder.transform.Rotate(Vector3.down, +20.0f * Time.deltaTime);
        }
    

    }

    //Get confidence level (HIGH)
    private bool GetGesture(MLHand hand, MLHandKeyPose type)
    {
        if (hand != null)
        {
            if (hand.KeyPose == type)
            {
                if (hand.KeyPoseConfidence > 0.9f)
                {
                    return true;
                }
            }
        }
        return false;
    }
 }
