using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.XR.ARSubsystems;

public class CameraMovement : MonoBehaviour
{
    public Vector3 offset; // offset position of camera from the spine position
    public ARTrackedImageManager imageManager; // Reference to ARTrackedImageManager
    private Transform spineTransform; // positional information of the spine
    private const string SPINE_IMAGE_NAME = "obama"; // Name of the reference image for the spine


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
        if (trackedImage.referenceImage.name == SPINE_IMAGE_NAME)
        {
            spineTransform = trackedImage.transform;
        }
    }


    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        transform.position = spineTransform.position + offset;
    }
}
