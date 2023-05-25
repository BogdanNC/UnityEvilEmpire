using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGold : ResourceClass
{
    [SerializeField] private int capacity = 200;
    [SerializeField] private int gatherRate = 10;

    private void Start()
    {
        Init(capacity, gatherRate);
    }
}
