using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceFruitTree : ResourceClass
{
    ResourceWood wood;
    ResourceFood food;

    //TODO:Food and wood quantities

    private int gatherRate = 20;

    public void Start()
    {
        //TODO: Init for wood and food missing
    }

    public override int Gather()
    {
        int gathered = 0;

        gathered += wood.Gather((gatherRate * 2) / 3);

        if(gathered >= gatherRate)
        {
            Debug.Log("Something went wrong gathering: " + this.name);
            return gathered;
        }

        gathered += food.Gather(gatherRate / 3);

        return gathered;
    }
}
