using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentRotationController : MonoBehaviour
{
    public Transform trackedObject;

    void Update()
    {
        if (trackedObject != null) {
            transform.rotation = trackedObject.rotation;
            transform.position = trackedObject.position;
        }
    }
}
