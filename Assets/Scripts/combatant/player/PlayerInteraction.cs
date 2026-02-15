using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerEquipment equipment;

    void Start() {
        this.equipment = GetComponentInParent<PlayerEquipment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            IInteractable interactable = null;

            Vector3 origin = transform.position + transform.forward * 0.1f;
            Ray ray = new Ray(origin, transform.forward);
            RaycastHit[] hits = new RaycastHit[10];
            int hitCount = Physics.SphereCastNonAlloc(
                ray,
                0.5f,
                hits,
                4f
            );

            for (int i = 0; i < hitCount; i++) {
                interactable = hits[i].transform.GetComponentInParent<IInteractable>();
                if (interactable != null) {
                    Debug.Log("SPHERE CAST GOT INTERACTABLE!"); 
                    break;
                }
            }
            if (interactable == null) {
                Collider[] overlaps = Physics.OverlapSphere(transform.position, 2f);
                foreach (Collider col in overlaps) {
                    interactable = col.GetComponentInParent<IInteractable>();
                    if (interactable != null) {
                        Debug.Log("OVERLAP GOT INTERACTABLE!");
                        break;
                    }
                }
            }

            if (interactable != null) {
                interactable.Interact(equipment); 
            }
        }
    }
}
