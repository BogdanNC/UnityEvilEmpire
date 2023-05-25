using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceFood : ResourceClass
{
    [SerializeField] private int capacity = 100;
    [SerializeField] private int gatherRate = 15;

    private void Start()
    {
        Init(capacity, gatherRate);
    }
}
