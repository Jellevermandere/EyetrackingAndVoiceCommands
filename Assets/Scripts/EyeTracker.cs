using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeTracker : MonoBehaviour
{
    enum cameraPos
    {
        left,
        right,
        up,
        down
    }

    //[HideInInspector]
    public Transform leftEye, rightEye, targetSphere;
    public Image testSphere;
    public Text debugText;

    [Tooltip("The device screensize in m")]
    [SerializeField]
    private Vector2 screenSize;

    [Tooltip("The device screensize extra margin")]
    [SerializeField]
    private float screenBleed = 1.2f;

    [Tooltip("The device orientation")]
    [SerializeField]
    private cameraPos cameraPosition;

    [Tooltip("the plane to cast the ray to")]
    [SerializeField]
    private BoxCollider screenPlane;

    [Tooltip("the collision mask for the eye tracking")]
    [SerializeField]
    private LayerMask eyeTrackMask;

    private float scaleFactor;

    public Vector2 eyeTrackPoint = Vector2.zero;



    // Start is called before the first frame update
    void Start()
    {
        setscreenplane();

        
    }

    // Update is called once per frame
    void Update()
    {


        if (leftEye != null && rightEye != null)
        {
            castEyeRays();
        }
        else SearchEyes();
    }

    void SearchEyes()
    {
        if(GameObject.FindGameObjectWithTag("LeftEye") != null)
        {
            leftEye = GameObject.FindGameObjectWithTag("LeftEye").transform;
        }
        if (GameObject.FindGameObjectWithTag("RightEye") != null)
        {
            rightEye = GameObject.FindGameObjectWithTag("RightEye").transform;
        }
       
    }

    Vector2 castEyeRays()
    {
        RaycastHit hitL;
        RaycastHit hitR;
        Vector2 screenPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 screenPercentages = Vector2.one / 2f;
        Vector3 averageHitPoint = screenPlane.transform.position;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(leftEye.position, leftEye.TransformDirection(Vector3.back), out hitL, Mathf.Infinity, eyeTrackMask))
        {
            Debug.DrawRay(leftEye.position, leftEye.TransformDirection(Vector3.back) * hitL.distance, Color.yellow);
            averageHitPoint = hitL.point;
        }
        if (Physics.Raycast(rightEye.position, rightEye.TransformDirection(Vector3.back), out hitR, Mathf.Infinity, eyeTrackMask))
        {
            Debug.DrawRay(rightEye.position, rightEye.TransformDirection(Vector3.back) * hitR.distance, Color.yellow);
            averageHitPoint = hitR.point;
        }

        averageHitPoint = (hitL.point + hitR.point) / 2f;
        //targetSphere.position = averageHitPoint;

        Vector3 relativeDistanceToCenter = screenPlane.transform.position - averageHitPoint;
        Vector3 xyPosition = Quaternion.Inverse(screenPlane.transform.rotation) * relativeDistanceToCenter;

        Vector2 scaledXYPosition = new Vector2(-xyPosition.x, -xyPosition.y) / scaleFactor;
        eyeTrackPoint = new Vector2(Screen.width, Screen.height) / 2f + scaledXYPosition;
        testSphere.rectTransform.position = eyeTrackPoint;

        debugText.text = (xyPosition * 100 + " & " + scaledXYPosition + " & " + testSphere.rectTransform.position);
        Debug.Log(xyPosition + " & " + testSphere.rectTransform.position);


        return screenPosition;
    }

    void setscreenplane()
    {
        screenPlane.size = screenSize * screenBleed;


        switch (cameraPosition){
            case cameraPos.left:
                screenPlane.transform.localPosition += Vector3.left * screenSize.x / 2; 

                break;

            case cameraPos.right:
                screenPlane.transform.localPosition += Vector3.right * screenSize.x / 2;

                break;

            case cameraPos.up:
                screenPlane.transform.localPosition += Vector3.down * screenSize.y / 2;

                break;

            case cameraPos.down:
                screenPlane.transform.localPosition += Vector3.up * screenSize.y / 2;

                break;

        }

        scaleFactor = screenSize.y / Screen.height;
        Debug.Log(scaleFactor);
    }
}
