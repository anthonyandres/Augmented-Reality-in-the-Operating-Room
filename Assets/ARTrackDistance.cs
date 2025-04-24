using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.XR.ARSubsystems;
using System.Numerics;
using System.Xml.Schema;

public class ARTrackDistance : MonoBehaviour
{
    public ARTrackedImageManager imageManager; // Reference to ARTrackedImageManager
    public TMP_Text distanceText; // UI Text element for distance display
    public TMP_Text toolTrackingStateText; // UI Text element for tool image tracking state (None, Limited, Tracking)
    public TMP_Text spineTrackingStateText; // UI Text element for spine image tracking state (None, Limited, Tracking)
    public TMP_Text testText; //DELETE THIS LATER, ONLY USED FOR TESTING SAKE
    private Transform toolTransform; // Transform of the tool
    private Transform spineTransform; // Transform of the spine

    private TrackingState toolTrackingState; // Image tracking state of the tool
    private TrackingState spineTrackingState; // Image tracking state of the spine

    private const string TOOL_IMAGE_NAME = "toolcard"; // Name of the reference image for the tool
    private const string SPINE_IMAGE_NAME = "spinecard"; // Name of the reference image for the spine

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
        foreach (var trackedImage in eventArgs.added)
        {
            AssignTrackedObject(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            AssignTrackedObject(trackedImage);
        }
    }

    private void AssignTrackedObject(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage.name == TOOL_IMAGE_NAME)
        {
            toolTransform = trackedImage.transform;
            toolTrackingState = trackedImage.trackingState;
        }
        else if (trackedImage.referenceImage.name == SPINE_IMAGE_NAME)
        {
            spineTransform = trackedImage.transform;
            spineTrackingState = trackedImage.trackingState;
        }
    }

    void Update()
    {
        if (toolTransform != null && spineTransform != null)
        {
            float distance = UnityEngine.Vector3.Distance(toolTransform.position, spineTransform.position);
            distanceText.text = "Card Center Distance: " + distance.ToString("F2") + " meters";

            // float angle = UnityEngine.Vector3.Angle(toolTransform.position, spineTransform.position);
            // testText.text = "Angle: " + angle.ToString("F2") + "\u00B0";
            
            // float x1 = toolTransform.position.x;
            // float x2 = spineTransform.position.x;
            // float y1 = toolTransform.position.y;
            // float y2 = spineTransform.position.y;
            // float z1 = toolTransform.position.z;
            // float z2 = spineTransform.position.z;
            // float xx = (x2-x1)*(x2-x1);
            // float yy = (y2-y1)*(y2-y1);
            // float zz = (z2-z1)*(z2-z1);
            // float Dis = Mathf.Sqrt(xx+yy+zz);
            // testText.text = "Calculated Euclidean Distance: " + Dis.ToString("F2") + " meters";

            toolTrackingStateText.text = "Tool State: " + toolTrackingState.ToString();
            spineTrackingStateText.text = "Spine State: " + spineTrackingState.ToString();
        }
        else
        {
            distanceText.text = "Waiting to track tool and spine...";
            toolTrackingStateText.text = "Tool State: ";
            spineTrackingStateText.text = "Spine State: ";
        }

        

    }
}