using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCameraSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        Screen.SetResolution(960, 640, false);
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.backgroundColor = GameSettings.BGColor;
    }
}
