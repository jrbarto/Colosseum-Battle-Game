using UnityEngine;

public class GateCloseSensor : MonoBehaviour
{
    public GameObject gate;
    
    void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            gate.GetComponentInParent<GateControl>().SetGateDirection(-1);
        }
    }
}
