using UnityEngine;

// used so the rotation caused by animations doesn't cause enemy spherecast to be off
// the spherecase uses the orientation fo the enemy's center
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
