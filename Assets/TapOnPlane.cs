using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
//using UnityEditor.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.XR.ARSubsystems;
//using UnityEngine.InputSystem;
using TMPro;

public class TapOnPlane : MonoBehaviour
{
    [SerializeField]
    //private GameObject prefab;
    public TMP_Text distanceText; // UI Text element for distance display
    public TMP_Text confirmation;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private int counter = 0;
    private float distance;
    private Pose pose1;
    private Pose pose2;


    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown; //subcribing to the onFingerDown event using the function FingerDown
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown; //unsubcribing to the onFingerDown event (stop listening) 
    }

    private void FingerDown(EnhancedTouch.Finger finger) //When a finger is pressed on the screen, this function is called
    {
        confirmation.text = "Finger Down!";
        if (finger.index !=0) return; //if there are multiple fingers on the screen at once, this function is not called
        confirmation.text = "Finger HIT!";

        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon)) //cast a ray against the plane, true if the racast hits the plane
        {
            counter++;
            if(counter == 1){
               pose1 = hits[0].pose;
               confirmation.text = "pose1 hit!";
            }
            else if(counter == 2){
                counter = 0;
                pose2 = hits[0].pose;
                confirmation.text = "pose2 hit!";
                distance = Vector3.Distance(pose1.position, pose2.position);
            }
            
            
            // foreach(ARRaycastHit hit in hits){ //for every raycast hit...
            //     Pose pose = hit.pose; //get the pose of the successful hit, we can get the position and orientation of where ever we hit on the plane
                
            // }
        }
    }

    void Update()
    {
        distanceText.text = "Selected Points Distance: " + distance.ToString("F2") + " meters";
    }
}
