using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Simple class for fixing aspect ratio. 
/// TODO: expand to 2 cameras; the UI / game view of the person using the program and the camera for screenshotting the scene / Kadinsky Shapes.
/// </summary>
public class CamManager : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    float aspectratio;
    // Start is called before the first frame update
    void Start()
    {
        cam.aspect = aspectratio;
    }

    
}
