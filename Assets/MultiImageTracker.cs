using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class MultiImageTracker : MonoBehaviour
{
    public TextMeshProUGUI text;
    private ARTrackedImageManager trackedImages;
    public GameObject[] ArPrefabsList;

    List<GameObject> ARObjects = new List<GameObject>();


    void Awake()
    {
        //text.text = "starting tracking...";
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        //text.text = "tracking enable";
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        //text.text = "tracking disable";
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged; 
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //text.text = "on tracked images changed";
        //instantiate prefabs
        foreach(var trackedImage in eventArgs.added)
        {
            foreach(var arPrefab in ArPrefabsList)
            {
                if(trackedImage.referenceImage.name == arPrefab.name)
                {
                    var newPrefab = Instantiate(arPrefab, trackedImage.transform);
                    ARObjects.Add(newPrefab);
                }
            }
        }

        //spawn prefabs when respective image is being tracked
        foreach(var trackedImage in eventArgs.updated)
        {
            foreach(var GameObject in ARObjects)
            {
                if(gameObject.name == trackedImage.name)
                {
                    if(gameObject.name == "spinecard")
                    {
                        //spawn camera and attach to spine transform
                        text.text = "spine detected";

                    }
                    gameObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
                }
            }
        }
    }
}
