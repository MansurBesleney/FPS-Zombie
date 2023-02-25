using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Transform target;

    void Awake()
    {
        target = GameObject.FindWithTag("CameraPoint").transform;
    }
     
    void Update()
    {
        if(target != null)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}
