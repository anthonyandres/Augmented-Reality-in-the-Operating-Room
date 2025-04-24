using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenesDefault : MonoBehaviour
{
    public void GoToMain()
    {
        SceneManager.LoadScene("Tracking");
    }
}
