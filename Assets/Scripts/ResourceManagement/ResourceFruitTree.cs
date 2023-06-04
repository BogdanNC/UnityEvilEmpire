using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceFruitTree : ResourceClass
{

    //TODO:Food and wood quantities
    [SerializeField] private int capacity = 300;
    [SerializeField] private int gatherRate = 20;

    public void Start()
    {
        //TODO: Init for wood and food missing
        Init(capacity, gatherRate);
    }

    /*public override int Gather()
    {
        int gathered = 0;

        gathered += Gather((gatherRate * 2) / 3);

        if(gathered >= gatherRate)
        {
            Debug.Log("Something went wrong gathering: " + this.name);
            return gathered;
        }

        gathered += Gather(gatherRate / 3);

        return gathered;
    }*/
}
