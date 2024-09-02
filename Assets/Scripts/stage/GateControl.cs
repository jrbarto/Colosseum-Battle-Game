using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateControl : MonoBehaviour
{
    Vector3 startPosition;
    int gateDirection = 0;
    int maxHeight = 16;
    float secondsToLerp = 5;
    float secondsLerped = 0;

    void Start() {
        startPosition = transform.localPosition;
    }

    public void SetGateDirection(int direction) {
        startPosition = transform.localPosition;
        secondsLerped = 0;
        gateDirection = direction;
    }
    
    void Update()
    {
        if (gateDirection != 0) {
            secondsLerped += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(
                startPosition,
                new Vector3(
                    startPosition.x, 
                    gateDirection < 0 ? 0 : maxHeight,
                    startPosition.z
                ),
                secondsLerped / secondsToLerp
            );
        }
    }
}
