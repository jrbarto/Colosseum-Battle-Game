using UnityEngine;

public class DynamicWeaponCollider : MonoBehaviour
{
    public GameObject topMarker;
    public GameObject bottomMarker;
    public BoxCollider boxCollider;

    void Update()
    {
        // distance between two markers
        float bladeLength = Vector3.Distance(topMarker.transform.position, bottomMarker.transform.position);

        // change y scale of collider to match length between the markers
        Vector3 newColliderSize = boxCollider.size;
        newColliderSize.y = bladeLength;
        boxCollider.size = newColliderSize;

        // reposition the collider center between the two markers
        Vector3 newColliderCenter = (topMarker.transform.position + bottomMarker.transform.position) / 2;
        boxCollider.center = transform.InverseTransformPoint(newColliderCenter);
    }

}
