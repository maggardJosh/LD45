using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceZAxis : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Vector3 tempPos = transform.position;
        tempPos.z = 0;
        transform.position = tempPos;
    }
}
