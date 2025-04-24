using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;

public class CameraSwitch : MonoBehaviour
{
    public Camera phoneCamera; // Reference to main phone camera
    public Camera spineCamera; // Reference to spine camera child
    public Button cameraSwitch; // Reference to UI element button
    public TextMeshProUGUI testText; //test text to remove later, used to test functionality of button if onClick should be in Start() or Update()

    private int buttonPresses = 0;

    // Start is called before the first frame update
    void Start()
    {
        phoneCamera.enabled = true;
        spineCamera.enabled = false;
        cameraSwitch.clicked += ButtonPress;
    }

    void ButtonPress()
    {
        phoneCamera.enabled = !phoneCamera.enabled;
        spineCamera.enabled = !spineCamera.enabled;
        testText.text = "BUTTON PRESSES: " + buttonPresses;
        Debug.Log("BUTTON PRESSES: " + buttonPresses);
        buttonPresses++;

    }

    // Update is called once per frame
    void Update()
    {
        //cameraSwitch.clicked += ButtonPress;
    }
}
