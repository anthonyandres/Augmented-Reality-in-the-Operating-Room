using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.XR.ARSubsystems;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class ARTrackingCSVNewFile : MonoBehaviour
{
    public ARTrackedImageManager imageManager;
    //public TMP_Text distanceText, toolTrackingStateText, spineTrackingStateText;
    //public TMP_Text toolPositionText, spinePositionText;
    public GameObject toolObject, spineObject;
    public LineRenderer lineRenderer;

    // OFFSET VALUES
    public float toolxDistance; // horizontal distance from the centre of card to tip of tool
    public float toolyDistance; // vertical distance from the centre of card to tip of tool
    public float toolzDistance; // depth between the centre of card to tip of tool

    public float spinexDistance; // horizontal distance from the centre of card to middle of spine
    public float spineyDistance; // vertical distance from the centre of card to middle of spine
    public float spinezDistance; // depth between the centre of card to middle of spine

    private TrackingState toolTrackingState, spineTrackingState;
    private const string TOOL_IMAGE_NAME = "toolcard", SPINE_IMAGE_NAME = "spinecard";

    private bool toggleOn = true; //line render TRUE on default
    // private bool rollEnable = false;
    // private bool pitchEnable = false;
    // private int pcounter = 0;
    // private int rcounter = 0;

    // private Vector3 r1, r2, p1, p2;
    // private int rollState, pitchState;

    // private string csvFilePath;
    // private float nextSampleTime = 0f;
    // private const float sampleInterval = 1f / 60f; // 60 samples per second

    void Start()
    {
        // // Generate a unique filename with timestamp for each session
        // string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        // csvFilePath = Path.Combine(Application.persistentDataPath, $"ARTrackingData_{timestamp}.csv");

        // // Write CSV header
        // File.WriteAllText(csvFilePath, "Time,Distance,Tool_X,Tool_Y,Tool_Z,Spine_X,Spine_Y,Spine_Z\n");
    }

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateTrackedObject(trackedImage);
        }
    }

    private void UpdateTrackedObject(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage.name == TOOL_IMAGE_NAME)
        {
            toolObject.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
            toolTrackingState = trackedImage.trackingState;
        }
        else if (trackedImage.referenceImage.name == SPINE_IMAGE_NAME)
        {
            spineObject.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
            spineTrackingState = trackedImage.trackingState;
        }
    }

    void Update()
    {
        if (toolObject != null && spineObject != null)
        {
            Vector3 toolCenter = toolObject.transform.position;//toolObject.GetComponent<Renderer>().bounds.center;
            Vector3 spineCenter = spineObject.transform.position;//spineObject.GetComponent<Renderer>().bounds.center;

            // rc.text = rcounter.ToString("F2");
            // pc.text = pcounter.ToString("F2");
            // if(rollState == 0){
            //     toolModelTip = new Vector3(toolCenter.x+toolxDistance, toolCenter.y+toolyDistance, toolCenter.z+toolzDistance);
            // }
            // else if(rollState == 1){
            //     r1 = new Vector3(toolCenter.x, toolCenter.y, toolCenter.z); //take current tool position
            // }
            // else if(rollState == 2){
            //     r2 = new Vector3(toolCenter.x, toolCenter.y, toolCenter.z); //take roll rotated position
            //     float rollOffset = -2*(r2.z - r1.z);
            //     toolModelTip = new Vector3(r2.x+toolxDistance, r2.y+toolyDistance, r2.z+toolzDistance+rollOffset);
            // }

            // if(pitchState == 0){
            //     toolModelTip = new Vector3(toolCenter.x+toolxDistance, toolCenter.y+toolyDistance, toolCenter.z+toolzDistance);
            // }
            // if(pitchState == 1){
            //     p1 = new Vector3(toolCenter.x, toolCenter.y, toolCenter.z); //take current tool position
            // }
            // else if(pitchState == 2){
            //     p2 = new Vector3(toolCenter.x, toolCenter.y, toolCenter.z); //take pitch rotated position
            //     float pitchOffset = -2*(p2.z - p1.z);
            //     toolModelTip = new Vector3(p2.x+toolxDistance, p2.y+toolyDistance, p2.z+toolzDistance+pitchOffset);
            // }

            // if(rollEnable){ //if rotated sideways, reverse
            //     toolModelTip = new Vector3(toolCenter.x+toolxDistance, toolCenter.y+toolyDistance, -(toolCenter.z+toolzDistance));
            // }
            // else if(pitchEnable){ //if rotated forward, reverse
            //     toolModelTip = new Vector3(-(toolCenter.x+toolxDistance), toolCenter.y+toolyDistance, toolCenter.z+toolzDistance);
            // }
            // else{ //if user has not notified of any rotation, okay!
            //    toolModelTip = new Vector3(toolCenter.x+toolxDistance, toolCenter.y+toolyDistance, toolCenter.z+toolzDistance); //add offset values to tool/spine center accordingly (we want tool tip and specific spine part) 
            // }
            
            Vector3 toolModelTip = new Vector3(toolCenter.x+toolxDistance, toolCenter.y+toolyDistance, toolCenter.z+toolzDistance); //add offset values to tool/spine center accordingly (we want tool tip and specific spine part) 
            //toolModelTip = toolObject.transform.rotation * toolModelTip;
            Vector3 spineModelCenter = new Vector3(spineCenter.x+spinexDistance, spineCenter.y+spineyDistance, spineCenter.z+spinezDistance);
            //float distance = Vector3.Distance(toolCenter, spineCenter);

            // Update UI
            // distanceText.text = $"Distance: {distance:F2} meters";
            // toolTrackingStateText.text = $"Tool State: {toolTrackingState}";
            // spineTrackingStateText.text = $"Spine State: {spineTrackingState}";
            // toolPositionText.text = $"Tool Pos: {toolCenter.x:F2}, {toolCenter.y:F2}, {toolCenter.z:F2}";
            // spinePositionText.text = $"Spine Pos: {spineCenter.x:F2}, {spineCenter.y:F2}, {spineCenter.z:F2}";

            // Update line renderer
            if (lineRenderer != null)
            {
                if(toggleOn)
                {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, toolModelTip); 
                lineRenderer.SetPosition(1, spineModelCenter);
                }
                else
                {
                    lineRenderer.positionCount = 0;
                }
            }

            // Sample and log data every 1/60 seconds
            // if (Time.time >= nextSampleTime)
            // {
            //     LogDataToCSV(Time.time, distance, toolCenter, spineCenter);
            //     nextSampleTime = Time.time + sampleInterval;
            // }
        }
        else
        {
            // distanceText.text = "Waiting to track tool and spine...";
            // toolTrackingStateText.text = "Tool State: ";
            // spineTrackingStateText.text = "Spine State: ";
            // toolPositionText.text = "Tool Pos: -";
            // spinePositionText.text = "Spine Pos: -";

            if (lineRenderer != null)
            {
                if(toggleOn)
                {
                lineRenderer.positionCount = 0;
                }
            }
        }
    }

    // private void LogDataToCSV(float timestamp, float distance, Vector3 toolPos, Vector3 spinePos)
    // {
    //     string dataLine = $"{timestamp:F3},{distance:F3},{toolPos.x:F3},{toolPos.y:F3},{toolPos.z:F3},{spinePos.x:F3},{spinePos.y:F3},{spinePos.z:F3}\n";
    //     File.AppendAllText(csvFilePath, dataLine);
    // }

    // void OnDrawGizmos()
    // {
    //     if (toolObject != null && spineObject != null)
    //     {
    //         Vector3 toolCenter = toolObject.transform.position;//toolObject.GetComponent<Renderer>().bounds.center;
    //         Vector3 spineCenter = spineObject.transform.position;//spineObject.GetComponent<Renderer>().bounds.center;
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawLine(toolCenter, spineCenter);
    //     }
    // }

    public void onToggleLinePress()
    {
        toggleOn = !toggleOn; //if line render is on, turn off; Vice Versa
    }

    // public void onRollPress()
    // {
    //     pitchEnable = false;
    //     rollEnable = !rollEnable;
    //     // pcounter = 0;
    //     // rcounter++;
    //     //     if(rcounter == 1){
    //     //         rollState = 1;
    //     //     }
    //     //     else if(rcounter == 2){
    //     //         rollState = 2;
    //     //     }
    //     //     else if(rcounter == 3){
    //     //         rcounter = 0;
    //     //     }
    // }

    // public void onPitchPress()
    // {
    //     rollEnable = false;
    //     pitchEnable = !pitchEnable;
    //     // rcounter = 0;
    //     // pcounter++;
    //     //     if(pcounter == 1){
    //     //         pitchState = 1;
    //     //     }
    //     //     else if(pcounter == 2){
    //     //         pitchState = 2;
    //     //     }
    //     //     else if(pcounter == 3){
    //     //         pcounter = 0;
    //     //     }
    // }
}
