using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceWood : ResourceClass
{
    [SerializeField] private int capacity = 150;
    [SerializeField] private int gatherRate = 20;

    private void Start()
    {
        Init(capacity, gatherRate);
    }
}
