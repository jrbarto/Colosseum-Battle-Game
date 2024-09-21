using UnityEngine;

public class Utils : MonoBehaviour
{
    public static GameObject FindChildByName(Transform parent, string childName) {
        foreach (Transform child in parent) {
            if (child.name == childName) {
                return child.gameObject;
            }
            GameObject result = FindChildByName(child, childName);
            if (result != null) {
                return result;
            }
        }
        return null;
    }
}
