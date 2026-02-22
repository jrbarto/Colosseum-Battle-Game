using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Game.Resources;

public class PickupObject : MonoBehaviour, IInteractable
{
    public Color glowColor = Color.yellow;
    public float glowIntensity = 3f;
    public float hoverHeight = 2.2f;
    public float hoverForce = 25f;
    public float damping = 6f;
    private Rigidbody rigidBody;
    private bool glowEnabled;
    private PlayerMessages playerMessages;

    public void Interact (PlayerEquipment equipment) {
        WeaponParts parts = GetComponent<WeaponParts>();
        if (parts != null && equipment != null) {
            Dictionary<ResourceType, int> resources = parts.GetWeaponResources();
            int count = resources.Count;
            int index = 0;
            string message = "You picked up ";
            foreach (KeyValuePair<ResourceType, int> entry in resources) {
                if (index != 0) {
                    if (index == count - 1) {
                        message += ", and ";
                    } else {
                        message += ", ";
                    }
                }
                message += $"{entry.Value} {entry.Key}";
                equipment.resources[entry.Key] += entry.Value;
                index++;
            }

            // notify player of acquired resources
            if (playerMessages != null) {
                playerMessages.ShowMessage(message);
                GameObject.Destroy(gameObject);
            }
        }
    }

    void Awake () {
        rigidBody = GetComponent<Rigidbody>();
        playerMessages = GameObject.FindWithTag("PlayerMessages").GetComponent<PlayerMessages>();
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

    public void ToggleGlowHierarchy(bool glowEnabled) {
        StartCoroutine(WaitBeforeActivation(glowEnabled));
    }

    void ToggleGlow(Transform parent, bool glowEnabled) {
        foreach (Transform child in parent) {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend != null) {
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
