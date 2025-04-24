using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.XR.ARSubsystems;
using System.Numerics;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class distanceCalculator : MonoBehaviour
{
    public TMP_Text distanceText; // UI Text element for distance display
    public TMP_Text vector; // UI text elemet for live vector display
    public TMP_Text vector2; // UI text elemet for live vector display
    public TMP_Text coord1; // UI text element for first tool coordinates
    public TMP_Text coord2; // UI text element for second tool coordinates
    public TMP_Text toolDistanceText; // UI text element for tool distance measurement for calibration
    public float toolxDistance; // horizontal distance from the centre of card to tip of tool
    public float toolyDistance; // vertical distance from the centre of card to tip of tool
    public float toolzDistance; // depth between the centre of card to tip of tool

    public float spinexDistance; // horizontal distance from the centre of card to middle of spine
    public float spineyDistance; // vertical distance from the centre of card to middle of spine
    public float spinezDistance; // depth between the centre of card to middle of spine

    private float distance; // final calculation

    private int counter = 0;
    private float toolDistance;
    private Vector3 pose1;
    private Vector3 pose2;


    public ARTrackedImageManager imageManager; // Reference to ARTrackedImageManager
    private Transform toolTransform; // Transform of the tool
    private Transform spineTransform; // Transform of the spine

    private TrackingState toolTrackingState; // Image tracking state of the tool
    private TrackingState spineTrackingState; // Image tracking state of the spine

    // vectors for calculations
    private Vector3 a;
    private Vector3 b;
    private Vector3 c;

    private float x1,x2,y1,y2,z1,z2,xx,yy,zz,dis;

    private Vector3 spineTransformed;

    private const string TOOL_IMAGE_NAME = "toolcard"; // Name of the reference image for the tool
    private const string SPINE_IMAGE_NAME = "spinecard"; // Name of the reference image for the spine

    // Start is called before the first frame update
    void Start()
    {
        a = Vector3.zero;
        b = new Vector3(toolxDistance, toolyDistance, toolzDistance);
        c = b - a;
        
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

    private float CalculateDistance(Transform toolTransform, Transform spineTransform)
    {   
        // x1 = toolTransform.position.x-spineTransform.position.x+xDistance;
        // x2 = xDistance;
        // y1 = toolTransform.position.y-spineTransform.position.y+yDistance;
        // y2 = yDistance;
        // z1 = toolTransform.position.z-spineTransform.position.z+zDistance;
        // z2 = zDistance;

        x1 = toolTransform.position.x+toolxDistance;
        x2 = spineTransform.position.x+spinexDistance;
        y1 = toolTransform.position.y+toolyDistance;
        y2 = spineTransform.position.y+spineyDistance;
        z1 = toolTransform.position.z+toolzDistance;
        z2 = spineTransform.position.z+spinezDistance;

        xx = (x2-x1)*(x2-x1);
        yy = (y2-y1)*(y2-y1);
        zz = (z2-z1)*(z2-z1);
        dis = Mathf.Sqrt(xx+yy+zz);
        return dis;
    }

    private float CalculateDistanceFromPhone(Transform toolTransform, Transform spineTransform)
    {
        float a = Vector3.Distance(toolTransform.position, spineTransform.position);
        float angle = Vector3.Angle(toolTransform.position, spineTransform.position);
        float b = a/Mathf.Tan(angle);
        float c = b/Mathf.Sin(angle/2);
        return b;
    }

    // Update is called once per frame
    void Update()
    {
        // distance = Mathf.Sqrt(c.x * c.x + c.y * c.y + c.z * c.z);
        distance = CalculateDistance(toolTransform, spineTransform);
        distanceText.text = "Tool-Spine Distance: " + distance.ToString("F2") + " meters";
        //spineTransformed = spineTransform.position;
        //spineTransformed += UnityEngine.Vector3.forward; 
        //vector.text = "spine vector: <"+spineTransformed.x+", "+spineTransformed.y+", "+spineTransformed.z+">";
        //vector2.text = "spine vector: <"+spineTransform.position.x+", "+spineTransform.position.y+", "+spineTransform.position.z+">";

        float distanceFromPhone = CalculateDistanceFromPhone(toolTransform, spineTransform);
        //vector.text = "Tool Distance From Phone: " + distanceFromPhone + " meters";
    }

    public void toolCalibrateButton()
    {
        counter++;
        if(counter == 1){//if first button press, obtain the first pose
            pose1 = toolTransform.position;
            coord1.text = pose1.ToString("F2");
            coord2.text = "";
            toolDistanceText.text = "";
        }
        else if(counter == 2){
            counter = 0;
            pose2 = toolTransform.position;
            toolDistance = Vector3.Distance(pose1, pose2);
            coord2.text = pose2.ToString("F2");
            toolDistanceText.text = "Calibration Distance: " + toolDistance.ToString("F2") + " meters";
        }
    }
}
