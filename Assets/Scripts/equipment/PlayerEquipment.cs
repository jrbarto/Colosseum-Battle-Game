using UnityEngine;
using System.Collections.Generic;
using Game.Resources;
using System;

public class PlayerEquipment : MonoBehaviour
{
    public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType))) {
            resources[resourceType] = 0;
        }
    }
}
