using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceClass : MonoBehaviour
{
    private int quantity;
    private int gatherRate;

    public void Init(int capacity, int gatherRate)
    {
        quantity = capacity;
        this.gatherRate = gatherRate;
    }

    public virtual int Gather()
    {
        int gathered = 0;

        if (quantity >= gatherRate)
        {
            quantity -= gatherRate;
            gathered = gatherRate;
        }
        else if (quantity > 0)
        {
            gathered = quantity;
            quantity = 0;
        }

        if (quantity <= 0)
        {
            Destroy(gameObject);
        }

        return gathered;
    }

    public virtual int Gather(int limit)
    {
        int gathered = 0;

        if(limit >= gatherRate)
        {
            return Gather();
        }

        if(quantity <= limit)
        {
            gathered += quantity;
            quantity = 0;
        }

        if(quantity > limit)
        {
            gathered += limit;
            quantity -= limit;
        }
        
        return gathered;
    }
}