using System.Collections;
using UnityEngine;

public class PickupObject : MonoBehaviour, IInteractable
{
    public Color glowColor = Color.yellow;
    public float glowIntensity = 3f;
    public float hoverHeight = 2.2f;
    public float hoverForce = 25f;
    public float damping = 6f;
    private Rigidbody rigidBody;
    private bool glowEnabled;

    public void Interact () {
        Debug.Log("Interacting with the pickup object!");
    }

    void Awake () {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        if (this.glowEnabled) {
            rigidBody.constraints = RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotation;
            Vector3 origin = transform.position;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, hoverHeight * 2f))
            {
                float bob = Mathf.Sin(Time.fixedTime * 2f) * 0.25f;
                float targetHeight = hoverHeight + bob;

                float heightError = targetHeight - hit.distance;
                float upwardVelocity = rigidBody.linearVelocity.y;
                float lift = (heightError * hoverForce) - (upwardVelocity * damping);

                rigidBody.AddForce(Vector3.up * lift, ForceMode.Acceleration);
            }
        }
    }

    IEnumerator WaitBeforeActivation (bool glowEnabled) {
        yield return new WaitForSeconds(3);
        ToggleGlow(transform, glowEnabled);
        this.glowEnabled = glowEnabled;
        Collider collider = GetComponent<Collider>();
        if (collider != null) {
            collider.isTrigger = true;
        }
    }

    public void ToggleGlowHierarchy(bool glowEnabled)
    {
        StartCoroutine(WaitBeforeActivation(glowEnabled));
    }

    void ToggleGlow(Transform parent, bool glowEnabled)
    {
        foreach (Transform child in parent)
        {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend != null)
            {
                foreach (Material mat in rend.materials)
                {
                    if (glowEnabled) {
                        mat.EnableKeyword("_EMISSION");
                        mat.SetColor("_EmissionColor", glowColor * glowIntensity);
                    } else {
                        mat.DisableKeyword("_EMISSION");
                    }
                    
                }
            }

            // Recursive call for all children
            ToggleGlow(child, glowEnabled);
        }
    }
}
