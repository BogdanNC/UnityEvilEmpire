using System;
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
    
    public static explicit operator ResourceClass(GameObject v)
    {
        // it gives the serialized error but it works
        if (v.tag == "Wood")
        {
            ResourceWood wood = v.GetComponent<ResourceWood>() ;
            return wood;
        }
        if (v.tag == "Gold")
        {
            ResourceGold gold = v.GetComponent<ResourceGold>();
            return gold;
        }
        if (v.tag == "Food")
        {
            if (v.GetComponent<ResourceFood>() != null)
            {
                ResourceFood food = v.GetComponent<ResourceFood>();
                return food;
            }
            if (v.GetComponent<ResourceFruitTree>() != null)
            {
                ResourceFruitTree food = v.GetComponent<ResourceFruitTree>();
                return food;
            }
        }
        return null;
    }
}
